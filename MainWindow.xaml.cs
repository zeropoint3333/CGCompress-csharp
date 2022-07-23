using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenCvSharp;

namespace CGCompress
{
    public partial class MainWindow : System.Windows.Window
    {
        private ViewModels.MainWindowViewModel mainWindowViewModel = new ViewModels.MainWindowViewModel();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = mainWindowViewModel;
            mainWindowViewModel.Path = System.IO.Directory.GetCurrentDirectory();

            Style rowStyle = new Style(typeof(DataGridRow));
            rowStyle.Setters.Add(new EventSetter(DataGridRow.MouseDoubleClickEvent, new MouseButtonEventHandler(FileExplorerRow_DoubleClick)));
            FileExplorer.RowStyle = rowStyle;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Mat src = Cv2.ImRead(@"D:\01.png");
            /*
            Mat[] img1 = Cv2.Split(src);
            Cv2.Merge(new Mat[] { img1[0], img1[1], img1[2] }, src);
            Cv2.ImShow("1", src);
            */
            //src.ConvertTo(src, MatType.CV_32F,1.0/255);
            //Cv2.Dct(src, src);
            //Cv2.Idct(src, src);
           
        }

        private void OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog openFolderDialog = new Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog();
            openFolderDialog.IsFolderPicker = true;
            if (openFolderDialog.ShowDialog() == Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok)
            {
                mainWindowViewModel.Path = openFolderDialog.FileName;
            }
        }

        private void Compress_Click(object sender, RoutedEventArgs e)
        {
            DirectoryInfo folder = new DirectoryInfo(mainWindowViewModel.Path);
            ArrayList imgpaths = new ArrayList();

            //读取所有图片格式的文件，包括jpg png jpeg等
            foreach (FileInfo file in folder.GetFiles())
            {
                if(new System.Text.RegularExpressions.Regex(@"([^\s]+(\.(?i)(jpg|jpeg|png|bmp|webp|jp2|tiff))$)").IsMatch(file.Name))
                imgpaths.Add(file.FullName);
            }

            if (imgpaths.Count == 0) return;

            Views.CompressConfigDialog compressorConfig = new Views.CompressConfigDialog(mainWindowViewModel.Path);
            if (compressorConfig.ShowDialog() == true)
            {
                ImagePack.Compress(imgpaths, Convert.ToInt32(compressorConfig.subtracttimes.Text), compressorConfig.outpath.Text,"."+compressorConfig.Format.Content);
            }
            GC.Collect();
            mainWindowViewModel.Path = mainWindowViewModel.Path;
        }

        private void Extract_Click(object sender, RoutedEventArgs e)
        {
            if(!File.Exists(mainWindowViewModel.Path + "\\compress_info.xml"))
            {
                MessageBox.Show("该目录下无压缩信息");
            }
            else
            {
                Views.DecompressConfigDialog decompressorConfig = new Views.DecompressConfigDialog(mainWindowViewModel.Path);
                if (decompressorConfig.ShowDialog() == true)
                {
                    ImagePack.Decompress(mainWindowViewModel.Path, decompressorConfig.outpath.Text,"."+decompressorConfig.Format.Content);
                }
                GC.Collect();
                mainWindowViewModel.Path = mainWindowViewModel.Path;
            }
        }

        private void SeeImage_Click(object sender, RoutedEventArgs e)
        {
            //Determine if an item is selected
            if (FileExplorer.SelectedCells.Count == 0) return;

            String imgname = ((FileSystemInfo)this.FileExplorer.SelectedItem).Name;
            String imgtype = ((FileSystemInfo)this.FileExplorer.SelectedItem).Extension.ToString();
            if (!new System.Text.RegularExpressions.Regex(@"(\.(?i)(jpg|jpeg|png|bmp|webp|jp2|tiff)$)").IsMatch(imgtype)) return;

            Views.ImageExplorer test = new Views.ImageExplorer(imgname, mainWindowViewModel.Path);
            test.Show();
        }

        private void FileExplorerRow_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow? row = sender as DataGridRow;
            if (Directory.Exists(((FileSystemInfo)row.Item).FullName))
            {
                mainWindowViewModel.Path = ((FileSystemInfo)row.Item).FullName;
                return;
            }
            String imgname = ((FileSystemInfo)row.Item).Name;
            String imgtype = ((FileSystemInfo)row.Item).Extension.ToString();
            if (!new System.Text.RegularExpressions.Regex(@"(\.(?i)(jpg|jpeg|png|bmp|webp|jp2|tiff)$)").IsMatch(imgtype)) return;

            Views.ImageExplorer test = new Views.ImageExplorer(imgname, mainWindowViewModel.Path);
            test.Show();
        }
        private void MoveUp_Click(object sender, RoutedEventArgs e)
        {
            if (mainWindowViewModel.Path.LastIndexOf("\\") < 0) return;
            mainWindowViewModel.Path = mainWindowViewModel.Path.Substring(0, mainWindowViewModel.Path.LastIndexOf("\\"));
        }
        private void Lock_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void Unlock_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
