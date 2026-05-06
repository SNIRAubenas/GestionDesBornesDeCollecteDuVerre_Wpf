using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GestionBornesCollecte.Converters
{
    // transforme le niveau de remplissage en couleur pour l affichage WPF
    // WPF peut pas mettre de logique dans le XAML donc on utilise un IValueConverter
    public class NiveauToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int niveau = (int)value;

            // rouge si plus de 85%, orange entre 50 et 85, vert en dessous
            if (niveau >= 85)
                return new SolidColorBrush(Colors.Red);
            if (niveau >= 50)
                return new SolidColorBrush(Colors.Orange);
            return new SolidColorBrush(Colors.Green);
        }

        // pas besoin dans notre cas, on convertit jamais dans l autre sens
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}