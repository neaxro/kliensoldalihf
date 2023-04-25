using Konyvkereso.Models;
using Konyvkereso.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Context;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Template10.Utils;
using Windows.UI.Xaml.Controls;
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
        private int currentPage = 0;
        private string _pageText = "0/0";

        // The Paging information
        public event PropertyChangedEventHandler PropertyChanged;
        public string PageText
        {
            get { return _pageText; }
            set
            {
                _pageText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageText)));
            }
        }
        
        // Search text
        private string _searchText = "";
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

        /// <summary>
        /// Starts a search
        /// </summary>
        private async void Search()
        {
            if (SearchText.Length == 0) return;
            if(currentPage == 0)
            {
                currentPage = 1;
            }

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

        /// <summary>
        /// Starts a search based on book's title
        /// </summary>
        /// <param name="author">The name of the author</param>
        /// <param name="pageNumber">The current page</param>
        /// <param name="sortMethod">The current sorting method</param>
        /// <returns></returns>
        private async Task SearchByTitle(string title, int pageNumber, SortCategories sortMethod)
        {
            var searchResult = await bookService.getBookByTitleAsynch(title, pageNumber, sortMethod);
            UpdatePagingInfo(searchResult);

            Results.Clear();
            Results.AddRange<Docs>(searchResult.Docs);
        }

        /// <summary>
        /// Starts a search based on book's author
        /// </summary>
        /// <param name="author">The name of the author</param>
        /// <param name="pageNumber">The current page</param>
        /// <param name="sortMethod">The current sorting method</param>
        /// <returns></returns>
        private async Task SearchByAuthor(string author, int pageNumber, SortCategories sortMethod)
        {
            var searchResult = await bookService.getBookByAuthorAsynch(author);
            UpdatePagingInfo(searchResult);

            Results.Clear();
            Results.AddRange<Docs>(searchResult.Docs);
        }

        /// <summary>
        /// The paging info will be updated so the view should be up to date
        /// </summary>
        /// <param name="result"></param>
        private void UpdatePagingInfo(SearchResult result)
        {
            if(result.Docs.Count > 0)
            {
                pageCount = result.Num_found / result.Docs.Count;
            }
            else
            {
                pageCount = 0;
                currentPage = 0;
            }

            PageText = String.Format("{0}/{1}", currentPage, pageCount);
        }

        #region Functions for the UI controllers

        /// <summary>
        /// Navigates to the Detail Screen
        /// </summary>
        /// <param name="bookPath"></param>
        public void NavigateToDetailsPage(string bookPath)
        {
            NavigationService.Navigate(typeof(Views.DetailPage), bookPath);
        }

        /// <summary>
        /// Pages forward
        /// </summary>
        public void PageForward()
        {
            if(currentPage != 0 && currentPage == pageCount)
            {
                currentPage = 1;
                Search();
            }
            else if(currentPage < pageCount)
            {
                currentPage++;
                Search();
            }
        }

        /// <summary>
        /// Pages backward
        /// </summary>
        public void PageBack()
        {
            if(currentPage != 0 && currentPage == 1)
            {
                currentPage = pageCount;
                Search();
            }
            else if (0 < currentPage)
            {
                currentPage--;
                Search();
            }
        }

        /// <summary>
        /// When the search button has been clicked it will search for books
        /// </summary>
        public void ButtonSearch()
        {
            currentPage = 1;
            Search();
        }

        /// <summary>
        /// Sets the search method
        /// </summary>
        /// <param name="category">The checkbox value</param>
        public void SearchCategoryChanged(string category)
        {
            switch (category)
            {
                case "Title":
                    SearchCategory = SearchCategories.Title;
                    break;

                case "Author":
                    SearchCategory = SearchCategories.Author;
                    break;

                default:
                    SearchCategory = SearchCategories.Title;
                    break;
            }
        }
        /// <summary>
        /// Sets the sorting method
        /// </summary>
        /// <param name="category">The checkbox value</param>
        public void SortingMethodChanged(string category)
        {
            switch (category)
            {
                case "Title":
                    SortCategory = SortCategories.Title;
                    break;

                case "Old":
                    SortCategory = SortCategories.Old;
                    break;

                case "New":
                    SortCategory = SortCategories.New;
                    break;

                case "Random":
                    SortCategory = SortCategories.Random;
                    break;

                default:
                    SortCategory = SortCategories.Title;
                    break;
            }
        }

        #endregion
    }
}
