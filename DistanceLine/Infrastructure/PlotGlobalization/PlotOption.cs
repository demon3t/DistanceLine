using OxyPlot;
using System.Diagnostics.CodeAnalysis;

namespace DistanceLine.Infrastructure.PlotGlobalization
{
    /// <summary>
    /// Настройки графика.
    /// </summary>
    public class PlotOption
    {
        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="title">Название графика.</param>
        public PlotOption(string title)
        {
            Title = title;
        }

        /// <summary>
        /// Заголовок.
        /// </summary>
        [AllowNull]
        public string Title { get; set; } = null;

        /// <summary>
        /// Цвет текста.
        /// </summary>
        public OxyColor TextColor { get; set; } = PlotProvider.TextColor;

        /// <summary>
        /// Цвет рамки графика.
        /// </summary>
        public OxyColor PlotAreaBorderColor { get; set; } = PlotProvider.PlotAreaBorderColor;
    }
}
