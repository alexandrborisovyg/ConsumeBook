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
using System.Globalization;

namespace OrganizerBook
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        ApplicationContext db = new ApplicationContext();

        public Settings()
        {
            InitializeComponent();

            RefreshListBoxUser();
            RefreshListBoxType();
            InititalizeListComboboxSubtype();
        }

        private void RefreshListBoxUser()
        {
            db.Users.Load();

            var queryUser = from u in db.Users.Local
                            where u.userId > 1
                            select new
                            {
                                userId = u.userId,
                                Name = u.Name,
                                Surname = u.Surname,
                                ShowName = u.Name + " " + u.Surname
                            };
            listBoxUsers.ItemsSource = queryUser.ToList();
        }

        private void AddUser(object sender, RoutedEventArgs e)
        {
            if(nameUserTextBox.Text != "")
            {
                User newUser = new User();
                newUser.Name = nameUserTextBox.Text;
                newUser.Surname = surnameUserTextBox.Text;
                db.Users.Add(newUser);
                db.SaveChanges();

                MessageBox.Show($"Пользователь {newUser.Name} {newUser.Surname} успешно добавлен в базу)");

                nameUserTextBox.Text = surnameUserTextBox.Text = "";

                RefreshListBoxUser();
            }
            else
            {
                MessageBox.Show("Не заполнено поле 'Имя'!");
            }
        }

        private void DeleteUser(object sender, RoutedEventArgs e)
        {
            if(listBoxUsers.SelectedIndex >= 0)
            {
                dynamic tempUser = listBoxUsers.SelectedItem;

                int userId = tempUser.userId;

                User user = db.Users
                    .Where(u => u.userId == userId)
                    .FirstOrDefault();

                db.Users.Load();
                db.Consumptions.Load();

                var queryConsumptions = from c in db.Consumptions.Local
                                        where c.UserId == userId
                                        select new
                                        {
                                            ConsumptionId = c.consumptionId
                                        };

                var ListChangeUserConsumptions = queryConsumptions.ToList();

                Consumption consumption;

                for (int i = 0; i < ListChangeUserConsumptions.Count; i++)
                {
                    consumption = db.Consumptions.Find(ListChangeUserConsumptions[i].ConsumptionId);

                    if (consumption != null)
                    {
                        consumption.UserId = 1;

                        db.Entry(consumption).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }

                db.Users.Remove(user);
                db.SaveChanges();

                MessageBox.Show($"Пользователь {user.Name} {user.Surname} удален из базы");

                RefreshListBoxUser();
            }
            else
            {
                MessageBox.Show("Не выбрано человек");
            }
        }


        private void RefreshListBoxType()
        {
            using (ApplicationContext db1 = new ApplicationContext())
            {
                db1.Users.Load();
                db1.Types.Load();
                db1.SubTypes.Load();

                var queryType = from t in db1.Types.Local
                                select new
                                {
                                    typeId = t.typeId,
                                    typeName = t.TypeName,
                                    typeAllowToDelete = t.TypeAllowToDelete
                                };


                typeList.ItemsSource = queryType.ToList();
            }
        }

        private void AddType(object sender, RoutedEventArgs e)
        {
            if(nameTypeTextBox.Text != "")
            {
                Type newType = new Type();
                newType.TypeName = nameTypeTextBox.Text;
                newType.TypeAllowToDelete = 1;
                db.Types.Add(newType);
                db.SaveChanges();

                SubType newSubType = new SubType();
                newSubType.TypeId = newType.typeId;
                newSubType.SubTypeAllowToDelete = 0;
                newSubType.SubTypeName = "Другое";
                db.SubTypes.Add(newSubType);
                db.SaveChanges();

                MessageBox.Show($"Тип {newType.TypeName} успешно добавлен в базу");
                RefreshListBoxType();
                nameTypeTextBox.Text = "";
            }
            else
            {
                MessageBox.Show("Не заполнено поле 'Тип'!");
            }
        }

        private void DelType(object sender, RoutedEventArgs e)
        {
            if (typeList.SelectedIndex >= 0)
            {
                dynamic tempType = typeList.SelectedItem;
                if (tempType.typeAllowToDelete != 0)
                {
                    db.SubTypes.Load();
                    db.Consumptions.Load();
                    db.Types.Load();

                    int typeId = tempType.typeId;

                    var queryTypeTemp = from st in db.Types.Local
                                           select new
                                           {
                                               typeId = st.typeId,
                                               typeName = st.TypeName
                                           };

                    var queryypeList = queryTypeTemp.ToList();

                    var querySubTypeTemp = from st in db.SubTypes.Local
                                           where st.TypeId == typeId
                                           select new
                                           {
                                               typeId = st.TypeId,
                                               subTypeId = st.subTypeId
                                           };

                    var querySubTypeList = querySubTypeTemp.ToList();

                    foreach (var i in querySubTypeList)
                    {

                        var queryCons = from con in db.Consumptions.Local
                                        where con.SubTypeId == i.subTypeId
                                        select new
                                        {
                                            consumptionId = con.consumptionId
                                        };

                        var queryConsumpList = queryCons.ToList();

                        foreach (var j in queryConsumpList)
                        {
                            Consumption cons = new Consumption();
                            cons = db.Consumptions.Find(j.consumptionId);
                            cons.SubTypeId = 1;

                            db.Entry(cons).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                        SubType stype = db.SubTypes
                        .Where(c => c.subTypeId == i.subTypeId)
                        .FirstOrDefault();

                        db.SubTypes.Remove(stype);
                        db.SaveChanges();
                    }

                    Type typedel = db.Types
                        .Where(c => c.typeId == typeId)
                        .FirstOrDefault();

                    db.Types.Remove(typedel);
                    db.SaveChanges();

                    RefreshListBoxType();
                }
                else
                {
                    MessageBox.Show("Стандартный тип удалить нельзя!");
                }
            }
        }


        private void InititalizeListComboboxSubtype()
        {
            db.Types.Load();

            var queryType = from t in db.Types.Local
                            select new
                            {
                                typeId = t.typeId,
                                typeName = t.TypeName,
                                typeAllowToDelete = t.TypeAllowToDelete
                            };

            var queryTypeList = queryType.ToList();
            for(int i = 0; i < queryTypeList.Count; i++)
            {
                comboboxTypeOfSubtype.Items.Add(queryTypeList[i]);
            }

            comboboxTypeOfSubtype.SelectedIndex = 0;
        }

        private void AddSubType(object sender, RoutedEventArgs e)
        {
            if (nameSubTypeTextBox.Text != "")
            {
                SubType newSubtype = new SubType();
                newSubtype.SubTypeName = nameSubTypeTextBox.Text;
                newSubtype.SubTypeAllowToDelete = 1;
                dynamic tempType = comboboxTypeOfSubtype.SelectedItem;
                newSubtype.TypeId = tempType.typeId;
                db.SubTypes.Add(newSubtype);
                db.SaveChanges();

                MessageBox.Show($"Подтип { newSubtype.SubTypeName} успешно добавлен в базу");
                changeTypeOfSubtype(null, null);
                nameSubTypeTextBox.Text = "";
            }
            else
            {
                MessageBox.Show("Не заполнено поле 'Подтип'!");
            }
        }

        private void DelSubType(object sender, RoutedEventArgs e)
        {
            if (subtypeList.SelectedIndex >= 0)
            {
                dynamic tempSubtype = subtypeList.SelectedItem;
                if(tempSubtype.subtypeAllowToDelete != 0)
                {
                    int subtypeId = tempSubtype.subtypeId;

                    SubType subtype = db.SubTypes
                        .Where(s => s.subTypeId == subtypeId)
                        .FirstOrDefault();

                    db.SubTypes.Load();
                    db.Consumptions.Load();

                    var queryConsumptions = from c in db.Consumptions.Local
                                            where c.SubTypeId == subtypeId
                                            select new
                                            {
                                                ConsumptionId = c.consumptionId
                                            };

                    var ListChangeSubtypeConsumptions = queryConsumptions.ToList();

                    Consumption consumption;

                    dynamic tempType = comboboxTypeOfSubtype.SelectedItem;
                    var querySubTypeOfType = from s in db.SubTypes.Local
                                             where s.TypeId == tempType.typeId
                                             select new
                                             {
                                                 subtypeId = s.subTypeId
                                             };
                    var listOfSubtypes = querySubTypeOfType.ToList();
                    int OtherId = listOfSubtypes[0].subtypeId;

                    for (int i = 0; i < ListChangeSubtypeConsumptions.Count; i++)
                    {
                        consumption = db.Consumptions.Find(ListChangeSubtypeConsumptions[i].ConsumptionId);

                        if (consumption != null)
                        {
                            consumption.SubTypeId = OtherId;

                            db.Entry(consumption).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }

                    db.SubTypes.Remove(subtype);
                    db.SaveChanges();

                    MessageBox.Show($"Подтип '{subtype.SubTypeName}' удален из базы");
                    changeTypeOfSubtype(null, null);
                }
                else
                {
                    MessageBox.Show("Не выбрано человек");
                }
            }
            else
            {
                MessageBox.Show("Стандартный подтип удалить нельзя!");
            } 
        }

        private void changeTypeOfSubtype(object sender, SelectionChangedEventArgs e)
        {
            db.SubTypes.Load();
            dynamic tempType = comboboxTypeOfSubtype.SelectedItem;
            var querySubType = from s in db.SubTypes.Local
                               where s.TypeId == tempType.typeId
                               select new
                               {
                                   subtypeId = s.subTypeId,
                                   subTypeName = s.SubTypeName,
                                   subtypeAllowToDelete = s.SubTypeAllowToDelete
                               };

            subtypeList.ItemsSource = querySubType.ToList();
        }
    }
}
