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

        public MainPageViewModel()
        {
            TryApiCommand = new DelegateCommand(Search);
        }

        public async void Search()
        {
            //SearchByTitle("the lord of the rings");
            SearchByAuthor("tolkien");
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
