using System;
using System.Windows;
using OrganizerBook.Pages;
using System.Windows.Media;

namespace OrganizerBook
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PageFilter pageFilter;
        PageAddConsumption pageAdd;
        int selectedTab;

        public MainWindow()
        {
            InitializeComponent();

            pageFilter = new PageFilter();
            pageAdd = new PageAddConsumption(pageFilter);

            selectedTab = 0;
            buttonFilter.Background = Brushes.White;
            buttonFilter.Foreground = Brushes.Black;

            Main.Content = pageFilter;

            gridCursor.Background = new SolidColorBrush(Color.FromRgb(156, 39, 176));
            gridCursor.Width = buttonFilter.ActualWidth;
        }

        private void FilterClick(object sender, RoutedEventArgs e)
        {
            Main.Content = pageFilter;

            selectedTab = 0;
            gridCursor.Margin = new Thickness((gridCursor.Width * selectedTab), 0, 0, 0);

            buttonFilter.Background = Brushes.White;
            buttonFilter.Foreground = Brushes.Black;

            buttonAdd.Background = new SolidColorBrush(Color.FromRgb(156, 39, 176));
            buttonAdd.Foreground = Brushes.White;
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            pageAdd = new PageAddConsumption();
            Main.Content = pageAdd;
           
            selectedTab = 1;
            gridCursor.Margin = new Thickness((gridCursor.Width * selectedTab), 0, 0, 0);

            buttonAdd.Background = Brushes.White;
            buttonAdd.Foreground = Brushes.Black;

            buttonFilter.Background = new SolidColorBrush(Color.FromRgb(156, 39, 176));
            buttonFilter.Foreground = Brushes.White;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            gridCursor.Width = buttonFilter.ActualWidth;
        }
    }
}
