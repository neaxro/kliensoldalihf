using Konyvkereso.Models;
using Konyvkereso.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;

namespace Konyvkereso.ViewModels
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

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            string bookPath = (string)parameter;

            return base.OnNavigatedToAsync(parameter, mode, state);
        }
    }
}
