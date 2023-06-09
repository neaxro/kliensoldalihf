﻿using Konyvkereso.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Konyvkereso
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void SearchCategoryCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string category = (sender as ComboBox).SelectedItem.ToString();
            switch (category)
            {
                case "Title":
                    ViewModel.SearchCategory = Konyvkereso.ViewModel.MainPageViewModel.SearchCategories.Title;
                    break;

                case "Author":
                    ViewModel.SearchCategory = Konyvkereso.ViewModel.MainPageViewModel.SearchCategories.Author;
                    break;

                default:
                    ViewModel.SearchCategory = Konyvkereso.ViewModel.MainPageViewModel.SearchCategories.Title;
                    break;
            }
        }

        private void BookGroups_ItemClicked(object sender, ItemClickEventArgs e)
        {
            Debug.WriteLine("Item Clicked");
            var selectedBook = (Docs)e.ClickedItem;
            //this.Frame.Navigate(typeof(DetailPage), selectedBook);

            ViewModel.NavigateToDetailsPage(0);
        }
    }
}
