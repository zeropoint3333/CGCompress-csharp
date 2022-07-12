using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace CGCompress
{
    internal class ImagePack
    {
        public async static void Compress(ArrayList imglist, int cycletimes, String outpath, String imgtype)
        {

            //Initialization DataTable
            DataTable Pictures = new DataTable();
            DataColumn name = new DataColumn("Name", typeof(String));
            DataColumn father = new DataColumn("Father", typeof(Int32));
            DataColumn origin_size = new DataColumn("origin_Size", typeof(Int32));
            Pictures.Columns.Add(name);
            Pictures.Columns.Add(father);
            Pictures.Columns.Add(origin_size);

            //Initialization every picture info
            String fullname_img;
            String name_img;
            Mat img1;
            int father_img;
            int origin_size_img;

            //Set first picture info
            fullname_img = imglist[0].ToString();
            img1 = Cv2.ImRead(fullname_img);
            name_img = fullname_img.Substring(fullname_img.LastIndexOf("/") + 1, fullname_img.LastIndexOf(".") - fullname_img.LastIndexOf("/") - 1);
            father_img = -1;
            int cols = (img1.Cols);
            int rows = (img1.Rows);
            origin_size_img = rows * cols * img1.Channels();
            Pictures.Rows.Add(name_img, father_img, origin_size_img);

            string pathString = System.IO.Path.Combine(outpath + "\\CGCompress");
            System.IO.Directory.CreateDirectory(pathString);
            img1.ImWrite(outpath + "\\CGCompress\\0" + imgtype);
            
            img1.Release();
            double similarate;

            ProgressDialog progressDialog = new ProgressDialog(imglist.Count);
            progressDialog.Show();

            Action doCompress =
                () =>
                {
                    for (int i = 1; i < imglist.Count; i++)
                    {
                        progressDialog.data.CurrentProgress = i;
                        //Set default value
                        fullname_img = imglist[i].ToString();
                        name_img = fullname_img.Substring(fullname_img.LastIndexOf("/") + 1, fullname_img.LastIndexOf(".") - fullname_img.LastIndexOf("/") - 1);
                        father_img = -1;
                        img1 = Cv2.ImRead(fullname_img);
                        cols = (img1.Cols);
                        rows = (img1.Rows);
                        origin_size_img = rows * cols * img1.Channels();
                        similarate = 0;
                        int k = 1;

                        //Find main picture
                        for (int j = 1; j < i + 1; j++)
                        {
                            //Filter correct main pictures
                            if ((Int32)Pictures.Rows[i - j][1] >= 0 || (Int32)Pictures.Rows[i - j][2] != origin_size_img)
                                continue;

                            //Make a difference in reverse order
                            Mat img2 = Cv2.ImRead(imglist[i - j].ToString());
                            Mat imgdiff = ImageTool.Subtract_Mold(img1, img2);
                            double zerorate = ImageTool.zerorate(imgdiff);

                            if (zerorate > similarate && zerorate > 0.8)
                            {
                                similarate = zerorate;
                                father_img = i - j;
                                break;
                            }
                            else
                            {
                                if (zerorate > similarate && zerorate > 0.5)
                                {
                                    similarate = zerorate;
                                    father_img = i - j;
                                }
                                if (k > cycletimes) break;
                            }
                            k++;
                            img2.Release();
                            imgdiff.Release();
                        }

                        //Result
                        Pictures.Rows.Add(name_img, father_img, origin_size_img);
                        if ((Int32)Pictures.Rows[i][1] < 0)
                        {
                            img1.ImWrite(outpath+"\\CGCompress\\" + i.ToString() + imgtype);
                        }
                        else
                        {
                            Mat img2 = Cv2.ImRead((String)imglist[(Int32)Pictures.Rows[i][1]]);
                            Mat img1_diff = ImageTool.Subtract_Mold(img1, img2);
                            img1_diff.ImWrite(outpath + "\\CGCompress\\" + i.ToString() + imgtype);
                        }
                        img1.Release();
                        GC.Collect();
                        
                    }

                };
            await Task.Run(doCompress);
            DataSet ds = new DataSet("Compress_Info");
            ds.Tables.Add(Pictures);
            ds.WriteXml(outpath + "\\CGCompress\\" + "compress_info.xml");
            progressDialog.Close();
            System.Windows.MessageBox.Show("完成压缩！");

            return;
        }
    }
}
