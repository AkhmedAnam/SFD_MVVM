using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace SFD_MVVM
{
    public static class CommonMethods
    {
        public static void SaveBitmapSourceToFile(BitmapSource imgSrc, string fileName)
        {
            if (imgSrc != null)
            {
                if (fileName != string.Empty)
                {
                    using (FileStream stream = new FileStream(fileName, FileMode.Create))
                    {
                        BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(imgSrc));
                        encoder.Save(stream);
                        stream.Close();
                    }
                }
            }
            else
                throw new ArgumentNullException("Image source", "Method 'SaveBitmapSourceToFile' got null reference");
        }
    }
}
