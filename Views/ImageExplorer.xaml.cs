using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for ImageExplorer.xaml
    /// </summary>
    public partial class ImageExplorer : System.Windows.Window
    {
        public ViewModels.ImageExplorerViewModel data = new ViewModels.ImageExplorerViewModel();
        public ImageExplorer(String imgname,String imgFolder)
        {
            data.imgName = imgname;
            data.imgFolder = imgFolder;
            data.img= Cv2.ImRead(imgFolder + "\\" + imgname);
            InitializeComponent();
            data.BorderHeight = imglist.Height;
            data.BorderWidth = imglist.Width*3/10;
            this.DataContext = data;
            LoadImageList(imgFolder);
        }

        private async void LoadImageList(String path)
        {
            if(File.Exists(path + "\\compress_info.xml"))
            {

            }
            else
            {

            }
        }
    }
}
