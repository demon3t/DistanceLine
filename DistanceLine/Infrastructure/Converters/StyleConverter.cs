﻿using DistanceLine.Views.Windows;
using System;
using System.Globalization;
using System.Windows.Data;

namespace DistanceLine.Infrastructure.Converters
{
    public class StyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MainWindow mainWindow)
            {
                return mainWindow.Style;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

}
