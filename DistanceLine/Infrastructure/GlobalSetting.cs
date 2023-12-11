using DistanceLine.Infrastructure.PlotGlobalization;
using DistanceLine.Views.Windows;
using HandyControl.Data;
using HandyControl.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistanceLine.Infrastructure
{
    /// <summary>
    /// Глобальные настройки.
    /// </summary>
    public static class GlobalSetting
    {
        /// <summary>
        /// Установить тему.
        /// </summary>
        /// <param name="skinType"></param>
        public static void SetTheme(SkinType skinType)
        {
            Theme.SetSkin(MainWindow.Instanse, skinType);
            PlotProvider.SkinType = skinType;
        }

        public static void SetFontSize(double fontSize)
        {
            MainWindow.Instanse.FontSize = fontSize;
            PlotProvider.FontSize = fontSize;
        }
    }
}
