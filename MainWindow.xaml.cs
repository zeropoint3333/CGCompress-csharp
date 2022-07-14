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
        private static class common
        {
            static String path = (@"E:\test");

            public static string Path
            {
                get { return path; }
                set { path = value; }
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            DirectoryInfo folder = new DirectoryInfo(common.Path);
            this.Path_TextBox.Text = common.Path;
            FileSystemInfo[] fileinfo = folder.GetFileSystemInfos();
            FileExplorer.ItemsSource = fileinfo;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Mat img1 = Cv2.ImRead(@"resource\01.png");
            Mat img2 = Cv2.ImRead(@"resource\03.png");

            Mat diff = ImageTool.Subtract_Mold(img2, img1);
            Mat add = ImageTool.Add_Mold(diff, diff);
            //MessageBox.Show(ImageTool.zerorate(img1).ToString()+"+"+ ImageTool.zerorate(diff).ToString());
            //Cv2.ImShow("image", add);1, add);
            //png jp2 webp压缩率依次升高

            //Cv2.WaitKey(0);
        }

        private void OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog openFolderDialog = new Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog();
            openFolderDialog.IsFolderPicker = true;
            if (openFolderDialog.ShowDialog() == Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok)
            {
                common.Path = openFolderDialog.FileName;
                DirectoryInfo folder = new DirectoryInfo(common.Path);
                this.Path_TextBox.Text = common.Path;
                FileSystemInfo[] subdir = folder.GetFileSystemInfos();
                FileExplorer.ItemsSource = subdir;
            }
        }

        private void Compress_Click(object sender, RoutedEventArgs e)
        {
            DirectoryInfo folder = new DirectoryInfo(common.Path);
            ArrayList imgpaths = new ArrayList();

            //读取所有图片格式的文件，包括jpg png jpeg等
            foreach (FileInfo file in folder.GetFiles())
            {
                if(new System.Text.RegularExpressions.Regex(@"([^\s]+(\.(?i)(jpg|jpeg|png|bmp|webp|jp2|tiff))$)").IsMatch(file.Name))
                imgpaths.Add(file.FullName);
            }

            Views.CompressConfigDialog compressorConfig = new Views.CompressConfigDialog(this.Path_TextBox.Text);
            if (compressorConfig.ShowDialog() == true)
            {
                ImagePack.Compress(imgpaths, Convert.ToInt32(compressorConfig.subtracttimes.Text), compressorConfig.outpath.Text,"."+compressorConfig.Format.Content);
            }
            GC.Collect();
        }

        private void Extract_Click(object sender, RoutedEventArgs e)
        {
            if(!File.Exists(this.Path_TextBox.Text + "\\compress_info.xml"))
            {
                MessageBox.Show("该目录下无压缩信息");
            }
            else
            {
                Views.DecompressConfigDialog decompressorConfig = new Views.DecompressConfigDialog(this.Path_TextBox.Text);
                if (decompressorConfig.ShowDialog() == true)
                {
                    ImagePack.Decompress(this.Path_TextBox.Text,decompressorConfig.outpath.Text,"."+decompressorConfig.Format.Content);
                }
                GC.Collect();
            }
        }

        private void SeeImage_Click(object sender, RoutedEventArgs e)
        {
            if (FileExplorer.SelectedCells.Count == 0) return;
            String imgname = (FileExplorer.Columns[0].GetCellContent(FileExplorer.Items[this.FileExplorer.SelectedIndex]) as TextBlock).Text;
            if (File.Exists(this.Path_TextBox.Text + "\\compress_info.xml"))
            {
                System.Data.DataSet ds = new System.Data.DataSet();
                ds.ReadXml(this.Path_TextBox.Text + "\\compress_info.xml");
                System.Data.DataTable Pictures = ds.Tables[0];
                int index = Convert.ToInt32(imgname.Substring(0,imgname.IndexOf(".")));
                string imgtype = imgname.Substring(imgname.IndexOf("."),imgname.Length-imgname.IndexOf("."));
                Mat img1 = Cv2.ImRead(this.Path_TextBox.Text + "\\" + index.ToString() + imgtype);
                if (Convert.ToInt32((String)Pictures.Rows[index]["Father"]) < 0)
                {
                    Cv2.ImShow("image" , img1);
                }
                else
                {
                    Mat img2 = Cv2.ImRead(this.Path_TextBox.Text + "\\" + (string)Pictures.Rows[index]["Father"] + imgtype);
                    Cv2.ImShow("image", ImageTool.Add_Mold(img2, img1));
                }
                img1.Release();
            }
            else
            {
                Cv2.ImShow("image", Cv2.ImRead(this.Path_TextBox.Text+"\\"+imgname));
            }
        }
    }
}
