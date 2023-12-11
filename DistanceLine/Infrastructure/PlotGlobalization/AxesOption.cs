using OxyPlot;
using OxyPlot.Axes;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DistanceLine.Infrastructure.PlotGlobalization
{
    /// <summary>
    /// Настройки осей.
    /// </summary>
    public class AxesOption
    {
        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="title">Заголовок оси.</param>
        /// <param name="position">Расположение оси.</param>
        public AxesOption(string title, AxisPosition position)
        {
            Title = title;
            Position = position;
            Axis = null;
        }

        /// <summary>
        /// Конструктор без параметров.
        /// </summary>
        public AxesOption()
        {

        }

        /// <summary>
        /// Коллекция настроек осей.
        /// </summary>
        [AllowNull]
        public List<AxesOption> Axis { get; set; } = new List<AxesOption>();

        /// <summary>
        /// Название оси.
        /// </summary>
        [AllowNull]
        public string Title { get; set; } = null;

        /// <summary>
        /// Цвет текста.
        /// </summary>
        public OxyColor TitleColor { get; set; } = OxyColors.Transparent;

        /// <summary>
        /// Цвет минорных засечек.
        /// </summary>
        public OxyColor MinorTicklineColor { get; set; } = PlotProvider.MinorTicklineColor;

        /// <summary>
        /// Цвет мажорных засечек.
        /// </summary>
        public OxyColor TicklineColor { get; set; } = PlotProvider.TicklineColor;

        /// <summary>
        /// Расположение оси.
        /// </summary>
        public AxisPosition Position { get; set; } = AxisPosition.Bottom;

        /// <summary>
        /// Возможность приближения.
        /// </summary>
        public bool IsZoomEnabled { get; set; } = false;

        /// <summary>
        /// Толщина мажорных линий сетки.
        /// </summary>
        public double MajorGridlineThickness { get; set; } = 1;

        /// <summary>
        /// Стиль мажорных линий сетки.
        /// </summary>
        public LineStyle MajorGridlineStyle { get; set; } = LineStyle.Solid;

        /// <summary>
        /// Цвет мажорных линий сетки.
        /// </summary>
        [AllowNull]
        public OxyColor MajorGridlineColor { get; set; } = PlotProvider.MajorGridlineColor;

        /// <summary>
        /// Минимальный шаг подписи значения.
        /// </summary>
        [AllowNull]
        public double MinimumMajorStep { get; set; } = 0;
    }
}
