using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
            name_img = fullname_img.Substring(fullname_img.LastIndexOf("\\") + 1, fullname_img.LastIndexOf(".") - fullname_img.LastIndexOf("\\") - 1);
            father_img = -1;
            int cols = (img1.Cols);
            int rows = (img1.Rows);
            origin_size_img = rows * cols * img1.Channels();
            Pictures.Rows.Add(name_img, father_img, origin_size_img);

            string pathString = System.IO.Path.Combine(outpath);
            System.IO.Directory.CreateDirectory(pathString);
            img1.ImWrite(outpath + "\\0" + imgtype);

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
                        name_img = fullname_img.Substring(fullname_img.LastIndexOf("\\") + 1, fullname_img.LastIndexOf(".") - fullname_img.LastIndexOf("\\") - 1);
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

                            //Make a difference in reverse order (not need use Modulo operation)
                            Mat img2 = Cv2.ImRead(imglist[i - j].ToString());
                            Mat imgdiff1 = ImageTool.Subtract_Mold(img1,img2);
                            
                            double zerorate = (ImageTool.zerorate(imgdiff1));

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
                        }

                        //Result
                        Pictures.Rows.Add(name_img, father_img, origin_size_img);
                        if (father_img < 0)
                        {
                            img1.ImWrite(outpath + "\\" + i.ToString() + imgtype);
                        }
                        else
                        {
                            Mat img2 = Cv2.ImRead((String)imglist[father_img]);
                            //Make a difference by Modulo operation
                            Mat img1_diff = ImageTool.Subtract_Mold(img1, img2);
                            img1_diff.ImWrite(outpath + "\\" + i.ToString() + imgtype);
                        }
                        GC.Collect();
                    }

                };
            await Task.Run(doCompress);
            DataSet ds = new DataSet("Compress_Info");
            ds.Tables.Add(Pictures);
            ds.WriteXml(outpath + "\\compress_info.xml");
            progressDialog.Close();
            System.Windows.MessageBox.Show("完成压缩！");

            return;
        }

        public async static void Decompress(String infopath, String outpath, String outputImageType)
        {
            string pathString = System.IO.Path.Combine(outpath);
            System.IO.Directory.CreateDirectory(pathString);
            DataSet ds = new DataSet();
            ds.ReadXml(infopath + "\\compress_info.xml");
            DataTable Pictures = ds.Tables[0];
            String inputImageType;
            if (File.Exists(infopath + "//1.png")) inputImageType = ".png";
            else
            {
                if (File.Exists(infopath + "//1.webp")) inputImageType = ".webp";
                else
                {
                    if (File.Exists(infopath + "//1.jp2")) inputImageType = ".jp2";
                    else
                    {
                        if (File.Exists(infopath + "//1.tiff")) inputImageType = ".tiff";
                        else return;
                    }
                }
            }

            ProgressDialog progressDialog = new ProgressDialog(Pictures.Rows.Count);
            progressDialog.Show();

            Action doCompress =
                () =>
                {
                    for (int i = 0; i < Pictures.Rows.Count; i++)
                    {
                        progressDialog.data.CurrentProgress = i;
                        Mat img1 = Cv2.ImRead(infopath +"\\"+i.ToString() + inputImageType);
                        if (Convert.ToInt32((String)Pictures.Rows[i]["Father"])< 0)
                        {
                            Cv2.ImWrite(outpath + "\\" + (string)Pictures.Rows[i]["Name"] + inputImageType, img1);
                        }
                        else
                        {
                            Mat img2 = Cv2.ImRead(infopath + "\\"+(string)Pictures.Rows[i]["Father"] + inputImageType);
                            Cv2.ImWrite(outpath + "\\" + (string)Pictures.Rows[i]["Name"] + inputImageType, ImageTool.Add_Mold(img2, img1));
                        }
                        img1.Release();
                        GC.Collect();
                    }
                };
            await Task.Run(doCompress);

            progressDialog.Close();
            System.Windows.MessageBox.Show("完成解压缩！");
        }
    }
}
