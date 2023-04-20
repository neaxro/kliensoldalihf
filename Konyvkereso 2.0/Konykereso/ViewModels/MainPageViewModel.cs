using Konyvkereso.Models;
using Konyvkereso.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;

namespace Konyvkereso.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public ObservableCollection<Docs> Results { get; set; } = new ObservableCollection<Docs>();
        private BookService bookService = new BookService();
        public DelegateCommand TryApiCommand { get; }
        public enum SearchCategories { Title = 0, Author = 1 };

        public MainPageViewModel()
        {
            TryApiCommand = new DelegateCommand(Search);
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            return base.OnNavigatedToAsync(parameter, mode, state);
        }

        private string _searchText = "";
        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; }
        }

        private SearchCategories _searchCategory;
        public SearchCategories SearchCategory
        {
            get { return _searchCategory; }
            set { _searchCategory = value; }
        }

        public void Search()
        {
            switch (SearchCategory)
            {
                case SearchCategories.Title:
                    SearchByTitle(SearchText);
                    break;

                case SearchCategories.Author:
                    SearchByAuthor(SearchText);
                    break;
            }
        }

        public async void SearchByTitle(string title)
        {
            Results.Clear();
            var searchResult = await bookService.getBookByTitleAsynch(title);

            foreach (var item in searchResult.Docs)
            {
                item.CoverUrl = String.Format("https://covers.openlibrary.org/b/id/{0}-M.jpg", item.Cover_i);
                Results.Add(item);
            }
        }

        public async void SearchByAuthor(string author)
        {
            Results.Clear();
            var searchResult = await bookService.getBookByAuthorAsynch(author);

            foreach (var item in searchResult.Docs)
            {
                item.CoverUrl = String.Format("https://covers.openlibrary.org/b/id/{0}-M.jpg", item.Cover_i);
                Results.Add(item);

            }
        }

        public void NavigateToDetailsPage(string bookPath)
        {
            NavigationService.Navigate(typeof(Views.DetailPage), bookPath);
        }
    }
}
