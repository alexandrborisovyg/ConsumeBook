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

namespace OrganizerBook.Pages.settings
{
    /// <summary>
    /// Логика взаимодействия для general.xaml
    /// </summary>
    public partial class general : Page
    {
        ApplicationContext db;

        public general()
        {
            InitializeComponent();

            db = new ApplicationContext();
        }

        public void CallPopup(string text)
        {
            popupMessage.Text = text;
            Storyboard s = (Storyboard)this.TryFindResource("ShowPopup");
            BeginStoryboard(s);
        }

        private void ButtonConsumptionsClear_Click(object sender, RoutedEventArgs e)
        {
            var itemsToDelete = db.Set<Consumption>();
            db.Consumptions.RemoveRange(itemsToDelete);
            db.SaveChanges();

            CallPopup("Все траты удалены");
        }
    }
}
