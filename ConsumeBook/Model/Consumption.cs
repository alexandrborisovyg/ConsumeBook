using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ConsumeBook
{
    public class Consumption : INotifyPropertyChanged, ICloneable
    {
        private string description;
        private int subTypeId;
        private int value;
        private DateTime date;
        private int userId;

        public int consumptionId { get; set; }

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }
        public int SubTypeId
        {
            get { return subTypeId; }
            set
            {
                subTypeId = value;
                OnPropertyChanged("SubTypeId");
            }
        }
        public int Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnPropertyChanged("Value");
            }
        }
        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }
        public int UserId
        {
            get { return userId; }
            set
            {
                userId = value;
                OnPropertyChanged("UserId");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public object Clone()
        {
            //return this.MemberwiseClone();
            DateTime bufTime = new DateTime(this.date.Year, this.date.Month, this.date.Day);
            return new Consumption { description = this.description, subTypeId = this.subTypeId, value = this.value, consumptionId = this.consumptionId, date = bufTime, userId = this.userId };
        }
    }
}
