using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OrganizerBook.Pages.settings;

namespace OrganizerBook
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        subtype pageSubtype;
        type pageType;
        people pagePeople;
        general pageGeneral;
        int selectedTab;

        public Settings()
        {
            InitializeComponent();

            pageSubtype = new subtype();
            pageType = new type();
            pagePeople = new people();
            pageGeneral = new general();

            selectedTab = 0;

            buttonPeople.Background = Brushes.White;
            buttonPeople.Foreground = Brushes.Black;

            pagePeople.RefreshListBoxUser();
            Main.Content = pagePeople;

            gridCursor.Background = new SolidColorBrush(Color.FromRgb(156, 39, 176));
            gridCursor.Width = buttonPeople.ActualWidth;
        }

        private void peopleClick(object sender, RoutedEventArgs e)
        {
            pagePeople.RefreshListBoxUser();
            Main.Content = pagePeople;

            selectedTab = 0;
            gridCursor.Margin = new Thickness((gridCursor.Width * selectedTab), 0, 0, 0);

            buttonPeople.Background = Brushes.White;
            buttonPeople.Foreground = Brushes.Black;

            buttonType.Background = new SolidColorBrush(Color.FromRgb(156, 39, 176));
            buttonType.Foreground = Brushes.White;

            buttonSubtype.Background = new SolidColorBrush(Color.FromRgb(156, 39, 176));
            buttonSubtype.Foreground = Brushes.White;

            buttonGeneral.Background = new SolidColorBrush(Color.FromRgb(156, 39, 176));
            buttonGeneral.Foreground = Brushes.White;
        }

        private void typeClick(object sender, RoutedEventArgs e)
        {
            pageType.RefreshListBoxType();
            Main.Content = pageType;

            selectedTab = 1;
            gridCursor.Margin = new Thickness((gridCursor.Width * selectedTab), 0, 0, 0);

            buttonType.Background = Brushes.White;
            buttonType.Foreground = Brushes.Black;

            buttonPeople.Background = new SolidColorBrush(Color.FromRgb(156, 39, 176));
            buttonPeople.Foreground = Brushes.White;

            buttonSubtype.Background = new SolidColorBrush(Color.FromRgb(156, 39, 176));
            buttonSubtype.Foreground = Brushes.White;

            buttonGeneral.Background = new SolidColorBrush(Color.FromRgb(156, 39, 176));
            buttonGeneral.Foreground = Brushes.White;
        }

        private void subtypeClick(object sender, RoutedEventArgs e)
        {
            pageSubtype.comboboxTypeOfSubtype.Items.Clear();
            pageSubtype.InititalizeListComboboxSubtype();
            Main.Content = pageSubtype;

            selectedTab = 2;
            gridCursor.Margin = new Thickness((gridCursor.Width * selectedTab), 0, 0, 0);

            buttonSubtype.Background = Brushes.White;
            buttonSubtype.Foreground = Brushes.Black;

            buttonPeople.Background = new SolidColorBrush(Color.FromRgb(156, 39, 176));
            buttonPeople.Foreground = Brushes.White;

            buttonType.Background = new SolidColorBrush(Color.FromRgb(156, 39, 176));
            buttonType.Foreground = Brushes.White;

            buttonGeneral.Background = new SolidColorBrush(Color.FromRgb(156, 39, 176));
            buttonGeneral.Foreground = Brushes.White;
        }

        private void generalClick(object sender, RoutedEventArgs e)
        {
            Main.Content = pageGeneral;

            selectedTab = 3;
            gridCursor.Margin = new Thickness((gridCursor.Width * selectedTab), 0, 0, 0);

            buttonGeneral.Background = Brushes.White;
            buttonGeneral.Foreground = Brushes.Black;

            buttonPeople.Background = new SolidColorBrush(Color.FromRgb(156, 39, 176));
            buttonPeople.Foreground = Brushes.White;

            buttonType.Background = new SolidColorBrush(Color.FromRgb(156, 39, 176));
            buttonType.Foreground = Brushes.White;

            buttonSubtype.Background = new SolidColorBrush(Color.FromRgb(156, 39, 176));
            buttonSubtype.Foreground = Brushes.White;
        }

        private void SettingWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            gridCursor.Width = buttonPeople.ActualWidth;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
