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
using System.Windows.Media.Animation;

namespace ConsumeBook.Pages.settings
{
    /// <summary>
    /// Логика взаимодействия для type.xaml
    /// </summary>
    public partial class type : Page
    {
        ApplicationContext db;

        public type()
        {
            db = new ApplicationContext();
            InitializeComponent();
        }

        public void CallPopup(string text)
        {
            popupMessage.Text = text;
            Storyboard s = (Storyboard)this.TryFindResource("ShowPopup");
            BeginStoryboard(s);
        }

        public void RefreshListBoxType()
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
            if (nameTypeTextBox.Text != "")
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

                CallPopup($"Тип {newType.TypeName}\nдобавлен");
                RefreshListBoxType();
                nameTypeTextBox.Text = "";
            }
            else
            {
                CallPopup("Не заполнено поле 'Тип'");
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

                    CallPopup($"Тип {typedel.TypeName}\nудален");
                }
                else
                {
                    CallPopup("Стандартный тип\nудалить нельзя");
                }
            }
            else
            {
                CallPopup("Не выбран тип");
            }
        }
    }
}
