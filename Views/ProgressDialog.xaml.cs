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
using System.Threading;

namespace CGCompress
{
    /// <summary>
    /// ProgressDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ProgressDialog : Window
    {
        public ViewModels.ProgressDialogViewModel data= new ViewModels.ProgressDialogViewModel();
        public ProgressDialog(int the_max)
        {
            this.ResizeMode = ResizeMode.CanMinimize;
            InitializeComponent();
            data.Max = the_max;
            this.DataContext = data;
        }
    }
}
