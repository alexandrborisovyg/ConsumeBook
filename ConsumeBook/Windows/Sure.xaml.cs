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

namespace ConsumeBook
{
    /// <summary>
    /// Логика взаимодействия для Sure.xaml
    /// </summary>
    public partial class Sure : Window
    {
        public Sure()
        {
            InitializeComponent();

            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            double screenWidth = SystemParameters.FullPrimaryScreenWidth;

            this.Top = (screenHeight - this.Height) / 0x00000002;
            this.Left = (screenWidth - this.Width) / 0x00000002;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            BindingSureWindow sureElements = (BindingSureWindow)this.Resources["sureElements"];
            sureElements.Answer = answerTextBox.Text;

            if (sureElements.Answer == "Да")
            {
                this.DialogResult = true;
                Close();
            }
            else
            {
                answerTextBox.BorderBrush = Brushes.Red;
            }
        }
    }
}
