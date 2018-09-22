using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ConsumeBook
{
    public class User : INotifyPropertyChanged
    {
        public int userId { get; set; }
        private string name;
        private string surname;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Surname
        {
            get
            {
                return surname;
            }
            set
            {
                if (value == null)
                    surname = "";
                else
                    surname = value;

                OnPropertyChanged("Surname");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
