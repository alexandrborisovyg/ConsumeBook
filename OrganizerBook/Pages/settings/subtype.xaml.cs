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
    /// Логика взаимодействия для subtype.xaml
    /// </summary>
    public partial class subtype : Page
    {
        ApplicationContext db;

        public subtype()
        {
            db = new ApplicationContext();
            InitializeComponent();
        }

        public void InititalizeListComboboxSubtype()
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
            for (int i = 0; i < queryTypeList.Count; i++)
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
                if (tempSubtype.subtypeAllowToDelete != 0)
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
            if(comboboxTypeOfSubtype.SelectedItem != null)
            {
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
}
