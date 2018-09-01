using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OrganizerBook
{
    class BindingSureWindow : INotifyPropertyChanged, IDataErrorInfo
    {
        private string answer { get; set; }

        public string Answer
        {
            get
            {
                return answer;
            }
            set
            {
                answer = value;
                OnPropertyChanged("Answer");
            }
        }

        public string this[string answerValidate]
        {
            get
            {
                string error = String.Empty;
                switch (answerValidate)
                {
                    case "answer":
                        if ((Answer != "Да"))
                        {
                            error = "Неверная строка";
                        }
                        break;
                }
                return error;
            }
        }
        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
