using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Drawing;

namespace ProjectManager.Converters
{
    public class ColorToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var colorName = value as string;

            if (colorName == null)
                return Brushes.White;

            return colorName.ToLower() switch
            {
                "white" => Brushes.Black,
                "yellow" => Brushes.Black,
                "cyan" => Brushes.Black,
                _ => Brushes.White
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

}
