using Konyvkereso.Models;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

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
            ViewModel.SearchCategoryChanged(category);
        }

        private void BookGroups_ItemClicked(object sender, ItemClickEventArgs e)
        {
            var selectedBook = (Docs)e.ClickedItem;
            ViewModel.NavigateToDetailsPage(selectedBook.Key);
        }

        private void PageBack_ButtonClicked(object sender, RoutedEventArgs e)
        {
            ViewModel.PageBack();
        }

        private void PageForward_ButtonClicked(object sender, RoutedEventArgs e)
        {
            ViewModel.PageForward();
        }

        private void SearchButton_ItemClicked(object sender, RoutedEventArgs e)
        {
            ViewModel.ButtonSearch();
        }

        private void SortCategoryCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string category = (sender as ComboBox).SelectedItem.ToString();
            ViewModel.SortingMethodChanged(category);
        }
    }
}
