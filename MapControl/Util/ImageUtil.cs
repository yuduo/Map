﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace HongLi.MapControl.Util
{
    /// <summary>
    /// 图片工具类
    /// </summary>
    public class ImageUtil
    {
        /// <summary>
        /// 将图片数据转换为Base64字符串
        /// </summary>
        /// <param name="img"></param>
        public static string ImageToBase64(Image img)
        {
            var binFormatter = new BinaryFormatter();
            var memStream = new MemoryStream();
            binFormatter.Serialize(memStream, img);
            var bytes = memStream.GetBuffer();
            var base64 = Convert.ToBase64String(bytes);
            return base64;
        }

        /// <summary>
        /// 将Base64字符串转换为图片
        /// </summary>
        /// <param name="base64"></param>
        public static Image Base64ToImage(string base64)
        {
            var bytes = Convert.FromBase64String(base64);
            var memStream = new MemoryStream(bytes);
            var binFormatter = new BinaryFormatter();
            var img = (Image)binFormatter.Deserialize(memStream);
            return img;
        }

        /// <summary>
        /// Convert Image to Byte[]
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static byte[] ImageToBytes(Image image)
        {
            var format = image.RawFormat;
            using (var ms = new MemoryStream())
            {
                if (format.Equals(ImageFormat.Jpeg))
                {
                    image.Save(ms, ImageFormat.Jpeg);
                }
                else if (format.Equals(ImageFormat.Png))
                {
                    image.Save(ms, ImageFormat.Png);
                }
                else if (format.Equals(ImageFormat.Bmp))
                {
                    image.Save(ms, ImageFormat.Bmp);
                }
                else if (format.Equals(ImageFormat.Gif))
                {
                    image.Save(ms, ImageFormat.Gif);
                }
                else if (format.Equals(ImageFormat.Icon))
                {
                    image.Save(ms, ImageFormat.Icon);
                }
                byte[] buffer = new byte[ms.Length];
                //Image.Save()会改变MemoryStream的Position，需要重新Seek到Begin
                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }

        /// <summary>
        /// Convert Byte[] to Image
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static Image BytesToImage(byte[] buffer)
        {
            var ms = new MemoryStream(buffer);
            var image = Image.FromStream(ms);
            return image;
        }

        /// <summary>
        /// Convert Byte[] to a picture and Store it in file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string CreateImageFromBytes(string fileName, byte[] buffer)
        {
            string file = fileName;
            Image image = BytesToImage(buffer);
            ImageFormat format = image.RawFormat;
            if (format.Equals(ImageFormat.Jpeg))
            {
                file += ".jpeg";
            }
            else if (format.Equals(ImageFormat.Png))
            {
                file += ".png";
            }
            else if (format.Equals(ImageFormat.Bmp))
            {
                file += ".bmp";
            }
            else if (format.Equals(ImageFormat.Gif))
            {
                file += ".gif";
            }
            else if (format.Equals(ImageFormat.Icon))
            {
                file += ".icon";
            }
            FileInfo info = new FileInfo(file);
            if (info.Directory != null) Directory.CreateDirectory(info.Directory.FullName);
            File.WriteAllBytes(file, buffer);
            return file;
        }
    }
}
