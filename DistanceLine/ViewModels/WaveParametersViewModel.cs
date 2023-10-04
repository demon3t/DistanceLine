using DistanceLine.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DistanceLine.ViewModels
{
    /// <summary>
    /// Контекст WaveParameters.
    /// </summary>
    public class WaveParametersViewModel : ObserverViewModel
    {
        #region Свойства

        #region Отображаемая коллекция

        /// <summary>
        /// Отображаемая коллекция.
        /// </summary>
        public ObservableCollection<DataShell> ViewItems
        {
            get { return _viewItems; }
            set { Set(ref _viewItems, value); }
        }

        private ObservableCollection<DataShell> _viewItems;

        #endregion

        #endregion

        /// <summary>
        /// Конструктор без параметров.
        /// </summary>
        public WaveParametersViewModel() : base()
        {
            _viewItems = new ObservableCollection<DataShell>();
        }

        /// <summary>
        /// Обновление данных страницы.
        /// </summary>
        public override Task Update()
        {
            ViewItems.Clear();
            ViewItems.Add(new DataShell("Средне-геометрическое расстояние между проводами фаз, м",
                InputData.WaveParameters.Single.MeanDistanceBetweenPhaseWires, InputData.WaveParameters.Split.MeanDistanceBetweenPhaseWires));
            ViewItems.Add(new DataShell("Эквивалентный радиус провода, мм",
                InputData.WaveParameters.Split.RadiusWire, InputData.WaveParameters.Single.RadiusWire));
            ViewItems.Add(new DataShell("Индуктивное сопротивление, Ом/км",
                InputData.WaveParameters.Split.ResistanceWire, InputData.WaveParameters.Single.ResistanceWire));
            ViewItems.Add(new DataShell("Емкостная проводимость, См/км",
                InputData.WaveParameters.Split.CapacitiveConductivityWire, InputData.WaveParameters.Single.CapacitiveConductivityWire));
            ViewItems.Add(new DataShell("Комплексное сопротивление, Ом/км",
                InputData.WaveParameters.Split.ComplexResistanceWire, InputData.WaveParameters.Single.ComplexResistanceWire));
            ViewItems.Add(new DataShell("Полная проводимость, См/км",
                InputData.WaveParameters.Split.ComplexConductivityWire, InputData.WaveParameters.Single.ComplexConductivityWire));
            ViewItems.Add(new DataShell("Коэффициент распространения волны, град/км",
                InputData.WaveParameters.Split.WavePropagationCoefficient, InputData.WaveParameters.Single.WavePropagationCoefficient));
            ViewItems.Add(new DataShell("Коэффициент затухания, 1/км",
                InputData.WaveParameters.Split.AttenuatioCoefficient, InputData.WaveParameters.Single.AttenuatioCoefficient));
            ViewItems.Add(new DataShell("Коэффициент фазы, град/км",
                InputData.WaveParameters.Split.PhaseCoefficientDeg, InputData.WaveParameters.Single.PhaseCoefficientDeg));
            ViewItems.Add(new DataShell("Волновое сопротивление, Ом",
                InputData.WaveParameters.Split.WaveResistanceLine, InputData.WaveParameters.Single.WaveResistanceLine));
            ViewItems.Add(new DataShell("Натуральная мощность, МВт",
                InputData.WaveParameters.Split.NaturalPower, InputData.WaveParameters.Single.NaturalPower));

            return Task.CompletedTask;
        }
    }
}
