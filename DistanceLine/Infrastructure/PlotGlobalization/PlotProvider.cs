using DistanceLine.Views.Windows;
using HandyControl.Data;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DistanceLine.Infrastructure.PlotGlobalization
{
    public static class PlotProvider
    {
        private static readonly IList<PlotModel> _plotModels = new List<PlotModel>();

        private static MainWindow _mainWindow => MainWindow.Instanse;

        #region Геттеры свойств

        internal static OxyColor TextColor => SkinType == SkinType.Dark ? OxyColors.White :
                SkinType == SkinType.Violet ? OxyColor.FromArgb(255, 122, 31, 163) : OxyColor.FromArgb(255, 50, 109, 242);

        internal static OxyColor PlotAreaBorderColor => SkinType == SkinType.Dark ? OxyColors.White :
                SkinType == SkinType.Violet ? OxyColor.FromArgb(255, 122, 31, 163) : OxyColor.FromArgb(255, 50, 109, 242);

        internal static OxyColor TicklineColor => SkinType == SkinType.Dark ? OxyColors.DarkGray :
                SkinType == SkinType.Violet ? OxyColor.FromArgb(255, 122, 31, 163) : OxyColor.FromArgb(255, 50, 109, 242);

        internal static OxyColor MinorTicklineColor => SkinType == SkinType.Dark ? OxyColors.DarkGray :
                SkinType == SkinType.Violet ? OxyColor.FromArgb(255, 122, 31, 163) : OxyColor.FromArgb(255, 50, 109, 242);

        internal static OxyColor MajorGridlineColor => SkinType == SkinType.Dark ? OxyColors.DarkGray :
                SkinType == SkinType.Violet ? OxyColors.Violet : OxyColors.LightBlue;

        internal static OxyColor MinorGridlineColor = SkinType == SkinType.Dark ? OxyColors.DarkGray :
                SkinType == SkinType.Violet ? OxyColors.Violet : OxyColor.FromArgb(255, 50, 109, 242);

        internal static OxyColor SeriesColor => SkinType == SkinType.Dark ? OxyColors.White :
                SkinType == SkinType.Violet ? OxyColor.FromArgb(255, 122, 31, 163) : OxyColor.FromArgb(255, 50, 109, 242);

        #endregion

        #region Стиль

        public static SkinType SkinType
        {
            get { return _skinType; }
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
            get { return _mainWindow.FontFamily; }
        }


        #endregion

        #region Размер шрифта

        public static double FontSize
        {
            get { return _mainWindow.FontSize; }
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

                if (pm.Series.Count < 2)
                {
                    var series = pm.Series.First();
                    if (series is LineSeries)
                    {
                        ((LineSeries)series).Color = SeriesColor;
                    }

                }

                pm.InvalidatePlot(true);
            });
        }

        public static PlotModel GetPlotModel(PlotOption plotOption, AxesOption axesOption, IEnumerable<Series>? series = null)
        {
            var plot = CreatePlotModel(plotOption);

            foreach (var ax in axesOption.Axis)
            {
                plot.Axes.Add(CreateAxis(ax));
            }

            foreach (var s in CreateSeries(series))
            {
                plot.Series.Add(s);
            }

            _plotModels.Add(plot);

            return plot;
        }

        private static PlotModel CreatePlotModel(PlotOption plotOption)
        {
            var plot = new PlotModel();

            plot.Title = plotOption.Title;
            plot.PlotAreaBorderColor = plotOption.PlotAreaBorderColor;
            plot.TextColor = plotOption.TextColor;

            return plot;
        }

        private static LinearAxis CreateAxis(AxesOption axisOption)
        {
            var axis = new LinearAxis();

            axis.Title = axisOption.Title;
            axis.TitleColor = axisOption.TitleColor;
            axis.MinorTicklineColor = axisOption.MinorTicklineColor;
            axis.TicklineColor = axisOption.TicklineColor;
            axis.Position = axisOption.Position;
            axis.IsZoomEnabled = axisOption.IsZoomEnabled;
            axis.MajorGridlineThickness = axisOption.MajorGridlineThickness;
            axis.MajorGridlineStyle = axisOption.MajorGridlineStyle;
            axis.MajorGridlineColor = axisOption.MajorGridlineColor;
            axis.MinimumMajorStep = axisOption.MinimumMajorStep;

            return axis;
        }

        private static IEnumerable<Series> CreateSeries(IEnumerable<Series>? series = null)
        {
            if (series is not null)
            {
                foreach (var s in series)
                {
                    yield return s;
                }
            }
            else
            {
                yield return new LineSeries
                {
                    MarkerType = MarkerType.Circle,
                    Color = SeriesColor,
                    LineStyle = LineStyle.Solid,
                    BrokenLineThickness = 3,
                    RenderInLegend = false,
                    TrackerFormatString = "{3}: {4:F3}\n{1}: {2:F3}"
                };
            }
        }
    }
}
