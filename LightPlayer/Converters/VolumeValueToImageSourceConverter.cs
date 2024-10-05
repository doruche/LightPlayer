using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LightPlayer.Converters
{
    public class VolumeValueToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double volume = (double)value;
            if (volume == 0)
                return "pack://application:,,,/Properties/volume-mute-line.png";
            else if (volume <= 4)
                return "pack://application:,,,/Properties/volume-down-line.png";
            else
                return "pack://application:,,,/Properties/Volume.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
