using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using OpenCvSharp;

namespace CGCompress.ViewModels
{
    public class ImageExplorerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        public String imgFolder;
        public String imgName;
        public Mat img;
        
        private double borderWidth;
        public double BorderWidth
        {
            get
            {
                return borderWidth;
            }
            set
            {
                borderWidth = value;
            }
        }

        private double borderHeight;
        public double BorderHeight
        {
            get {
                return borderHeight; 
            }
            set
            {
                borderHeight = value;
            }
        }
        public double imgWidth
        {
            get
            {
                if (borderWidth / img.Width > borderHeight / img.Height)
                {
                    return img.Width * borderHeight / img.Height;
                }
                else
                {
                    return borderWidth;
                }
            }
        }
        public double imgHeight
        {
            get
            {
                if (borderWidth / img.Width > borderHeight / img.Height)
                {
                    return borderHeight;
                }
                else
                {
                    return img.Height * borderWidth / img.Width;
                }
            }
        }

        public BitmapImage ImageSource
        {
            get
            {
                String imgtype = imgName.Substring(imgName.IndexOf(".")+1,imgName.Length- imgName.IndexOf(".")-1);
                if (File.Exists(imgFolder + "\\compress_info.xml"))
                {
                    System.Data.DataSet ds = new System.Data.DataSet();
                    ds.ReadXml(imgFolder + "\\compress_info.xml");
                    System.Data.DataTable Pictures = ds.Tables[0];
                    int index = Convert.ToInt32(imgName.Substring(0, imgName.IndexOf(".")));
                    Mat img1 = Cv2.ImRead(imgFolder + "\\" + imgName);
                    if (Convert.ToInt32((String)Pictures.Rows[index]["Father"]) < 0)
                    {
                        img = img1;
                    }
                    else
                    {
                        Mat img2 = Cv2.ImRead(imgFolder + "\\" + (string)Pictures.Rows[index]["Father"] + "."+imgtype);
                        img = ImageTool.Add_Mold(img2, img1);
                    }
                }
                else
                {
                    img = Cv2.ImRead(imgFolder + "\\" + imgName);
                }
                

                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                List<byte> lstbyte = new List<byte>();
                byte[] btArr = lstbyte.ToArray();
                int[] param = new int[2] { 1, 80 };
                Cv2.ImEncode(".jpg", img, out btArr, param);
                bmp.StreamSource = new System.IO.MemoryStream(btArr);
                bmp.EndInit();
                return bmp;
            }
        }

        public List<String> images = new List<String>();
        public List<String> Images
        {
            get
            {
                foreach (FileInfo file in new DirectoryInfo(imgFolder).GetFiles())
                {
                    if (new System.Text.RegularExpressions.Regex(@"([^\s]+(\.(?i)(jpg|jpeg|png|bmp|webp|jp2|tiff))$)").IsMatch(file.Name))
                        images.Add(file.FullName);
                }
                return images;
            }
        }
    }
}
