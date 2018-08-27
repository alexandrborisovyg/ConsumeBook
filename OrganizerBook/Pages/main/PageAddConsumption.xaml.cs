using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data.Entity;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace OrganizerBook.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageAddConsumption.xaml
    /// </summary>
    public partial class PageAddConsumption : Page
    {
        PageFilter filterWindow;
        ApplicationContext db;
        List<Consumption> consumptions;
        bool initialize = false;
        Consumption Consumption;

        public PageAddConsumption(PageFilter filterWindow)
        {
            InitializeComponent();

            this.filterWindow = filterWindow;

            consumptions = new List<Consumption>();

            db = new ApplicationContext();
            db.Consumptions.Load();
            db.Types.Load();
            db.SubTypes.Load();
            db.Users.Load();
            FillAddWindow();
        }

        public void CallPopup(string text)
        {
            popupMessage.Text = text;
            Storyboard s = (Storyboard)this.TryFindResource("ShowPopup");
            BeginStoryboard(s);
        }

        public void FillAddWindow()
        {
            using (ApplicationContext db1 = new ApplicationContext())
            {
                db1.Users.Load();
                db1.Types.Load();
                db1.SubTypes.Load();

                comboboxUserAddWindow.Items.Clear();
                comboboxTypeAddWindow.Items.Clear();
                comboboxSubTypeAddWindow.Items.Clear();

                var queryUser = from u in db1.Users.Local
                                select new
                                {
                                    userId = u.userId,
                                    Name = u.Name,
                                    Surname = u.Surname,
                                    ShowName = u.Name + " " + u.Surname
                                };
                var queryType = from t in db1.Types.Local
                                select new
                                {
                                    typeId = t.typeId,
                                    typeName = t.TypeName
                                };
                var querySubOne = from s in db1.SubTypes.Local
                                  where s.subTypeId == 1
                                  select new
                                  {
                                      TypeId = s.TypeId,
                                      subTypeId = s.subTypeId,
                                      subTypeName = s.SubTypeName
                                  };

                var queryUserList = queryUser.ToList();
                for (int i = 0; i < queryUserList.Count; i++)
                {
                    comboboxUserAddWindow.Items.Add(queryUserList[i]);
                }

                var queryTypeList = queryType.ToList();
                for (int i = 0; i < queryTypeList.Count; i++)
                {
                    comboboxTypeAddWindow.Items.Add(queryTypeList[i]);
                }

                var querySubTypeList = querySubOne.ToList();
                for (int i = 0; i < querySubTypeList.Count; i++)
                {
                    comboboxSubTypeAddWindow.Items.Add(querySubTypeList[i]);
                }

                comboboxSubTypeAddWindow.SelectedIndex = 0;
                comboboxTypeAddWindow.SelectedIndex = 0;
                comboboxUserAddWindow.SelectedIndex = 0;
            }

            DataContext = Consumption;
            Consumption = new Consumption();
            textboxAddDate.Text = DateTime.Now.ToLongDateString();
            initialize = true;
            comboboxSubTypeAddWindow.SelectedIndex = 0;
            dynamic subtypeTemp = comboboxSubTypeAddWindow.SelectedItem;
            Consumption.SubTypeId = subtypeTemp.subTypeId;
        }

        private void Accept_ClickAddWindow(object sender, RoutedEventArgs e)
        {
            if (textboxAddValue.Text != "0")
            {
                int resultFromValue;
                Int32.TryParse(textboxAddValue.Text, out resultFromValue);
                if (resultFromValue == 0)
                {
                    CallPopup("Некорректные данные");
                    return;
                }
            }

            Consumption.Description = textboxAddDescription.Text;
            Consumption.Value = Int32.Parse(textboxAddValue.Text);
            Consumption.Date = DateTime.Parse(textboxAddDate.Text);

            dynamic usersTemp = comboboxUserAddWindow.SelectedItem;
            if (comboboxUserAddWindow.SelectedIndex >= 0 && initialize)
            {
                Consumption.UserId = usersTemp.userId;
            }

            db.Consumptions.Add(Consumption);
            db.SaveChanges();

            textboxAddDate.Text = DateTime.Today.ToShortDateString();
            textboxAddValue.Text = "0";
            textboxAddDescription.Text = "";

            comboboxTypeAddWindow.SelectedIndex = 0;
            comboboxUserAddWindow.SelectedIndex = 0;

            Consumption = new Consumption();

            dynamic subtypeTemp = comboboxSubTypeAddWindow.SelectedItem;
            Consumption.SubTypeId = subtypeTemp.subTypeId;

            filterWindow.sum = false;
            filterWindow.RefreshDataGrid();
            filterWindow.FillFilters();

            CallPopup("Трата добавлена");
        }

        private void SelectedTypeAddWindow(object sender, SelectionChangedEventArgs e)
        {
            if (comboboxTypeAddWindow.SelectedIndex >= 0 && initialize)
            {
                comboboxSubTypeAddWindow.Items.Clear();

                using (ApplicationContext db = new ApplicationContext())
                {
                    db.SubTypes.Load();

                    dynamic typeTemp = comboboxTypeAddWindow.SelectedItem;
                    var querySub = from s in db.SubTypes.Local
                                   where s.TypeId == typeTemp.typeId
                                   select new
                                   {
                                       typeId = s.TypeId,
                                       subTypeId = s.subTypeId,
                                       subTypeName = s.SubTypeName
                                   };

                    var querySubTypeList = querySub.ToList();
                    for (int i = 0; i < querySubTypeList.Count; i++)
                    {
                        comboboxSubTypeAddWindow.Items.Add(querySubTypeList[i]);
                    }

                    comboboxSubTypeAddWindow.SelectedIndex = 0;

                    dynamic subtypeTemp = comboboxSubTypeAddWindow.SelectedItem;
                    Consumption.SubTypeId = subtypeTemp.subTypeId;
                }
            }
        }

        private void SelectedSubtypeAddWindow(object sender, SelectionChangedEventArgs e)
        {
            if (comboboxSubTypeAddWindow.SelectedIndex >= 0 && initialize)
            {
                dynamic subtypeTemp = comboboxSubTypeAddWindow.SelectedItem;
                Consumption.SubTypeId = subtypeTemp.subTypeId;
            }
        }

        private void comboboxUserAddWindow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void textboxAddValue_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void cbtype_open(object sender, EventArgs e)
        {
            comboboxTypeAddWindow.Background = new SolidColorBrush(Color.FromRgb(156, 39, 176));
            comboboxTypeAddWindow.Foreground = Brushes.WhiteSmoke;
        }

        private void cbtype_close(object sender, EventArgs e)
        {
            comboboxTypeAddWindow.Background = Brushes.WhiteSmoke;
            comboboxTypeAddWindow.Foreground = Brushes.Black;
        }

        private void cbsubtype_close(object sender, EventArgs e)
        {
            comboboxSubTypeAddWindow.Background = Brushes.WhiteSmoke;
            comboboxSubTypeAddWindow.Foreground = Brushes.Black;
        }

        private void cbsubtype_open(object sender, EventArgs e)
        {
            comboboxSubTypeAddWindow.Background = new SolidColorBrush(Color.FromRgb(156, 39, 176));
            comboboxSubTypeAddWindow.Foreground = Brushes.WhiteSmoke;
        }

        private void cbuser_close(object sender, EventArgs e)
        {
            comboboxUserAddWindow.Background = Brushes.WhiteSmoke;
            comboboxUserAddWindow.Foreground = Brushes.Black;
        }

        private void cbuser_open(object sender, EventArgs e)
        {
            comboboxUserAddWindow.Background = new SolidColorBrush(Color.FromRgb(156, 39, 176));
            comboboxUserAddWindow.Foreground = Brushes.WhiteSmoke;
        }

        private void textboxAddValue_textinput(object sender, KeyEventArgs e)
        {
            if (textboxAddValue.Text == "0")
                textboxAddValue.Text = "";
        }

        private void textboxAddDate_mouseenter(object sender, MouseEventArgs e)
        {
            textboxAddDate.BorderBrush = Brushes.Black;
        }
    }
}
