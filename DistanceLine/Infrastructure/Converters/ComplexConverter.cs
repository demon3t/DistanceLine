using System;
using System.Globalization;
using System.Windows.Data;

namespace DistanceLine.Infrastructure.Converters
{
    [ValueConversion(typeof(object), typeof(string))]
    public class ComplexConverter : IValueConverter
    {
        public static bool IsAlgebraicConverter { get; set; } = true;

        private readonly AlgebraicConverter algebraicConverter = new AlgebraicConverter();

        private readonly IndicativeConverter indicativeConverter = new IndicativeConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (IsAlgebraicConverter)
            {
                return algebraicConverter.Convert(value, targetType, parameter, culture);
            }
            else
            {
                return indicativeConverter.Convert(value, targetType, parameter, culture);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (IsAlgebraicConverter)
            {
                return algebraicConverter.ConvertBack(value, targetType, parameter, culture);
            }
            else
            {
                return indicativeConverter.ConvertBack(value, targetType, parameter, culture);
            }
        }
    }
}
