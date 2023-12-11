using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using DistanceLine.Models;
using DistanceLine.ViewModels.Windows;

namespace DistanceLine.ViewModels
{
    public class CorrectionFactorViewModel : ObserverViewModel
    {
        #region Свойства

        #region Отображаемая коллекция

        /// <summary>
        /// Отображаемая коллекция.
        /// </summary>
        public ObservableCollection<DataShell> ViewItemsBefore
        {
            get { return _viewItemsBefore; }
            set { Set(ref _viewItemsBefore, value); }
        }

        private ObservableCollection<DataShell> _viewItemsBefore;

        /// <summary>
        /// Отображаемая коллекция.
        /// </summary>
        public ObservableCollection<DataShell> ViewItemsAfter
        {
            get { return _viewItemsAfter; }
            set { Set(ref _viewItemsAfter, value); }
        }

        private ObservableCollection<DataShell> _viewItemsAfter;

        #endregion

        #endregion

        /// <summary>
        /// Конструктор без параметров.
        /// </summary>
        public CorrectionFactorViewModel() : base()
        {
            MainWindowViewModel.InputData.PropertyChanged += (sender, e) =>
            {
                Update();
            };

            Update();
        }

        /// <summary>
        /// Обновление данных страницы.
        /// </summary>
        public override Task Update()
        {
            ViewItemsBefore = new ObservableCollection<DataShell>()
            {
                new DataShell("По активному сопротивлению", MainWindowViewModel.InputData.CorrectionFactor.FactorActiveResistance),
                new DataShell("По индуктивному сопротивлению", MainWindowViewModel.InputData.CorrectionFactor.FactorCapacitiveResistance),
                new DataShell("По ёмкостному сопротивлению", MainWindowViewModel.InputData.CorrectionFactor.FactorCapacitiveConductivity),
                new DataShell("Активное сопротивление схемы замещения", MainWindowViewModel.InputData.CorrectionFactor.PShemeActiveResistance),
                new DataShell("Индуктивное сопротивление схемы замещения", MainWindowViewModel.InputData.CorrectionFactor.PShemeCapacitiveResistance),
                new DataShell("Емкостное сопротивление схемы замещения", MainWindowViewModel.InputData.CorrectionFactor.PShemeCapacitiveConductivity),
                new DataShell("Суммарная проводимость компенсирующих реакторов", MainWindowViewModel.InputData.CorrectionFactor.TotalConductivityReactor),
                new DataShell("Номинальная мощность реактора", MainWindowViewModel.InputData.CorrectionFactor.NominalPowerOneReactor),
            };

            ViewItemsAfter = new ObservableCollection<DataShell>()
            {
                new DataShell("По активному сопротивлению", MainWindowViewModel.InputData.CorrectionFactor.FactorActiveResistanceAfter),
                new DataShell("По индуктивному сопротивлению", MainWindowViewModel.InputData.CorrectionFactor.FactorCapacitiveResistanceAfter),
                new DataShell("По ёмкостному сопротивлению", MainWindowViewModel.InputData.CorrectionFactor.FactorCapacitiveConductivityAfter),
                new DataShell("Активное сопротивление схемы замещения", MainWindowViewModel.InputData.CorrectionFactor.PShemeActiveResistanceAfter),
                new DataShell("Индуктивное сопротивление схемы замещения", MainWindowViewModel.InputData.CorrectionFactor.PShemeCapacitiveResistanceAfter),
                new DataShell("Емкостное сопротивление схемы замещения", MainWindowViewModel.InputData.CorrectionFactor.PShemeCapacitiveConductivityAfter),
            };

            return Task.CompletedTask;
        }
    }
}
