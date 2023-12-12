using DistanceLine.Infrastructure;
using DistanceLine.Infrastructure.PlotGlobalization;
using DistanceLine.Models;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DistanceLine.ViewModels
{
    public class VoltageDistributionViewModel : ObserverViewModel
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

        public VoltageDistributionViewModel() : base()
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
                        new AxesOption("Длина линии, км", AxisPosition.Bottom),
                        new AxesOption("Напряжение, кВ", AxisPosition.Left),
                    }
                },
                new Series[]
                {
                    new LineSeries()
                    {
                        MarkerType = MarkerType.None,
                        Color = OxyColors.Blue,
                        LineStyle = LineStyle.Solid,
                        BrokenLineThickness = 3,
                        RenderInLegend = true,
                        Title = "Одностороннее включение",
                    },
                    new LineSeries()
                    {
                        MarkerType = MarkerType.None,
                        Color = OxyColors.Red,
                        LineStyle = LineStyle.Solid,
                        BrokenLineThickness = 3,
                        RenderInLegend = true,
                        Title = "Мощность меньше натуральной",
                    },
                    new LineSeries()
                    {
                        MarkerType = MarkerType.None,
                        Color = OxyColors.DarkOrange,
                        LineStyle = LineStyle.Solid,
                        BrokenLineThickness = 3,
                        RenderInLegend = true,
                        Title = "Натуральная мощность",
                    },
                    new LineSeries()
                    {
                        MarkerType = MarkerType.None,
                        Color = OxyColors.Green,
                        LineStyle = LineStyle.Solid,
                        BrokenLineThickness = 3,
                        RenderInLegend = true,
                        Title = "Мощность больше натуральной",
                    }
                });
        }

        public override Task Update()
        {
            ((LineSeries)VoltageDistribution.Series[0]).Points.Clear();
            ((LineSeries)VoltageDistribution.Series[0]).Points.AddRange(GetPointCollection(InputData.VoltageDistribution.GetVoltageCollection_OneSided));

            ((LineSeries)VoltageDistribution.Series[1]).Points.Clear();
            ((LineSeries)VoltageDistribution.Series[1]).Points.AddRange(GetPointCollection(InputData.VoltageDistribution.GetVoltageCollection_LessNaturalPower));

            ((LineSeries)VoltageDistribution.Series[2]).Points.Clear();
            ((LineSeries)VoltageDistribution.Series[2]).Points.AddRange(GetPointCollection(InputData.VoltageDistribution.GetVoltageCollection_NaturalPower));

            ((LineSeries)VoltageDistribution.Series[3]).Points.Clear();
            ((LineSeries)VoltageDistribution.Series[3]).Points.AddRange(GetPointCollection(InputData.VoltageDistribution.GetVoltageCollection_MoreNaturalPower));

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
