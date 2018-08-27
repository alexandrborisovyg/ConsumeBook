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
    /// Логика взаимодействия для PageFilter.xaml
    /// </summary>
    public partial class PageFilter : Page
    {
        public bool sum = false;

        ApplicationContext db;
        List<Consumption> consumptions;
        DateTime date;
        int startYear = 2010, currentYear, shiftYear = 5;
        int stepChangeCombobox = 0, chooseMonth = -1, chooseDay = -1, chooseYear = -1;
        bool is_FilterActivated = false;

        public PageFilter()
        {
            InitializeComponent();

            consumptions = new List<Consumption>();
            date = DateTime.Today;
            currentYear = date.Year;

            db = new ApplicationContext();
            db.Consumptions.Load();
            db.Types.Load();
            db.SubTypes.Load();
            db.Users.Load();

            FillFilters();

            SetDate();
            SetSelectedIndice();
            comboboxDay.SelectedIndex = 0;

            RefreshDataGrid();
        }

        public void CallPopup(string text)
        {
            popupMessage.Text = text;
            Storyboard s = (Storyboard)this.TryFindResource("ShowPopup");
            BeginStoryboard(s);
        }

        public void FillFilters()
        {
            comboboxSubType.Items.Clear();
            comboboxType.Items.Clear();
            comboboxUser.Items.Clear();

            using (ApplicationContext db = new ApplicationContext())
            {
                db.Users.Load();
                db.Types.Load();

                var queryUser = from u in db.Users.Local
                                select new
                                {
                                    userId = u.userId,
                                    Name = u.Name,
                                    Surname = u.Surname,
                                    ShowName = u.Name + " " + u.Surname
                                };
                var queryType = from t in db.Types.Local
                                select new
                                {
                                    typeId = t.typeId,
                                    typeName = t.TypeName
                                };

                var bufUsers = queryUser.ToList();
                var NoChooseUser = new
                {
                    userId = 0,
                    Name = "",
                    Surname = "",
                    ShowName = "Не выбрано"
                };
                bufUsers.Insert(0, NoChooseUser);
                for (int i = 0; i < bufUsers.Count; i++)
                {
                    comboboxUser.Items.Add(bufUsers[i]);
                }

                var bufTypes = queryType.ToList();
                var NoChooseType = new
                {
                    typeId = 0,
                    typeName = "Не выбрано"
                };
                bufTypes.Insert(0, NoChooseType);
                for (int i = 0; i < bufTypes.Count; i++)
                {
                    comboboxType.Items.Add(bufTypes[i]);
                }

                comboboxUser.SelectedIndex = 0;
                comboboxType.SelectedIndex = 0;
            }
        }

        private void DropFilter_Click(object sender, RoutedEventArgs e)
        {
            textboxKeyWord.Text = "";
            textblockFromValue.Text = "0";
            textblockToValue.Text = "0";
            textblockFromPeriod.Text = "1/1/0001";
            textblockToPeriod.Text = "1/1/0001";
            comboboxType.SelectedIndex = 0;
            comboboxSubType.SelectedIndex = 0;
            comboboxUser.SelectedIndex = 0;
        }

        public void RefreshDataGrid()
        {
            using (ApplicationContext db1 = new ApplicationContext())
            {
                db1.Consumptions.Load();
                db1.Types.Load();
                db1.SubTypes.Load();
                db1.Users.Load();

                if (comboboxDay.SelectedIndex != 0 && comboboxMonth.SelectedIndex != 0)
                {
                    var resultGrid = from c in db1.Consumptions.Local
                                     join u in db1.Users.Local on c.UserId equals u.userId
                                     join s in db1.SubTypes.Local on c.SubTypeId equals s.subTypeId
                                     join t in db1.Types on s.TypeId equals t.typeId
                                     where comboboxDay.SelectedIndex == c.Date.Day &&
                                     comboboxMonth.SelectedIndex == c.Date.Month &&
                                     comboboxYear.SelectedIndex + startYear - 1 == c.Date.Year
                                     select new
                                     {
                                         ConsumptionId = c.consumptionId,
                                         DescriptionConsumption = c.Description,
                                         TypeName = t.TypeName,
                                         SubTypeName = s.SubTypeName,
                                         SubTypeId = s.subTypeId,
                                         ValueConsumption = c.Value,
                                         DateConsumption = c.Date,
                                         ShowDate = c.Date.ToShortDateString(),
                                         UserName = u.Name + " " + u.Surname,
                                         UserId = u.userId
                                     };
                    var listResult = resultGrid.ToList();
                    this.DataContext = listResult;
                    consumptionGrid.ItemsSource = listResult;
                }
                else
                if (comboboxDay.SelectedIndex == 0 && comboboxMonth.SelectedIndex != 0)
                {
                    var resultGrid = from c in db1.Consumptions.Local
                                     join u in db1.Users.Local on c.UserId equals u.userId
                                     join s in db1.SubTypes.Local on c.SubTypeId equals s.subTypeId
                                     join t in db1.Types on s.TypeId equals t.typeId
                                     where comboboxMonth.SelectedIndex == c.Date.Month &&
                                     comboboxYear.SelectedIndex + startYear - 1 == c.Date.Year
                                     select new
                                     {
                                         ConsumptionId = c.consumptionId,
                                         DescriptionConsumption = c.Description,
                                         TypeName = t.TypeName,
                                         SubTypeName = s.SubTypeName,
                                         SubTypeId = s.subTypeId,
                                         ValueConsumption = c.Value,
                                         DateConsumption = c.Date,
                                         ShowDate = c.Date.ToShortDateString(),
                                         UserName = u.Name + " " + u.Surname,
                                         UserId = u.userId
                                     };
                    var listResult = resultGrid.ToList();
                    this.DataContext = listResult;
                    consumptionGrid.ItemsSource = listResult;
                }
                else if (comboboxYear.SelectedIndex != 0)
                {
                    var resultGrid = from c in db1.Consumptions.Local
                                     join u in db1.Users.Local on c.UserId equals u.userId
                                     join s in db1.SubTypes.Local on c.SubTypeId equals s.subTypeId
                                     join t in db1.Types on s.TypeId equals t.typeId
                                     where comboboxYear.SelectedIndex + startYear - 1 == c.Date.Year
                                     select new
                                     {
                                         ConsumptionId = c.consumptionId,
                                         DescriptionConsumption = c.Description,
                                         TypeName = t.TypeName,
                                         SubTypeName = s.SubTypeName,
                                         SubTypeId = s.subTypeId,
                                         ValueConsumption = c.Value,
                                         DateConsumption = c.Date,
                                         ShowDate = c.Date.ToShortDateString(),
                                         UserName = u.Name + " " + u.Surname,
                                         UserId = u.userId
                                     };
                    var listResult = resultGrid.ToList();
                    this.DataContext = listResult;
                    consumptionGrid.ItemsSource = listResult;
                }
                else
                {
                    var resultGrid = from c in db1.Consumptions.Local
                                     join u in db1.Users.Local on c.UserId equals u.userId
                                     join s in db1.SubTypes.Local on c.SubTypeId equals s.subTypeId
                                     join t in db1.Types on s.TypeId equals t.typeId
                                     select new
                                     {
                                         ConsumptionId = c.consumptionId,
                                         DescriptionConsumption = c.Description,
                                         TypeName = t.TypeName,
                                         SubTypeName = s.SubTypeName,
                                         SubTypeId = s.subTypeId,
                                         ValueConsumption = c.Value,
                                         DateConsumption = c.Date,
                                         ShowDate = c.Date.ToShortDateString(),
                                         UserName = u.Name + " " + u.Surname,
                                         UserId = u.userId
                                     };
                    var listResult = resultGrid.ToList();
                    this.DataContext = listResult;
                    consumptionGrid.ItemsSource = listResult;
                }

                var column = consumptionGrid.Columns[0];

                consumptionGrid.Items.SortDescriptions.Clear();

                consumptionGrid.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription(column.SortMemberPath, System.ComponentModel.ListSortDirection.Descending));

                // Apply sort
                foreach (var col in consumptionGrid.Columns)
                {
                    col.SortDirection = null;
                }
                column.SortDirection = System.ComponentModel.ListSortDirection.Descending;
                consumptionGrid.Items.Refresh();

                if (!sum)
                {
                    BindingMainWindow filtersElements = (BindingMainWindow)this.Resources["filtersElements"];
                    filtersElements.Sum = 0;
                    foreach (var row in consumptionGrid.Items)
                    {
                        dynamic tempRow = row;
                        filtersElements.Sum += tempRow.ValueConsumption;
                    }
                    sum = true;

                  //  DropFilter_Click(null, null);
                }
            }
        }

        private void ClearComboboxes()
        {
            comboboxDay.Items.Clear();
            comboboxMonth.Items.Clear();
            comboboxYear.Items.Clear();
        }

        private void SetSelectedIndice()
        {
            if (chooseDay == 0)
            {
                comboboxDay.SelectedIndex = 0;
            }
            else
            {
                comboboxDay.SelectedIndex = date.Day;
            }

            if (chooseMonth == 0)
            {
                comboboxMonth.SelectedIndex = 0;
            }
            else
            {
                comboboxMonth.SelectedIndex = date.Month;
            }

            if (chooseYear == 0)
            {
                comboboxYear.SelectedIndex = 0;
            }
            else
            {
                comboboxYear.SelectedIndex = date.Year - startYear + 1;
            }
        }

        private void SetDate()
        {
            comboboxDay.Items.Add("Не выбрано");
            for (int i = 1; i <= DateTime.DaysInMonth(date.Year, date.Month); i++)
            {
                comboboxDay.Items.Add(i);
            }

            comboboxMonth.Items.Add("Не выбрано");
            CultureInfo ci = new CultureInfo("ru-RU");
            DateTimeFormatInfo dateTimeFormatInfo = ci.DateTimeFormat;
            for (int i = 0; i < 12; i++)
            {
                string Month = dateTimeFormatInfo.MonthGenitiveNames[i].ToString();
                comboboxMonth.Items.Add(Month);
            }

            comboboxYear.Items.Add("Не выбрано");
            for (int i = startYear; i < currentYear + shiftYear; i++)
            {
                comboboxYear.Items.Add(i);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            CallPopup("Трата удалена");

            // если ни одного объекта не выделено, выходим
            if (consumptionGrid.SelectedItem == null) return;
            // получаем выделенный объект
            dynamic tempConsumption = consumptionGrid.SelectedItem;
            int consumptionId = tempConsumption.ConsumptionId;
            Consumption consumption = db.Consumptions
                .Where(c => c.consumptionId == consumptionId)
                .FirstOrDefault();

            db.Consumptions.Remove(consumption);
            db.SaveChanges();

            sum = false;
            if (is_FilterActivated == false)
                RefreshDataGrid();
            else AcceptFilter_Click(sender, e);
        }

        private void RightClick(object sender, RoutedEventArgs e)
        {
            if (comboboxDay.SelectedIndex != 0 && comboboxMonth.SelectedIndex != 0)
            {
                if (comboboxDay.SelectedIndex == DateTime.DaysInMonth(date.Year, date.Month))
                {
                    comboboxDay.SelectedIndex = 1;
                }
                else
                {
                    comboboxDay.SelectedIndex++;
                }
            }
            if (comboboxDay.SelectedIndex == 0 && comboboxMonth.SelectedIndex != 0)
            {
                if (comboboxMonth.SelectedIndex == 12)
                {
                    comboboxMonth.SelectedIndex = 1;
                }
                else
                {
                    comboboxMonth.SelectedIndex++;
                }
            }
            if (comboboxDay.SelectedIndex == 0 && comboboxMonth.SelectedIndex == 0)
            {
                if (comboboxYear.SelectedIndex == comboboxYear.Items.Count - 1)
                {
                    comboboxYear.SelectedIndex = 0;
                }
                else
                {
                    comboboxYear.SelectedIndex++;
                }
            }
        }

        private void AcceptFilter_Click(object sender, RoutedEventArgs e)
        {
            int resultFromValue, resultToValue;

            if (textblockFromValue.Text != "0")
            {
                Int32.TryParse(textblockFromValue.Text, out resultFromValue);
                if ( resultFromValue == 0)
                {
                    CallPopup("Некорректные\nданные в фильтре");
                    return;
                }
            }
            if (textblockToValue.Text != "0")
            {
                Int32.TryParse(textblockToValue.Text, out resultToValue);
                if (resultToValue == 0)
                {
                    CallPopup("Некорректные\nданные в фильтре");
                    return;
                }
            }

            using (ApplicationContext db1 = new ApplicationContext())
            {
                comboboxMonth.SelectedIndex = comboboxYear.SelectedIndex = 0;

                db1.Consumptions.Load();
                db1.Types.Load();
                db1.SubTypes.Load();
                db1.Users.Load();

                dynamic tempSubtype = comboboxSubType.Items[comboboxSubType.SelectedIndex];
                int tempSubtypeId = tempSubtype.subTypeId;

                dynamic tempType = comboboxType.Items[comboboxType.SelectedIndex];
                int tempTypeId = tempType.typeId;

                dynamic tempUser = comboboxUser.Items[comboboxUser.SelectedIndex];
                int tempUserId = tempUser.userId;

                var resultGrid = from c in db1.Consumptions.Local
                                 join u in db1.Users.Local on c.UserId equals u.userId
                                 join s in db1.SubTypes.Local on c.SubTypeId equals s.subTypeId
                                 join t in db1.Types on s.TypeId equals t.typeId

                                 where (c.Description.Contains(textboxKeyWord.Text) == true || textboxKeyWord.Text == "") &&
                                       (
                                           Int32.Parse(textblockFromValue.Text) == 0 && Int32.Parse(textblockToValue.Text) == 0 ||
                                           (
                                           Int32.Parse(textblockToValue.Text) >= Int32.Parse(textblockFromValue.Text)
                                           &&
                                           c.Value >= Int32.Parse(textblockFromValue.Text) && c.Value <= Int32.Parse(textblockToValue.Text)
                                           )
                                       ) &&
                                       (
                                           textblockFromPeriod.Text == "1/1/0001" && textblockToPeriod.Text == "1/1/0001" ||
                                           (
                                           DateTime.Parse(textblockToPeriod.Text) >= DateTime.Parse(textblockFromPeriod.Text)
                                           &&
                                           c.Date >= DateTime.Parse(textblockFromPeriod.Text) && c.Date <= DateTime.Parse(textblockToPeriod.Text)
                                           )
                                       ) &&
                                       (t.typeId == tempTypeId || comboboxType.SelectedIndex == 0) &&
                                       (c.SubTypeId == tempSubtypeId || comboboxSubType.SelectedIndex == 0) &&
                                       (c.UserId == tempUserId || comboboxUser.SelectedIndex == 0)
                                 select new
                                 {
                                     ConsumptionId = c.consumptionId,
                                     DescriptionConsumption = c.Description,
                                     TypeName = t.TypeName,
                                     SubTypeName = s.SubTypeName,
                                     SubTypeId = s.subTypeId,
                                     ValueConsumption = c.Value,
                                     ShowDate = c.Date.ToShortDateString(),
                                     DateConsumption = c.Date,
                                     UserName = u.Name + " " + u.Surname,
                                     UserId = u.userId
                                 };
                var listResult = resultGrid.ToList();
                this.DataContext = listResult;
                consumptionGrid.ItemsSource = listResult;

                BindingMainWindow filtersElements = (BindingMainWindow)this.Resources["filtersElements"];
                filtersElements.Sum = 0;
                foreach (var row in consumptionGrid.Items)
                {
                    dynamic tempRow = row;
                    filtersElements.Sum += tempRow.ValueConsumption;
                }
                is_FilterActivated = true;
            }

            var column = consumptionGrid.Columns[0];

            consumptionGrid.Items.SortDescriptions.Clear();

            consumptionGrid.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription(column.SortMemberPath, System.ComponentModel.ListSortDirection.Descending));

            // Apply sort
            foreach (var col in consumptionGrid.Columns)
            {
                col.SortDirection = null;
            }
            column.SortDirection = System.ComponentModel.ListSortDirection.Descending;
            consumptionGrid.Items.Refresh();
        }

        private void SettingsShow(object sender, RoutedEventArgs e)
        {
            Settings newWindow = new Settings();
            newWindow.ShowDialog();
            db.Consumptions.Load();
            db.Types.Load();
            db.SubTypes.Load();
            db.Users.Load();
            FillFilters();           
            RefreshDataGrid();           

        }

        private void typeChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboboxType.SelectedIndex >= 0)
            {
                comboboxSubType.Items.Clear();

                using (ApplicationContext db = new ApplicationContext())
                {
                    db.SubTypes.Load();

                    var querySub = from s in db.SubTypes.Local
                                   where s.TypeId == comboboxType.SelectedIndex
                                   select new
                                   {
                                       typeId = s.TypeId,
                                       subTypeId = s.subTypeId,
                                       subTypeName = s.SubTypeName
                                   };

                    var bufSubtypes = querySub.ToList();
                    var NoChooseSubtype = new
                    {
                        typeId = 0,
                        subTypeId = 0,
                        subTypeName = "Не выбрано"
                    };
                    bufSubtypes.Insert(0, NoChooseSubtype);
                    for (int i = 0; i < bufSubtypes.Count; i++)
                    {
                        comboboxSubType.Items.Add(bufSubtypes[i]);
                    }

                    comboboxSubType.SelectedIndex = 0;
                }
            }
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            SumInMenu.Visibility = Visibility.Collapsed;
            ItemSum.Visibility = Visibility.Visible;
            ItemKeyWord.Visibility = Visibility.Visible;
            ItemValue.Visibility = Visibility.Visible;
            ItemDate.Visibility = Visibility.Visible;
            ItemType.Visibility = Visibility.Visible;
            ItemUser.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            AcceptFilter.Visibility = Visibility.Visible;
            DropFilter.Visibility = Visibility.Visible;

            DropFilterKeyWord.Visibility = DropFilterValue.Visibility = DropFilterDate.Visibility = DropFilterSubType.Visibility = DropFilterUser.Visibility = Visibility.Visible;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            SumInMenu.Visibility = Visibility.Visible;
            ItemSum.Visibility = Visibility.Collapsed;
            ItemKeyWord.Visibility = Visibility.Collapsed;
            ItemValue.Visibility = Visibility.Collapsed;
            ItemDate.Visibility = Visibility.Collapsed;
            ItemType.Visibility = Visibility.Collapsed;
            ItemUser.Visibility = Visibility.Collapsed;
            ItemSum.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
            AcceptFilter.Visibility = Visibility.Collapsed;
            DropFilter.Visibility = Visibility.Collapsed;

            DropFilterKeyWord.Visibility = DropFilterValue.Visibility = DropFilterDate.Visibility = DropFilterSubType.Visibility = DropFilterUser.Visibility = Visibility.Collapsed;
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // GridData.Width = this.ActualWidth;
        }

        private void Datepicker_mouseenter_To(object sender, MouseEventArgs e)
        {
            textblockToPeriod.BorderBrush = System.Windows.Media.Brushes.Black;
        }

        private void Datepicker_mouseenter_From(object sender, MouseEventArgs e)
        {
            textblockFromPeriod.BorderBrush = System.Windows.Media.Brushes.Black;
        }

        private void textblockFromPeriod_CalendarClosed(object sender, RoutedEventArgs e)
        {
            textblockToPeriod.Foreground = Brushes.White;
            textblockFromPeriod.Foreground = Brushes.White;
        }

        private void DropFilterSubtype_Click(object sender, RoutedEventArgs e)
        {
            comboboxSubType.SelectedIndex = 0;
            comboboxType.SelectedIndex = 0;
        }

        private void DropFilterDate_Click(object sender, RoutedEventArgs e)
        {
            textblockFromPeriod.Text = textblockToPeriod.Text = "1/1/0001";
        }

        private void DropFilterValue_Click(object sender, RoutedEventArgs e)
        {
            textblockFromValue.Text = textblockToValue.Text = "0";
        }

        private void DropFilterKeyWord_Click(object sender, RoutedEventArgs e)
        {
            textboxKeyWord.Text = "";
        }

        private void DropFilterUser_Click(object sender, RoutedEventArgs e)
        {
            comboboxUser.SelectedIndex = 0;
        }

        private void textblockFromValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (textblockFromValue.Text == "0")
                textblockFromValue.Text = "";
        }

        private void textblockToValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (textblockToValue.Text == "0")
                textblockToValue.Text = "";
        }

        private void textblockFromPeriod_CalendarOpened(object sender, RoutedEventArgs e)
        {
            if(textblockFromPeriod.Text == "1/1/0001" && textblockToPeriod.Text == "1/1/0001" )
            {
                textblockFromPeriod.Text = DateTime.Today.ToShortDateString();
                textblockToPeriod.Text = DateTime.Today.ToShortDateString();
            }
            textblockToPeriod.Foreground = Brushes.Black;
            textblockFromPeriod.Foreground = Brushes.Black;
        }

        private void consumptionGrid_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // если ни одного объекта не выделено, выходим
            if (consumptionGrid.SelectedItem == null) return;
            // получаем выделенный объект

            dynamic tempConsumption = consumptionGrid.SelectedItem;
            Consumption consumption = new Consumption();
            consumption.consumptionId = tempConsumption.ConsumptionId;
            consumption.Date = tempConsumption.DateConsumption;
            consumption.Description = tempConsumption.DescriptionConsumption;
            consumption.SubTypeId = tempConsumption.SubTypeId;
            consumption.UserId = tempConsumption.UserId;
            consumption.Value = tempConsumption.ValueConsumption;

            ConsumptionWindow consumptionWindow = new ConsumptionWindow(new Consumption
            {
                consumptionId = consumption.consumptionId,
                Description = consumption.Description,
                SubTypeId = consumption.SubTypeId,
                Value = consumption.Value,
                Date = consumption.Date,
                UserId = consumption.UserId
            });

            if (consumptionWindow.ShowDialog() == true)
            {
                // получаем измененный объект
                consumption = db.Consumptions.Find(consumptionWindow.Consumption.consumptionId);
                if (consumption != null)
                {
                    consumption.consumptionId = consumptionWindow.Consumption.consumptionId;
                    consumption.Description = consumptionWindow.Consumption.Description;
                    consumption.SubTypeId = consumptionWindow.Consumption.SubTypeId;
                    consumption.Value = consumptionWindow.Consumption.Value;
                    consumption.Date = consumptionWindow.Consumption.Date;
                    consumption.UserId = consumptionWindow.Consumption.UserId;

                    db.Entry(consumption).State = EntityState.Modified;
                    db.SaveChanges();
                }
                CallPopup("Трата изменена");
            }
            sum = false;
            if (is_FilterActivated == false)
                RefreshDataGrid();
            else AcceptFilter_Click(sender, e);
        }

        private void YearChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboboxYear.SelectedIndex != -1)
            {
                if (date.Day > DateTime.DaysInMonth(comboboxYear.SelectedIndex + startYear - 1, date.Month))
                {
                    date = new DateTime(comboboxYear.SelectedIndex + startYear - 1, date.Month, 1);
                }
                else
                {
                    date = new DateTime(comboboxYear.SelectedIndex + startYear - 1, date.Month, date.Day);
                }
            }

            if (stepChangeCombobox == 0)
            {
                chooseYear = comboboxYear.SelectedIndex;
                chooseMonth = comboboxMonth.SelectedIndex;
                chooseDay = comboboxDay.SelectedIndex;
                stepChangeCombobox = 1;
                ClearComboboxes();
                stepChangeCombobox = 2;
            }
            if (stepChangeCombobox == 2)
            {
                SetDate();
                stepChangeCombobox = 3;
                SetSelectedIndice();
                stepChangeCombobox = 0;
                sum = false;
            }

            if (comboboxYear.SelectedIndex == 0)
            {
                comboboxDay.SelectedIndex = 0;
                comboboxMonth.SelectedIndex = 0;
            }

            is_FilterActivated = false;
            RefreshDataGrid();
        }

        private void LeftDate_Click(object sender, RoutedEventArgs e)
        {
            if (comboboxDay.SelectedIndex != 0 && comboboxMonth.SelectedIndex != 0)
            {
                comboboxDay.SelectedIndex--;
                if (comboboxDay.SelectedIndex == 0)
                {
                    comboboxDay.SelectedIndex = DateTime.DaysInMonth(date.Year, date.Month);
                }
            }
            if (comboboxDay.SelectedIndex == 0 && comboboxMonth.SelectedIndex != 0)
            {
                comboboxMonth.SelectedIndex--;
                if (comboboxMonth.SelectedIndex == 0)
                {
                    comboboxMonth.SelectedIndex = 12;
                }
            }
            if (comboboxDay.SelectedIndex == 0 && comboboxMonth.SelectedIndex == 0)
            {
                if (comboboxYear.SelectedIndex + startYear == startYear)
                {
                    comboboxYear.SelectedIndex = comboboxYear.Items.Count - 1;
                }
                else
                {
                    comboboxYear.SelectedIndex--;
                }
            }
        }

        private void MonthChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboboxMonth.SelectedIndex > 0)
            {
                if (date.Day > DateTime.DaysInMonth(date.Year, comboboxMonth.SelectedIndex))
                {
                    date = new DateTime(date.Year, comboboxMonth.SelectedIndex, 1);
                }
                else
                {
                    date = new DateTime(date.Year, comboboxMonth.SelectedIndex, date.Day);
                }
            }

            if (stepChangeCombobox == 0)
            {
                chooseMonth = comboboxMonth.SelectedIndex;
                chooseDay = comboboxDay.SelectedIndex;
                stepChangeCombobox = 1;
                ClearComboboxes();
                stepChangeCombobox = 2;
            }
            if (stepChangeCombobox == 2)
            {
                SetDate();
                stepChangeCombobox = 3;
                SetSelectedIndice();
                stepChangeCombobox = 0;
                sum = false;
            }

            if (comboboxMonth.SelectedIndex == 0)
            {
                comboboxDay.SelectedIndex = 0;
                comboboxMonth.SelectedIndex = 0;
            }

            is_FilterActivated = false;
            RefreshDataGrid();
        }

        private void DayChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboboxDay.SelectedIndex > 0)
            {
                date = new DateTime(date.Year, date.Month, comboboxDay.SelectedIndex);
            }

            if (comboboxMonth.SelectedIndex == 0 && stepChangeCombobox != 1)
            {
                comboboxMonth.SelectedIndex = 1;
            }

            sum = false;
            is_FilterActivated = false;

            RefreshDataGrid();
        }
    }
}
