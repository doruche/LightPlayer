using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace LightPlayer.Converters
{
    public class PathToBitmapSource : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = (string)value;
            TagLib.File song = TagLib.File.Create(path);
            if (song.Tag.Pictures.Length > 0)
            {
                return GetBitmapSource(song.Tag.Pictures[0].Data.Data);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private BitmapSource GetBitmapSource(byte[] b)
        {
            using (MemoryStream stream = new MemoryStream(b))
            {
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();

                // This is important: the image should be loaded on setting the StreamSource property, afterwards the stream will be closed
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.DecodePixelWidth = 160;

                // Set this to 160 to get exactly 160x160 image
                // Comment it out to retain original aspect ratio having the image width 160 and auto calculated image height
                bi.DecodePixelHeight = 160;

                bi.StreamSource = stream;
                bi.EndInit();
                return bi;
            }
        }
    }
}
