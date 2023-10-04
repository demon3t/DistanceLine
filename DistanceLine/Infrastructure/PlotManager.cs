using DistanceLine.Views.Windows;
using HandyControl.Data;
using HandyControl.Themes;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using FontFamilyConverter = DistanceLine.Infrastructure.Converters.FontFamilyConverter;
using FontSizeConverter = DistanceLine.Infrastructure.Converters.FontSizeConverter;

namespace DistanceLine.Infrastructure
{
    public static class PlotManager
    {
        private static readonly IList<PlotModel> _plotModels = new List<PlotModel>();

        public static MainWindow MainWindow
        {
            get { return _mainWindow; }
            set
            {
                _mainWindow = value;

                var fontFamilyPropertyBinding = new Binding()
                {
                    Source = _mainWindow,
                    Converter = new FontFamilyConverter()
                };
                _mainWindow.SetBinding(MainWindow.FontFamilyProperty, fontFamilyPropertyBinding);
                DependencyPropertyDescriptor.FromProperty(MainWindow.FontFamilyProperty, typeof(MainWindow))
                    .AddValueChanged(MainWindow, OnPlotFontFamilyChanced);


                var fontSizePropertyBinding = new Binding()
                {
                    Source = _mainWindow,
                    Converter = new FontSizeConverter()
                };
                _mainWindow.SetBinding(MainWindow.FontSizeProperty, fontSizePropertyBinding);
                DependencyPropertyDescriptor.FromProperty(MainWindow.FontSizeProperty, typeof(MainWindow))
                    .AddValueChanged(MainWindow, OnPlotFontSizeChanced);
            }
        }

        private static MainWindow _mainWindow = new MainWindow();

        #region Геттеры свойств

        private static OxyColor TextColor => SkinType == SkinType.Dark ? OxyColors.White :
                SkinType == SkinType.Violet ? OxyColor.FromArgb(255, 122, 31, 163) : OxyColor.FromArgb(255, 50, 109, 242);

        private static OxyColor PlotAreaBorderColor => SkinType == SkinType.Dark ? OxyColors.White :
                SkinType == SkinType.Violet ? OxyColor.FromArgb(255, 122, 31, 163) : OxyColor.FromArgb(255, 50, 109, 242);

        private static OxyColor TicklineColor => SkinType == SkinType.Dark ? OxyColors.DarkGray :
                SkinType == SkinType.Violet ? OxyColor.FromArgb(255, 122, 31, 163) : OxyColor.FromArgb(255, 50, 109, 242);

        private static OxyColor MinorTicklineColor => SkinType == SkinType.Dark ? OxyColors.DarkGray :
                SkinType == SkinType.Violet ? OxyColors.Violet : OxyColor.FromArgb(255, 50, 109, 242);

        private static OxyColor MajorGridlineColor => SkinType == SkinType.Dark ? OxyColors.DarkGray :
                SkinType == SkinType.Violet ? OxyColors.Violet : OxyColors.LightBlue;

        private static OxyColor MinorGridlineColor = SkinType == SkinType.Dark ? OxyColors.DarkGray :
                SkinType == SkinType.Violet ? OxyColors.Violet : OxyColor.FromArgb(255, 50, 109, 242);

        #endregion

        #region Стиль

        public static SkinType SkinType
        {
            get { return Theme.GetSkin(MainWindow); }
            set
            {
                _skinType = value;
                OnPlotSkinChanced();
            }
        }

        private static SkinType _skinType;

        #endregion

        #region Размер шрифта

        public static FontFamily FontFamily
        {
            get { return MainWindow.FontFamily; }
        }


        #endregion

        #region Размер шрифта

        public static double FontSize
        {
            get { return MainWindow.FontSize; }
        }


        #endregion


        private static void OnPlotFontFamilyChanced(object? sender, EventArgs e)
        {
            Parallel.ForEach(_plotModels, pm =>
            {
                pm.TitleFont = FontFamily.ToString();

                Parallel.ForEach(pm.Axes, ax =>
                {
                    ax.Font = FontFamily.ToString();
                    ax.TitleFont = FontFamily.ToString();
                });

                pm.InvalidatePlot(true);
            });
        }

        private static void OnPlotFontSizeChanced(object? sender, EventArgs e)
        {
            Parallel.ForEach(_plotModels, pm =>
            {
                pm.TitleFontSize = FontSize * 1.5;

                Parallel.ForEach(pm.Axes, ax =>
                {
                    ax.FontSize = FontSize;
                    ax.TitleFontSize = FontSize * 1.5;
                });

                pm.InvalidatePlot(true);
            });
        }

        private static void OnPlotSkinChanced()
        {
            Parallel.ForEach(_plotModels, pm =>
            {
                pm.TextColor = TextColor;
                pm.PlotAreaBorderColor = PlotAreaBorderColor;

                Parallel.ForEach(pm.Axes, ax =>
                {
                    ax.TicklineColor = TicklineColor;
                    ax.MinorTicklineColor = MinorTicklineColor;
                    ax.MajorGridlineColor = MajorGridlineColor;
                    ax.MinorGridlineColor = MinorGridlineColor;
                });

                pm.InvalidatePlot(true);
            });
        }

        public static PlotModel GetPlotModel(string title = "График", string xTitle = "Ось X", string yTitle = "Ось Y", IEnumerable<Series>? series = null)
        {
            var plot = new PlotModel()
            {
                Title = title,
                TextColor = TextColor,
                PlotAreaBorderColor = PlotAreaBorderColor,
                Axes =
                {
                    new LinearAxis
                    {
                        Title = xTitle,
                        TitleColor = OxyColors.Transparent,
                        MinorTicklineColor = MinorTicklineColor,
                        TicklineColor = TicklineColor,
                        Position = AxisPosition.Bottom,
                        IsZoomEnabled = false,
                        MajorGridlineThickness = 1,
                        MajorGridlineStyle = LineStyle.Solid,
                        MajorGridlineColor = MajorGridlineColor,
                    },
                    new LinearAxis
                    {
                        Title = yTitle,
                        TitleColor = OxyColors.Transparent,
                        MinorTicklineColor = MinorTicklineColor,
                        TicklineColor = TicklineColor,
                        Position = AxisPosition.Left,
                        IsZoomEnabled = false,
                        MajorGridlineThickness = 1,
                        MajorGridlineStyle = LineStyle.Solid,
                        MajorGridlineColor = MajorGridlineColor,
                    },
                },
            };

            if (series is not null)
            {
                Parallel.ForEach(series, s =>
                {
                    plot.Series.Add(s);
                });
            }
            else
            {
                plot.Series.Add(new LineSeries
                {
                    MarkerType = MarkerType.Circle,
                    Color = OxyColors.DarkBlue,
                    LineStyle = LineStyle.Solid,
                    BrokenLineThickness = 3,
                    RenderInLegend = false,
                });
            }

            _plotModels.Add(plot);

            return plot;
        }
    }
}
