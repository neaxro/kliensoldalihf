using Konyvkereso.Model;
using Konyvkereso.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;

namespace Konyvkereso.ViewModel
{
    public class DetailPageViewModel : ViewModelBase
    {
        private readonly BookService bookService = new BookService();

        private Docs _book;
        public Docs Book
        {
            get { return _book; }
            set { _book = value; }
        }
    }
}
