using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows.Input;
using DistanceLine.Infrastructure;
using DistanceLine.Models;
using DistanceLine.ViewModels.Windows;
using DistanceLine.Infrastructure.PlotGlobalization;

namespace DistanceLine.ViewModels
{
    public class LongitudinalCompensationViewModel : ObserverViewModel
    {

        #region Свойства

        #region Отображаемая коллекция

        /// <summary>
        /// График натуральной мощности.
        /// </summary>
        public PlotModel NaturalPowerGraph { get { return _naturalPowerGraph; } }

        private PlotModel _naturalPowerGraph;

        /// <summary>
        /// График волнового сопротивления.
        /// </summary>
        public PlotModel WaveResistanceGraph { get { return _waveResistanceGraph; } }

        private PlotModel _waveResistanceGraph;

        /// <summary>
        /// График волнового сопротивления.
        /// </summary>
        public PlotModel CornerGraph { get { return _cornerGraph; } }

        private PlotModel _cornerGraph;

        /// <summary>
        /// График A.
        /// </summary>
        public PlotModel AGraph { get { return _aGraph; } }

        private PlotModel _aGraph;

        /// <summary>
        /// График B.
        /// </summary>
        public PlotModel BGraph { get { return _bGraph; } }

        private PlotModel _bGraph;

        /// <summary>
        /// График C.
        /// </summary>
        public PlotModel CGraph { get { return _cGraph; } }

        private PlotModel _cGraph;

        /// <summary>
        /// График D.
        /// </summary>
        public PlotModel DGraph { get { return _dGraph; } }

        private PlotModel _dGraph;

        #endregion

        #endregion

        /// <summary>
        /// Конструктор без параметров.
        /// </summary>
        public LongitudinalCompensationViewModel() : base()
        {
            #region Команды

            ZoomEnableCommand = new RelayCommand(OnZoomEnableCommandExecuted, CanZoomEnableCommandExecute);

            #endregion

            _naturalPowerGraph = PlotProvider.GetPlotModel(
                new PlotOption("Натуральная мощность"),
                new AxesOption()
                {
                    Axis =
                    {
                        new AxesOption("Степень компенсации", OxyPlot.Axes.AxisPosition.Bottom)
                        {
                            MinimumMajorStep = 0.25,
                        },
                        new AxesOption("Натуральная мощность, МВт", OxyPlot.Axes.AxisPosition.Left),
                    },
                });

            _waveResistanceGraph = PlotProvider.GetPlotModel(
                new PlotOption("Волновое сопротивление"),
                new AxesOption()
                {
                    Axis =
                    {
                        new AxesOption("Степень компенсации", OxyPlot.Axes.AxisPosition.Bottom)
                        {
                            MinimumMajorStep = 0.25,
                        },
                        new AxesOption("Волновое сопротивление, Ом", OxyPlot.Axes.AxisPosition.Left),
                    },
                });

            _cornerGraph = PlotProvider.GetPlotModel(
                new PlotOption("Угол"),
                new AxesOption()
                {
                    Axis =
                    {
                        new AxesOption("Степень компенсации", OxyPlot.Axes.AxisPosition.Bottom)
                        {
                            MinimumMajorStep = 0.25,
                        },
                        new AxesOption("Угол, град", OxyPlot.Axes.AxisPosition.Left),
                    },
                });

            _aGraph = PlotProvider.GetPlotModel(
                new PlotOption("A"),
                new AxesOption()
                {
                    Axis =
                    {
                        new AxesOption("Степень компенсации", OxyPlot.Axes.AxisPosition.Bottom)
                        {
                            MinimumMajorStep = 0.25,
                        },
                        new AxesOption("A", OxyPlot.Axes.AxisPosition.Left),
                    },
                });

            _bGraph = PlotProvider.GetPlotModel(
                new PlotOption("B"),
                new AxesOption()
                {
                    Axis =
                    {
                        new AxesOption("Степень компенсации", OxyPlot.Axes.AxisPosition.Bottom)
                        {
                            MinimumMajorStep = 0.25,
                        },
                        new AxesOption("B, Ом", OxyPlot.Axes.AxisPosition.Left),
                    },
                });

            _cGraph = PlotProvider.GetPlotModel(
                new PlotOption("C"),
                new AxesOption()
                {
                    Axis =
                    {
                        new AxesOption("Степень компенсации", OxyPlot.Axes.AxisPosition.Bottom)
                        {
                            MinimumMajorStep = 0.25,
                        },
                        new AxesOption("C, См", OxyPlot.Axes.AxisPosition.Left),
                    },
                });

            _dGraph = PlotProvider.GetPlotModel(
                new PlotOption("D"),
                new AxesOption()
                {
                    Axis =
                    {
                        new AxesOption("Степень компенсации", OxyPlot.Axes.AxisPosition.Bottom)
                        {
                            MinimumMajorStep = 0.25,
                        },
                        new AxesOption("D", OxyPlot.Axes.AxisPosition.Left),
                    },
                });
        }

        public override Task Update()
        {
            ((LineSeries)NaturalPowerGraph.Series.First()).Points.Clear();
            ((LineSeries)NaturalPowerGraph.Series.First()).Points.AddRange(GetPointCollection(InputData.LongitudinalCompensation.GetPc_Reactor));
            NaturalPowerGraph.InvalidatePlot(true);

            ((LineSeries)WaveResistanceGraph.Series.First()).Points.Clear();
            ((LineSeries)WaveResistanceGraph.Series.First()).Points.AddRange(GetPointCollection(InputData.LongitudinalCompensation.GetZw_Reactor));
            WaveResistanceGraph.InvalidatePlot(true);

            ((LineSeries)CornerGraph.Series.First()).Points.Clear();
            ((LineSeries)CornerGraph.Series.First()).Points.AddRange(GetPointCollection(InputData.LongitudinalCompensation.GetCorner_Reactor));
            CornerGraph.InvalidatePlot(true);

            ((LineSeries)AGraph.Series.First()).Points.Clear();
            ((LineSeries)AGraph.Series.First()).Points.AddRange(GetPointCollection(InputData.LongitudinalCompensation.GetA_Upk));
            AGraph.InvalidatePlot(true);

            ((LineSeries)BGraph.Series.First()).Points.Clear();
            ((LineSeries)BGraph.Series.First()).Points.AddRange(GetPointCollection(InputData.LongitudinalCompensation.GetB_Upk));
            BGraph.InvalidatePlot(true);

            ((LineSeries)CGraph.Series.First()).Points.Clear();
            ((LineSeries)CGraph.Series.First()).Points.AddRange(GetPointCollection(InputData.LongitudinalCompensation.GetC_Upk));
            CGraph.InvalidatePlot(true);

            ((LineSeries)DGraph.Series.First()).Points.Clear();
            ((LineSeries)DGraph.Series.First()).Points.AddRange(GetPointCollection(InputData.LongitudinalCompensation.GetD_Upk));
            DGraph.InvalidatePlot(true);

            return Task.CompletedTask;
        }

        private IEnumerable<DataPoint> GetPointCollection(Func<double, double> func)
        {
            for (var i = 0d; i < 1; i += 0.25)
            {
                yield return new DataPoint(i, func(i));
            }

            yield break;
        }

        private IEnumerable<DataPoint> GetPointCollection(Func<double, Complex> func)
        {
            for (var i = 0d; i < 1; i+= 0.25)
            {
                yield return new DataPoint(i, func(i).Magnitude);
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
