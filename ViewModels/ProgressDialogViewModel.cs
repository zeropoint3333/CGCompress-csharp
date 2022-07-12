using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGCompress.ViewModels
{
    public class ProgressDialogViewModel : INotifyPropertyChanged
    {
        private int max = 98;
        public int Max
        {
            get { return max; }
            set { max = value; }
        }

        private int current_Progress = 0;
        public int CurrentProgress
        {
            get { return current_Progress; }
            set {
                current_Progress = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TextString"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentProgress"));
        }

        private string textString = "";

        

        public string TextString
        {
            get { return current_Progress.ToString()+"/"+max.ToString(); }
        }
    }
}
