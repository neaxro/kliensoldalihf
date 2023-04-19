using Konyvkereso.Model;
using Konyvkereso.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.ApplicationModel.Activation;

namespace Konyvkereso.ViewModel
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

            foreach(var item in searchResult.Docs)
            {
                Results.Add(item);
            }
        }

        public async void SearchByAuthor(string author)
        {
            Results.Clear();
            var searchResult = await bookService.getBookByAuthorAsynch(author);

            foreach (var item in searchResult.Docs)
            {
                Results.Add(item);
            }
        }
    }
}
