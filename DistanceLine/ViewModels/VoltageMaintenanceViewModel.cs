using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DistanceLine.Infrastructure;
using DistanceLine.Models;
using DistanceLine.ViewModels.Windows;
using DistanceLine.Infrastructure.PlotGlobalization;

namespace DistanceLine.ViewModels
{
    public class VoltageMaintenanceViewModel : ObserverViewModel
    {
        #region Свойства

        #region Отображаемая коллекция

        /// <summary>
        /// График натуральной мощности.
        /// </summary>
        public PlotModel VoltageDistribution { get { return _voltageDistribution; } }

        private PlotModel _voltageDistribution;

        #endregion

        #endregion

        public VoltageMaintenanceViewModel() : base()
        {
            #region Команды

            ZoomEnableCommand = new RelayCommand(OnZoomEnableCommandExecuted, CanZoomEnableCommandExecute);

            #endregion

            _voltageDistribution = PlotProvider.GetPlotModel(
                new PlotOption("Распределение напряжения"),
                new AxesOption()
                {
                    Axis =
                    {
                        new AxesOption("Активная мощность, МВт", AxisPosition.Bottom),
                        new AxesOption("Реактивная мощность, Мвар", AxisPosition.Left),
                    }
                },
                new Series[]
                {
                    new LineSeries
                    {
                        MarkerType = MarkerType.None,
                        Color = OxyColors.Blue,
                        LineStyle = LineStyle.Solid,
                        BrokenLineThickness = 3,
                        RenderInLegend = false,
                    },
                    new LineSeries
                    {
                        MarkerType = MarkerType.None,
                        Color = OxyColors.Red,
                        LineStyle = LineStyle.Solid,
                        BrokenLineThickness = 3,
                        RenderInLegend = false,
                    },
                    new LineSeries
                    {
                        MarkerType = MarkerType.None,
                        Color = OxyColors.DarkOrange,
                        LineStyle = LineStyle.Solid,
                        BrokenLineThickness = 3,
                        RenderInLegend = false,
                    },
                    new LineSeries
                    {
                        MarkerType = MarkerType.None,
                        Color = OxyColors.Green,
                        LineStyle = LineStyle.Solid,
                        BrokenLineThickness = 3,
                        RenderInLegend = false,
                    }
                });
        }

        public override Task Update()
        {
            ((LineSeries)VoltageDistribution.Series[0]).Points.Clear();
            ((LineSeries)VoltageDistribution.Series[0]).Points.AddRange(GetPointCollection(InputData.VoltageMaintenance.Q1_n));

            ((LineSeries)VoltageDistribution.Series[1]).Points.Clear();
            ((LineSeries)VoltageDistribution.Series[1]).Points.AddRange(GetPointCollection(InputData.VoltageMaintenance.Q2_n));

            ((LineSeries)VoltageDistribution.Series[2]).Points.Clear();
            ((LineSeries)VoltageDistribution.Series[2]).Points.AddRange(GetPointCollection(InputData.VoltageMaintenance.Q1_k));

            ((LineSeries)VoltageDistribution.Series[3]).Points.Clear();
            ((LineSeries)VoltageDistribution.Series[3]).Points.AddRange(GetPointCollection(InputData.VoltageMaintenance.Q2_k));

            VoltageDistribution.InvalidatePlot(true);

            return Task.CompletedTask;
        }

        private IEnumerable<DataPoint> GetPointCollection(Func<IEnumerable<(double, double)>> func)
        {
            foreach (var item in func())
            {
                yield return new DataPoint(item.Item1, item.Item2);
            }

            yield break;
        }

        #region Команды

        #region Разрешить приближение

        public ICommand ZoomEnableCommand { get; }

        private bool CanZoomEnableCommandExecute(object p)
        {
            if (p is PlotView plotView)
            {
                return plotView.Model.Axes is not null;
            }

            return false;
        }

        private void OnZoomEnableCommandExecuted(object p)
        {
            if (p is PlotView plotView)
            {
                foreach (var axis in plotView.Model.Axes)
                {
                    axis.IsZoomEnabled = !axis.IsZoomEnabled;
                }
            }
        }

        #endregion

        #endregion
    }
}
