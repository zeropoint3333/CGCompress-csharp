using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGCompress.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string path= "";
        public  string Path
        {
            get { return path; }
            set { 
                path = value;
                OnPropertyChanged("Path");
                OnPropertyChanged("FileInfos");
            }
        }

        public FileSystemInfo[] FileInfos
        {
            get {
                DirectoryInfo folder = new DirectoryInfo(Path);
                FileSystemInfo[] subdir = folder.GetFileSystemInfos();
                return subdir;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
