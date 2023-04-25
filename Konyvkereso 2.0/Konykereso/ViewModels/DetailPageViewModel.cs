﻿using Konyvkereso.Models;
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

        private BookDetail _book;
        public BookDetail Book
        {
            get { return _book; }
            set { Set(ref _book, value); }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            string bookKey = (string)parameter;
            Book = await bookService.getDetailedBookInfo(bookKey);
            //Book.coverUrl = setCoverUrl(Book.covers[0], 2);

            await base.OnNavigatedToAsync(parameter, mode, state);
        }

        private string setCoverUrl(int coverID, int size)
        {
            switch(size)
            {
                case 0:
                    return String.Format("https://covers.openlibrary.org/b/id/{0}-S.jpg", coverID);
                case 1:
                    return String.Format("https://covers.openlibrary.org/b/id/{0}-M.jpg", coverID);
                case 2:
                    return String.Format("https://covers.openlibrary.org/b/id/{0}-L.jpg", coverID);
                default:
                    return String.Format("https://covers.openlibrary.org/b/id/{0}-M.jpg", coverID);
            }
        }
    }
}
