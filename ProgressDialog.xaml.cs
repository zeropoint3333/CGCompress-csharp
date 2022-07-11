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
using System.ComponentModel;

namespace CGCompress
{
    /// <summary>
    /// ProgressDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ProgressDialog : Window, INotifyPropertyChanged
    {


#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        public ProgressDialog(int total)
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        {
            this.ResizeMode = ResizeMode.CanMinimize;
            InitializeComponent();
            progressbar.Maximum = total;
            progresstext.Text = (@"0/") + total.ToString();
            test();

        }


        private int currentProgress = 0;
        public int CurrentProgress
        {
            get => currentProgress;
            set
            {
                currentProgress = value;
                OnPropertyChanged("CurrentProgress");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        public async void test()
        {
            Action<int> bindProgress =
                value => progressbar.Value = value;
            //新建一个Progress对象，把Action设定为刚才的action，这里需要注意的是
            //我们为了简化，把这个progress对象cast成了它的Interface，这是因为只有
            //IProgress才有 .report方法，而我们在这里需要用到这个方法。
            //
            //这个也可以新建为Progress或者索性var，然后再之后的引用里面写为
            //  ((IProgress<int>)progress).Report(i)
            //反正早晚都是需要cast之后才能使用
            IProgress<int> progress = new Progress<int>(bindProgress);

            //好了，现在这个从Progress报告到赋值给进度条的绑定就做好了，
            //和我们的例2一样，建立Action，然后异步跑它
            Action growProgress =
                () =>
                {
                    for (int i = 0; i <= 100; i++)
                    {
                        System.Threading.Thread.Sleep(34);
                        progress.Report(i);
                    }
                };
            await Task.Run(growProgress);
        }
    }
}
