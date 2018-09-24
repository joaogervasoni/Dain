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
            byte[] data;
            using (Stream inputStream = imageFile.InputStream)
            {
                if (!(inputStream is MemoryStream memoryStream))
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                data = memoryStream.ToArray();
            }
            // return string.Format($"data:{imageFile.ContentType};base64,{Convert.ToBase64String(data)}");
            return data;

        }
    }
}