using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCvSharp;


namespace CGCompress
{
    internal class OpenCvTool
    {
        //OpenCvSharp自带的Add方法为饱和运算，这里改为需要的模运算
        public static Mat Add_Mold(Mat img1, Mat img2)
        {
            int H = img1.Height;
            int W = img1.Width;
            int C = img1.Channels();
            Mat sum = new Mat(H, W, img1.Type());
            unsafe
            {
                byte* bit1 = (byte*)img1.Data;
                byte* bit2 = (byte*)img2.Data;
                byte* bitadd = (byte*)sum.Data;
                for (int k = 0; k < C; k++)
                {

                    for (int i = 0; i < H; ++i)
                    {
                        for (int j = 0; j < W; ++j)
                        {
                            int index = i * W * C + j * C + k;
                            bitadd[index] = (byte)((bit1[index] + bit2[index]) % 256);
                        }
                    }
                }
            }
            return sum;
        }

        public static Mat Subtract_Mold(Mat img1, Mat img2)
        {
            //OpenCvSharp自带的Subtract方法为饱和运算，这里改为需要的模运算
            int H = img1.Rows;
            int W = img1.Cols;
            int C = img1.Channels();
            Mat diff = new Mat(H, W, img1.Type());
            unsafe
            {
                byte* bit1 = (byte*)img1.Data;
                byte* bit2 = (byte*)img2.Data;
                byte* bitdiff = (byte*)diff.Data;
                for (int k = 0; k < C; k++)
                {
                    for (int i = 0; i < H; ++i)
                    {
                        for (int j = 0; j < W; ++j)
                        {
                            int index = (i * W * C + j * C + k);
                            bitdiff[index] = (byte)((bit1[index] - bit2[index]) % 256);
                        }
                    }
                }
            }

            return diff;
        }
    }
}
