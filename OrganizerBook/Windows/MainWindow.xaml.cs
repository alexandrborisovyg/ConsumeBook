using System;
using System.Windows;
using OrganizerBook.Pages;
using System.Windows.Media;
using System.Windows.Input;
using System.Data.Entity;
using System.Globalization;


namespace OrganizerBook
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ApplicationContext db = new ApplicationContext();
        PageFilter pageFilter;
        PageAddConsumption pageAdd;
        int selectedTab;

        public MainWindow()
        {
            InitializeComponent();

            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            double screenWidth = SystemParameters.FullPrimaryScreenWidth;

            this.Top = (screenHeight - this.Height) / 0x00000002;
            this.Left = (screenWidth - this.Width) / 0x00000002;

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
            pageAdd = new PageAddConsumption(pageFilter);
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


        private void SettingsShow(object sender, RoutedEventArgs e)
        {
            Settings newWindow = new Settings();
            newWindow.ShowDialog();
            db.Consumptions.Load();
            db.Types.Load();
            db.SubTypes.Load();
            db.Users.Load();
            pageFilter.RefreshDataGrid();
            pageFilter.FillFilters();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
