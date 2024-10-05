using LightPlayer.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace LightPlayer.Converters
{
    public class ItemSourceToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<Musician>)
            {
                var musicians = (IEnumerable<Musician>)value;
                return musicians.Count() == 0 ? Visibility.Visible : Visibility.Hidden;
            }
            else
            {
                var albums = (IEnumerable<Album>)value;
                return albums.Count() == 0 ? Visibility.Visible : Visibility.Hidden;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
