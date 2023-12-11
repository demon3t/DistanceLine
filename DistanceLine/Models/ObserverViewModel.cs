using DistanceLine.Infrastructure.Base;
using DistanceLine.Models.CalculateData;
using DistanceLine.ViewModels.Windows;
using DistanceLine.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using WpfApplication.Infrastructure.Base;

namespace DistanceLine.Models
{
    /// <summary>
    /// Базовый класс для ViewModel TabItem.
    /// </summary>
    public abstract class ObserverViewModel : ViewModel, IUpdate
    {
        /// <summary>
        /// Конструктор без параметров.
        /// </summary>
        /// <exception cref="ApplicationException"></exception>
        public ObserverViewModel()
        {
            using (var services = MainWindow.Services.BuildServiceProvider())
            {
                MainWindowViewModel = services.GetService<MainWindowViewModel>() ?? throw new ApplicationException("ViewModel главного окна не инициализирован.");
                InputData = services.GetService<InputData>() ?? throw new ApplicationException("Исходные данные не инициализированы.");
            }
        }

        /// <summary>
        /// ViewModel главного окна.
        /// </summary>
        public MainWindowViewModel MainWindowViewModel { get; set; }

        /// <summary>
        /// Исходные данные.
        /// </summary>
        public InputData InputData
        {
            get { return _inputData; }
            set { Set(ref _inputData, value); }
        }

        private InputData _inputData;

        /// <summary>
        /// Обновление данных.
        /// </summary>
        public abstract Task Update();
    }
}
