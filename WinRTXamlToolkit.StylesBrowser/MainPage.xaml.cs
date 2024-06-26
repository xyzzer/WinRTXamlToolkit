﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace WinRTXamlToolkit.StylesBrowser
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void OnExitButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void OnTextBlockStylesButtonClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(TextBlockStylesPage));
        }

        private void OnRichTextBlockStylesButtonClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RichTextBlockStylesPage));
        }

        private void OnAppBarButtonStylesButtonClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AppBarButtonStylesPage));
        }

        private void OnButtonStylesButtonClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ButtonStylesPage));
        }

        private void OnBrushesButtonClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BrushesPage));
        }
    }
}
