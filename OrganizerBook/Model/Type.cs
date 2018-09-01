using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OrganizerBook
{
    public class Type : INotifyPropertyChanged
    {
        public int typeId { get; set; }
        private string typeName;
        private int typeAllowToDelete;

        public string TypeName
        {
            get { return typeName; }
            set
            {
                typeName = value;
                OnPropertyChanged("TypeName");
            }
        }
        public int TypeAllowToDelete
        {
            get { return typeAllowToDelete; }
            set
            {
                typeAllowToDelete = value;
                OnPropertyChanged("TypeAllowToDelete");
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
