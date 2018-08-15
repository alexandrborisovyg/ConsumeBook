using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OrganizerBook
{
    public class BindingMainWindow : INotifyPropertyChanged
    {
        private int sum { get; set; }
        private string keyWord { get; set; }
        private int fromValue { get; set; }
        private int toValue { get; set; }
        private DateTime fromPeriod { get; set; }
        private DateTime toPeriod { get; set; }

        public int Sum
        {
            get{
                return sum;
            }
            set
            {
                sum = value;
                OnPropertyChanged("Sum");
            }
        }

        public string KeyWord
        {
            get{
                return keyWord;
            }
            set
            {
                keyWord = value;
                OnPropertyChanged("KeyWord");
            }
        }
        public int FromValue
        {
            get{
                return fromValue;
            }
            set
            {
                fromValue = value;
                OnPropertyChanged("FromValue");
            }
        }
        public int ToValue
        {
            get{
                return toValue;
            }
            set
            {
                toValue = value;
                OnPropertyChanged("ToValue");
            }
        }
        public DateTime FromPeriod
        {
            get{
                return fromPeriod;
            }
            set
            {
                fromPeriod = value;
                OnPropertyChanged("FromPeriod");
            }
        }
        public DateTime ToPeriod
        {
            get
            {
                return toPeriod;
            }
            set
            {
                toPeriod = value;
                OnPropertyChanged("ToPeriod");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
