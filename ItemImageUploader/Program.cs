using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Configuration;
using PowerPOS;
using SubSonic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ItemImageUploader
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("ITEM IMAGE UPLOADER");
                string itemImageFolder = (Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) + "\\ItemImage\\").Replace("file:\\", "");
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ItemImageFolder"]))
                {
                    itemImageFolder = ConfigurationManager.AppSettings["ItemImageFolder"];
                    if (!Directory.Exists(itemImageFolder))
                        itemImageFolder = (Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) + "\\ItemImage\\").Replace("file:\\", "");
                }

                Console.WriteLine("--> Reading image from directory : " + itemImageFolder);

                if (Directory.Exists(itemImageFolder))
                {
                    Console.WriteLine("Directory found");
                    UploadImage(Directory.GetFiles(itemImageFolder, "*.jpg"));
                    UploadImage(Directory.GetFiles(itemImageFolder, "*.jpeg"));
                    UploadImage(Directory.GetFiles(itemImageFolder, "*.png"));
                    //UploadImage(Directory.GetFiles(itemImageFolder, "*.gif"));
                }
                Console.WriteLine("Upload finish");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                Console.WriteLine("Error Occured");
                Console.WriteLine(ex);
            }
        }

        static void UploadImage(string[] imageJPG)
        {
            Console.WriteLine("Uploading "+imageJPG.Length+" file(s)");
            for (int i = 0; i < imageJPG.Length; i++)
            {
                try
                {
                    if (File.Exists(imageJPG[i]))
                    {
                        Console.Write("-> Upload image for item " + imageJPG[i]);
                        string itemNo = Path.GetFileNameWithoutExtension(imageJPG[i]);
                        var theItem = new Item(Item.Columns.ItemNo, itemNo);
                        if (!theItem.IsNew)
                        {
                            theItem.ItemImage = ResizeAndCompressImage(imageJPG[i]);
                            theItem.Save("Item Image Uploader");
                            File.Delete(imageJPG[i]);
                            Console.WriteLine(" SUCCESS");
                        }
                        else
                        {
                            Console.WriteLine(" FAILED : ITEM NOT FOUND");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" FAILED : ERROR OCCURED");
                    Logger.writeLog(ex);
                    Console.WriteLine("Error occured "+ex.Message);
                }
            } 
        }

        static byte[] ResizeAndCompressImage(string path)
        {
            Bitmap oriFile = new Bitmap(path);
            Size newSize = oriFile.Size;
            bool needResize = false;
            if (oriFile.Width > oriFile.Height)
            {
                if (oriFile.Width > 800)
                {
                    newSize.Width = 800;
                    newSize.Height = Convert.ToInt32(Convert.ToDecimal((Convert.ToDecimal(800) / Convert.ToDecimal(oriFile.Width)))
                        * oriFile.Height);
                    needResize = true;
                }
            }
            else
            {
                if (oriFile.Height > 800)
                {
                    newSize.Height = 800;
                    newSize.Width = Convert.ToInt32(Convert.ToDecimal((Convert.ToDecimal(800) / Convert.ToDecimal(oriFile.Height)))
                        * oriFile.Width);
                    needResize = true;
                }
            }

            if (needResize)
                oriFile = ResizeImage(oriFile, newSize);
            byte[] newData = CompressImage(oriFile);
            oriFile.Dispose();
            //string newPath = Path.GetFullPath(path)+Path.GetFileNameWithoutExtension(path)+"___NEW.jpeg";
            //File.WriteAllBytes(newPath, newData);
            return newData;
        }

        static Bitmap ResizeImage(Bitmap mg, Size newSize)
        {
            double ratio = 0d;
            double myThumbWidth = 0d;
            double myThumbHeight = 0d;
            int x = 0;
            int y = 0;

            Bitmap bp;

            if ((mg.Width / Convert.ToDouble(newSize.Width)) > (mg.Height /
            Convert.ToDouble(newSize.Height)))
                ratio = Convert.ToDouble(mg.Width) / Convert.ToDouble(newSize.Width);
            else
                ratio = Convert.ToDouble(mg.Height) / Convert.ToDouble(newSize.Height);
            myThumbHeight = Math.Ceiling(mg.Height / ratio);
            myThumbWidth = Math.Ceiling(mg.Width / ratio);

            //Size thumbSize = new Size((int)myThumbWidth, (int)myThumbHeight);
            Size thumbSize = new Size((int)newSize.Width, (int)newSize.Height);
            bp = new Bitmap(newSize.Width, newSize.Height);
            x = (newSize.Width - thumbSize.Width) / 2;
            y = (newSize.Height - thumbSize.Height);
            // Had to add System.Drawing class in front of Graphics ---
            System.Drawing.Graphics g = Graphics.FromImage(bp);
            g.SmoothingMode = SmoothingMode.HighSpeed;
            g.InterpolationMode = InterpolationMode.Default;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            Rectangle rect = new Rectangle(x, y, thumbSize.Width, thumbSize.Height);
            g.DrawImage(mg, rect, 0, 0, mg.Width, mg.Height, GraphicsUnit.Pixel);
            g.Dispose();
            mg.Dispose();
            return bp;
        }

        static byte[] CompressImage(Bitmap bmp1)
        {
            byte[] theData = null;
            using (MemoryStream theStream = new MemoryStream())
            {
                ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);

                System.Drawing.Imaging.Encoder myEncoder =
                    System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);

                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                myEncoderParameters.Param[0] = myEncoderParameter;
                bmp1.Save(theStream, jgpEncoder, myEncoderParameters);
                theData = theStream.ToArray();
            }
            return theData;
        }

        static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}
