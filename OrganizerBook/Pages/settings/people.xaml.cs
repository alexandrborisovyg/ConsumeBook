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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;

namespace OrganizerBook.Pages.settings
{
    /// <summary>
    /// Логика взаимодействия для people.xaml
    /// </summary>
    public partial class people : Page
    {
        ApplicationContext db;

        public people()
        {
            db = new ApplicationContext();
            InitializeComponent();
        }

        public void RefreshListBoxUser()
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
            if (nameUserTextBox.Text != "")
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
            if (listBoxUsers.SelectedIndex >= 0)
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
    }
}
