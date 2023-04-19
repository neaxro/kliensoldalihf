using Konyvkereso.Model;
using Konyvkereso.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;

namespace Konyvkereso.ViewModel
{
    public class MainPageViewModel : ViewModelBase
    {
        public ObservableCollection<Docs> Results { get; set; } = new ObservableCollection<Docs>();
        private BookService bookService = new BookService();

        public DelegateCommand TryApiCommand { get; }

        public MainPageViewModel()
        {
            TryApiCommand = new DelegateCommand(TrySearch);
        }

        public async void TrySearch()
        {
            Results.Clear();
            var searchResult = await bookService.getBookWithTitleAsynch("the lord of the rings");

            foreach(var item in searchResult.Docs)
            {
                Results.Add(item);
            }
        }
    }
}
