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
using System.Windows.Navigation;
using System.Windows.Shapes;

using OpenCvSharp;

namespace CGCompress
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Mat img1 = Cv2.ImRead(@"resource\01.png");
            Mat img2 = Cv2.ImRead(@"resource\03.png");

            Mat diff = OpenCvTool.Subtract_Mold(img2, img1);
            Mat add = OpenCvTool.Add_Mold(img1, diff);

            Cv2.ImShow("image", diff);
            
            Cv2.WaitKey(0);
        }
    }
}
