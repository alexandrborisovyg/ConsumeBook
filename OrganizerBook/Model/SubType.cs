using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OrganizerBook
{
    public class SubType : INotifyPropertyChanged
    {
        public int subTypeId {get; set;}
        private string subTypeName;
        private int subTypeAllowToDelete;
        private int typeId;

        public string SubTypeName
        {
            get { return subTypeName; }
            set
            {
                subTypeName = value;
                OnPropertyChanged("SubTypeName");
            }
        }
        public int SubTypeAllowToDelete
        {
            get { return subTypeAllowToDelete; }
            set
            {
                subTypeAllowToDelete = value;
                OnPropertyChanged("SubTypeAllowToDelete");
            }
        }
        public int TypeId
        {
            get { return typeId; }
            set
            {
                typeId = value;
                OnPropertyChanged("TypeId");
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
