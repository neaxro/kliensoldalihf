using Konyvkereso.Models;
using Konyvkereso.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.Media.Effects;
using Windows.UI.Xaml.Navigation;

namespace Konyvkereso.ViewModels
{
    public class DetailPageViewModel : ViewModelBase
    {
        // Service for API
        private readonly BookService bookService = new BookService();

        // Collection of authors related to the book
        public ObservableCollection<AuthorDetail> Authors { get; set; } = new ObservableCollection<AuthorDetail>();

        // The selected book
        private BookDetail _book;
        public BookDetail Book
        {
            get { return _book; }
            set { Set(ref _book, value); }
        }

        /// <summary>
        /// When navigated to the screen it will load the information about the book
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="mode"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            string bookKey = (string)parameter;
            Book = await bookService.getDetailedBookInfo(bookKey);

            await base.OnNavigatedToAsync(parameter, mode, state);
        }

        /// <summary>
        /// Opens a web page for the authors wikipedia or openlibrary page
        /// </summary>
        /// <param name="author">The author</param>
        public async void NavigateToAuthorWeb(AuthorDetail author)
        {
            string url;
            if (author.wikipedia == null)
            {
                url = String.Format("https://openlibrary.org{0}", author.key);
            }
            else
            {
                url = author.wikipedia;
            }

            if (url == null) return;
            await Windows.System.Launcher.LaunchUriAsync(new Uri(url));
        }
    }
}
