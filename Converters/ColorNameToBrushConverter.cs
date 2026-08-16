using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ProjectManager.Converters
{
    // Convertit une chaîne comme "White", "Red", "#FF0000" en Brush pour les bordures de cartes.
    // Retombe sur du blanc si le nom de couleur est invalide ou vide.
    public class ColorNameToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var colorName = value as string;

            if (string.IsNullOrWhiteSpace(colorName))
                return Brushes.White;

            try
            {
                var color = (Color)ColorConverter.ConvertFromString(colorName);
                return new SolidColorBrush(color);
            }
            catch
            {
                return Brushes.White;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
