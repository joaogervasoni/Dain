using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Dain.Utils
{
    public class ImageHandler
    {
        public static byte[] HttpPostedFileBaseToByteArray(HttpPostedFileBase imageFile)
        {
            if (imageFile == null) return null;
            byte[] data;
            using (Stream inputStream = imageFile.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                data = memoryStream.ToArray();
            }
            // return string.Format($"data:{imageFile.ContentType};base64,{Convert.ToBase64String(data)}");
            return data;
        }
        

        /// <summary>
        /// This is a helper method to encode the image byte array to a format that the browser understand
        /// </summary>
        /// <param name="image">The image in byte array</param>
        /// <param name="imageType">The type of the image</param>
        /// <returns>A image in base64</returns>
        public static string PhotoBase64(byte[] image, string imageType)
        {
            if (image == null || string.IsNullOrEmpty(imageType)) return null;

            return string.Format($"data:{imageType};base64,{Convert.ToBase64String(image)}");
        }
    }
}