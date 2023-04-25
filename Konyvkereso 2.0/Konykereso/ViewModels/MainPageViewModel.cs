using Konyvkereso.Models;
using Konyvkereso.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Template10.Utils;
using Windows.UI.Xaml.Navigation;

namespace Konyvkereso.ViewModels
{
    public class MainPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        // Service for API
        private BookService bookService = new BookService();

        // Enums
        public enum SearchCategories { Title, Author };
        public enum SortCategories { Old, New, Title, Random };

        // List of the books
        public ObservableCollection<Docs> Results { get; set; } = new ObservableCollection<Docs>();
        // Paging data
        private int pageCount = 0;
        private int currentPage = 1;
        private string _pageText = "0/0";
        public string PageText
        {
            get { return _pageText; }
            set
            {
                _pageText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageText)));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        // Search text
        private string _searchText = "Harry Potter";
        private string lastSearchText = "";
        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; }
        }

        // Searchable categories
        private SearchCategories _searchCategory;
        public SearchCategories SearchCategory
        {
            get { return _searchCategory; }
            set { _searchCategory = value; }
        }

        // Sortable categories
        private SortCategories _sortCategory;
        public SortCategories SortCategory
        {
            get { return _sortCategory; }
            set { _sortCategory = value; }
        }

        private async void Search()
        {
            switch (SearchCategory)
            {
                case SearchCategories.Title:
                    await SearchByTitle(SearchText, currentPage, SortCategory);
                    break;

                case SearchCategories.Author:
                    await SearchByAuthor(SearchText, currentPage, SortCategory);
                    break;
            } 
        }

        private async Task SearchByTitle(string title, int pageNumber, SortCategories sortMethod)
        {
            var searchResult = await bookService.getBookByTitleAsynch(title, pageNumber, sortMethod);
            UpdatePagingInfo(searchResult);

            Results.Clear();
            Results.AddRange<Docs>(searchResult.Docs);
        }

        private async Task SearchByAuthor(string author, int pageNumber, SortCategories sortMethod)
        {
            
            var searchResult = await bookService.getBookByAuthorAsynch(author);
            UpdatePagingInfo(searchResult);

            Results.Clear();
            Results.AddRange<Docs>(searchResult.Docs);
        }

        /// <summary>
        /// Navigates to the Detail Screen
        /// </summary>
        /// <param name="bookPath"></param>
        public void NavigateToDetailsPage(string bookPath)
        {
            NavigationService.Navigate(typeof(Views.DetailPage), bookPath);
        }

        private void UpdatePagingInfo(SearchResult result)
        {
            pageCount = result.Num_found / result.Docs.Count;
            PageText = String.Format("{0}/{1}", currentPage, pageCount);
        }

        public void PageForward()
        {
            if(currentPage < pageCount)
            {
                currentPage++;
                Search();
            }
        }

        public void PageBack()
        {
            if (0 < currentPage)
            {
                currentPage--;
                Search();
            }
        }

        public void ButtonSearch()
        {
            currentPage = 1;
            Search();
        }
    }
}
