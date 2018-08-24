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
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace OrganizerBook
{
    /// <summary>
    /// Логика взаимодействия для ConsumptionWindow.xaml
    /// </summary>
    public partial class ConsumptionWindow : Window
    {
        public Consumption Consumption { get; private set; }
        bool initialize = false;
       
        public ConsumptionWindow(Consumption c)
        {
            InitializeComponent();
            Consumption = c;

            using (ApplicationContext db = new ApplicationContext())
            {
                initialize = false;
                db.Users.Load();
                db.Types.Load();
                db.SubTypes.Load();

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
                var querySubOne = from s in db.SubTypes.Local
                                   from t in db.Types.Local
                                   where Consumption.SubTypeId == s.subTypeId
                                   where t.typeId == s.TypeId
                                   select new
                               {
                                   TypeId = s.TypeId,
                                   subTypeId = s.subTypeId,
                                   subTypeName = s.SubTypeName
                               };

                var queryUserList = queryUser.ToList();
                for (int i = 0; i < queryUserList.Count; i++)
                {
                    comboboxUser.Items.Add(queryUserList[i]);
                }

                var queryTypeList = queryType.ToList();
                for (int i = 0; i < queryTypeList.Count; i++)
                {
                    comboboxType.Items.Add(queryTypeList[i]);
                }

                if (Consumption.consumptionId == 0)
                {
                    var querySubAnother = from s in db.SubTypes.Local
                                       where s.subTypeId == 1
                                       select new
                                       {
                                           TypeId = s.TypeId,
                                           subTypeId = s.subTypeId,
                                           subTypeName = s.SubTypeName,
                                       };
                    var buf = querySubAnother.ToList();
                    dynamic buffer = buf[0];
                    comboboxSubType.Items.Add(buffer);

                    comboboxUser.SelectedIndex = 0;
                    comboboxType.SelectedIndex = 0;
                    comboboxSubType.SelectedIndex = 0;

                    Consumption.Value = 0;
                    Consumption.SubTypeId = 1;
                    Consumption.UserId = 1;
                }
                else
                {
                    var buf = querySubOne.ToList();
                    dynamic tempSubOne = buf[0];

                    var querySubList = from s in db.SubTypes.Local
                                       join t in db.Types.Local on s.TypeId equals t.typeId
                                       where tempSubOne.TypeId == t.typeId
                                       select new
                                       {
                                           TypeId = s.TypeId,
                                           subTypeId = s.subTypeId,
                                           subTypeName = s.SubTypeName
                                       };

                    var querySubTypeList = querySubList.ToList();
                    for (int i = 0; i < querySubTypeList.Count; i++)
                    {
                        comboboxSubType.Items.Add(querySubTypeList[i]);
                        if (querySubTypeList[i].subTypeId == Consumption.SubTypeId)
                        {
                            comboboxType.SelectedIndex = querySubTypeList[i].TypeId - 1;
                            comboboxSubType.SelectedIndex = i;
                        }
                    }

                    for(int i = 0; i < comboboxUser.Items.Count; i++)
                    {
                        dynamic tempUser2 = comboboxUser.Items[i];
                        if(tempUser2.userId == Consumption.UserId)
                        {
                            comboboxUser.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
            this.textBoxDate.Text = Consumption.Date.ToShortDateString();
            this.DataContext = Consumption;

            initialize = true;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            Consumption.Date = DateTime.Parse(this.textBoxDate.Text);
            this.DialogResult = true;
        }

        private void SelectedUser(object sender, SelectionChangedEventArgs e)
        {
            dynamic usersTemp = comboboxUser.SelectedItem;
            if (comboboxUser.SelectedIndex >= 0 && initialize)
            {
                Consumption.UserId = usersTemp.userId;
            }
        }

        private void SelectedSubtype(object sender, SelectionChangedEventArgs e)
        {
            if (comboboxSubType.SelectedIndex >= 0 && initialize)
            {
                dynamic subtypeTemp = comboboxSubType.SelectedItem;
                Consumption.SubTypeId = subtypeTemp.subTypeId;
            }
        }

        private void SelectedType(object sender, SelectionChangedEventArgs e)
        {
            if (comboboxType.SelectedIndex >= 0 && initialize)
            {
                comboboxSubType.Items.Clear();

                using (ApplicationContext db = new ApplicationContext())
                {
                    db.SubTypes.Load();

                    dynamic typeTemp = comboboxType.SelectedItem;
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
                        comboboxSubType.Items.Add(querySubTypeList[i]);
                    }

                    comboboxSubType.SelectedIndex = 0;

                    dynamic subtypeTemp = comboboxSubType.SelectedItem;
                    Consumption.SubTypeId = subtypeTemp.subTypeId;
                }
            }
        }

        private void cbtype_open(object sender, EventArgs e)
        {
            comboboxType.Background = new SolidColorBrush(Color.FromRgb(156, 39, 176));
            comboboxType.Foreground = Brushes.WhiteSmoke;
        }

        private void cbtype_close(object sender, EventArgs e)
        {
            comboboxType.Background = Brushes.WhiteSmoke;
            comboboxType.Foreground = Brushes.Black;
        }

        private void cbsubtype_close(object sender, EventArgs e)
        {
            comboboxSubType.Background = Brushes.WhiteSmoke;
            comboboxSubType.Foreground = Brushes.Black;
        }

        private void cbsubtype_open(object sender, EventArgs e)
        {
            comboboxSubType.Background = new SolidColorBrush(Color.FromRgb(156, 39, 176));
            comboboxSubType.Foreground = Brushes.WhiteSmoke;
        }

        private void cbuser_close(object sender, EventArgs e)
        {
            comboboxUser.Background = Brushes.WhiteSmoke;
            comboboxUser.Foreground = Brushes.Black;
        }

        private void cbuser_open(object sender, EventArgs e)
        {
            comboboxUser.Background = new SolidColorBrush(Color.FromRgb(156, 39, 176));
            comboboxUser.Foreground = Brushes.WhiteSmoke;
        }

        private void Datepicker_mouseenter(object sender, MouseEventArgs e)
        {
            textBoxDate.BorderBrush = Brushes.Black;
        }
    }
}
