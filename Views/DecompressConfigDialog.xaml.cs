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

namespace CGCompress.Views
{
    /// <summary>
    /// DecompressConfigDialog.xaml 的交互逻辑
    /// </summary>
    public partial class DecompressConfigDialog : Window
    {
        public DecompressConfigDialog(string inputpath)
        {
            InitializeComponent();
            this.DataContext = new ViewModels.DecompressConfigDialogViewModel();
            outpath.Text = inputpath + "\\CGDecompress";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog openFolderDialog = new Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog();
            openFolderDialog.IsFolderPicker = true;
            if (openFolderDialog.ShowDialog() == Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok)
            {
                outpath.Text = openFolderDialog.FileName;
            }
            this.Activate();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
