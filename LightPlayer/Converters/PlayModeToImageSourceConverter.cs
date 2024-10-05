using LightPlayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LightPlayer.Converters
{
    class PlayModeToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PlayMode playMode = (PlayMode)value;
            if (playMode == PlayMode.OrderPlay)
                return "pack://application:,,,/Properties/OrderPlay.png";
            else if(playMode == PlayMode.OrderRepeat)
                return "pack://application:,,,/Properties/ListRepeat.png";
            else
                return "pack://application:,,,/Properties/SingleRepeat.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
