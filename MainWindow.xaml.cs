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
            Mat source1 = new Mat(@"resource\01.png", ImreadModes.Color);
            Mat source2 = new Mat(@"resource\02.png", ImreadModes.Color);
            Mat diff = new Mat();
            Cv2.Absdiff(source1, source2, diff);
            Cv2.ImShow("Demo", diff);
            Cv2.WaitKey(0);
        }
    }
}
