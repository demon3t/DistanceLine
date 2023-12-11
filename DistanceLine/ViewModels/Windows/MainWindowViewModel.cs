using DistanceLine.Infrastructure;
using DistanceLine.Infrastructure.Base;
using DistanceLine.Infrastructure.Converters;
using DistanceLine.Infrastructure.PlotGlobalization;
using DistanceLine.Models;
using DistanceLine.Models.CalculateData;
using DistanceLine.Views;
using DistanceLine.Views.Windows;
using HandyControl.Controls;
using HandyControl.Themes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using JsonSerializer = System.Text.Json.JsonSerializer;
using TabItem = System.Windows.Controls.TabItem;

namespace DistanceLine.ViewModels.Windows
{
    /// <summary>
    /// Контекст MainWindow.
    /// </summary>
    public class MainWindowViewModel : ViewModel
    {
        #region Свойства

        #region Входные данные

        /// <summary>
        /// Входные данные.
        /// </summary>
        public InputData InputData
        {
            get { return _inputData; }
            set { Set(ref _inputData, value); }
        }

        private InputData _inputData = new InputData();

        #endregion

        #region Страницы расчёта

        /// <summary>
        /// Коллекция расчётов.
        /// </summary>
        public ObservableCollection<TabItem> TabItems
        {
            get { return _tabItems; }
            set { Set(ref _tabItems, value); }
        }
        private ObservableCollection<TabItem> _tabItems = new ObservableCollection<TabItem>();

        #endregion

        #endregion

        #region Приватные поля

        /// <summary>
        /// Путь к файлу сохранения.
        /// </summary>
        private string _filePath = string.Empty;

        /// <summary>
        /// Окно настоек.
        /// </summary>
        [AllowNull]
        internal UserControl ModalWindow;

        /// <summary>
        /// Главное окно.
        /// </summary>
        [DisallowNull]
        public readonly MainWindow GeneralWindow;

        /// <summary>
        /// Доступные размеры шрифтов.
        /// </summary>
        private ObservableCollection<Shell> _fontSizes = Shell.FontSizes;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Конструктор без параметров.
        /// </summary>
        public MainWindowViewModel()
        {
            InputData = new InputData();
            InputData.PropertyChanged += IntutDataPropertyChanged;

            MainWindow.Services.AddSingleton<MainWindowViewModel>(this)
                .AddTransient<InputData>(id => InputData)
                .BuildServiceProvider();


            GeneralWindow = MainWindow.Instanse;

            GeneralWindow.Closing += (sender, e) =>
            {
                var directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InternalSave");
                Directory.CreateDirectory(directoryPath);

                var filePath = Path.Combine(directoryPath, "data.json");

                var jsonData = JsonConvert.SerializeObject(new SaveObject(InputData, GeneralWindow.FontSize, GeneralWindow.FontFamily, Theme.GetSkin(GeneralWindow)));

                File.WriteAllText(filePath, jsonData);
            };

            var directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InternalSave");
            var filePath = Path.Combine(directoryPath, "data.json");

            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                var so = JsonConvert.DeserializeObject<SaveObject>(jsonData);

                if (so is not null)
                {
                    InputData = so.InputData;
                    InputData.PropertyChanged += IntutDataPropertyChanged;
                    GeneralWindow.FontFamily = so.FontFamily;
                    GeneralWindow.FontSize = so.FontSize;
                    Theme.SetSkin(GeneralWindow, so.SkinType);
                    PlotProvider.SkinType = so.SkinType;
                }
            }

            #region Команды

            ThemeChangeCommand = new RelayCommand(OnThemeChangeCommandExecuted, CanThemeChangeCommandExecute);
            UpdateCommand = new RelayCommand(OnUpdateCommandExecuted, CanUpdateCommandExecute);
            OpenFileCommand = new RelayCommand(OnOpenFileCommandExecuted, CanOpenFileCommandExecute);
            SafeFileCommand = new RelayCommand(OnSafeFileCommandExecuted, CanSafeFileCommandExecute);
            SafeFileAsCommand = new RelayCommand(OnSafeFileAsCommandExecuted, CanSafeFileAsCommandExecute);
            OpenSettingsCommand = new RelayCommand(OnOpenSettingsCommandExecuted, CanOpenSettingsCommandExecute);
            OpenReferenceCommand = new RelayCommand(OnOpenReferenceCommandExecuted, CanOpenReferenceCommandExecute);
            ComplexCommand = new RelayCommand(OnComplexCommandExecuted, CanComplexCommandExecute);
            EnlargeFontSize = new RelayCommand(OnEnlargeFontSizeCommandExecuted, CanEnlargeFontSizeCommandExecute);
            ReduceFontSize = new RelayCommand(OnReduceFontSizeCommandExecuted, CanReduceFontSizeCommandExecute);


            #endregion

            InitTabControl();
        }

        #endregion

        #region Приватные методы

        /// <summary>
        /// Заполнить TabControl расчётами.
        /// </summary>
        private void InitTabControl()
        {
            TabItems = new ObservableCollection<TabItem>()
            {
                new TabItem()
                {
                    Header = "ВОЛНОВЫЕ ПАРАМЕТРЫ",
                    Content = ViewManager.GetView<WaveParameters, WaveParametersViewModel>()
                },
                new TabItem()
                {
                    Header = "ДАННЫЕ ЧЕТЫРЁХПОЛЮСНИКА",
                    Content = ViewManager.GetView<FourPole, FourPoleViewModel>()
                },
                new TabItem()
                {
                    Header = "ПОПРАВОЧНЫЕ КОЭФФИЦИЕНТЫ",
                    Content = ViewManager.GetView<CorrectionFactor, CorrectionFactorViewModel>()
                },
                new TabItem()
                {
                    Header = "РЕАКТОР",
                    Content = ViewManager.GetView<Reactor, ReactorViewModel>()
                },
                new TabItem()
                {
                    Header = "ПРОДОЛЬНАЯ КОМПЕНСАЦИЯ",
                    Content = ViewManager.GetView<LongitudinalCompensation, LongitudinalCompensationViewModel>()
                },
                new TabItem()
                {
                    Header = "РАСПРЕДЕЛЕНИЕ НАПРЯЖЕНИЯ",
                    Content = ViewManager.GetView<VoltageDistribution, VoltageDistributionViewModel>()
                },
                new TabItem()
                {
                    Header = "ПОДДЕРЖАНИЕ НАПРЯЖЕНИЯ",
                    Content = ViewManager.GetView<VoltageMaintenance, VoltageMaintenanceViewModel>()
                },
            };

            UpdateCommand.Execute(true);
        }

        /// <summary>
        /// Обновить расчёты.
        /// </summary>
        private void IntutDataPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateCommand?.Execute(this);
        }

        #endregion

        #region Команды

        #region Открыть настройки

        public ICommand OpenSettingsCommand { get; }

        private bool CanOpenSettingsCommandExecute(object p)
        {
            return ModalWindow is null;
        }

        private void OnOpenSettingsCommandExecuted(object p)
        {
            ModalWindow = new SettingWindow()
            {
                DataContext = new SettingWindowViewModel(),
            };
            Dialog.Show(ModalWindow);
        }

        #endregion

        #region Изменить отображение комплексных значений

        public ICommand ComplexCommand { get; }

        private bool CanComplexCommandExecute(object p)
        {
            return true;
        }

        private void OnComplexCommandExecuted(object p)
        {
            ComplexConverter.IsAlgebraicConverter = !ComplexConverter.IsAlgebraicConverter;
            UpdateCommand?.Execute(this);
        }

        #endregion

        #region О приложении

        public ICommand OpenReferenceCommand { get; }

        private bool CanOpenReferenceCommandExecute(object p)
        {
            return false;
        }

        private void OnOpenReferenceCommandExecuted(object p)
        {

        }

        #endregion

        #region Открыть файл

        public ICommand OpenFileCommand { get; }

        private bool CanOpenFileCommandExecute(object p)
        {
            return true;
        }

        private void OnOpenFileCommandExecuted(object p)
        {
            var openFileDialog = new OpenFileDialog()
            {
                AddExtension = true,
                Filter = "Вильнер|*.vilner|Дальние линии|*.dlep|Все файлы|*.dlep;*.vilner",
                Title = "Открыть файл",
            };

            if (!openFileDialog.ShowDialog() ?? false)
            {
                return;
            }

            _filePath = openFileDialog.FileName;

            var inputData = JsonSerializer.Deserialize<InputData>(File.ReadAllText(_filePath));

            if (inputData is not null)
            {
                UpdateCommand?.Execute(this);
            }
        }

        #endregion

        #region Сохранить файл

        public ICommand SafeFileCommand { get; }

        private bool CanSafeFileCommandExecute(object p)
        {
            return InputData is not null && File.Exists(_filePath);
        }

        private void OnSafeFileCommandExecuted(object p)
        {
            File.WriteAllText(_filePath, JsonSerializer.Serialize<InputData>(InputData));

            UpdateCommand?.Execute(null);
        }

        #endregion

        #region Сохранить файл как

        public ICommand SafeFileAsCommand { get; }

        private bool CanSafeFileAsCommandExecute(object p)
        {
            return InputData is not null;
        }

        private void OnSafeFileAsCommandExecuted(object p)
        {
            var saveFileAsDialog = new OpenFileDialog()
            {
                AddExtension = true,
                Filter = "Вильнер|*.vilner|Дальние линии|*.dlep|Все файлы|*.dlep;*.vilner",
                CheckFileExists = false,
                Title = "Сохранить файл",
            };

            saveFileAsDialog.ShowDialog();

            _filePath = saveFileAsDialog.FileName;

            if (string.IsNullOrEmpty(_filePath))
            {
                return;
            }

            File.WriteAllText(_filePath, JsonSerializer.Serialize<InputData>(InputData));
        }

        #endregion

        #region Обновить

        public ICommand UpdateCommand { get; }

        private bool CanUpdateCommandExecute(object p)
        {
            return true;
        }

        private async void OnUpdateCommandExecuted(object p)
        {
            foreach (var tabItem in TabItems)
            {
                await ((ObserverViewModel)((UserControl)tabItem.Content).DataContext).Update();
            }
        }

        #endregion

        #region Увеличить размер шрифта

        public ICommand EnlargeFontSize { get; }

        private bool CanEnlargeFontSizeCommandExecute(object p)
        {
            return _fontSizes.Max(s => (double)(s.Value)) > GeneralWindow.FontSize;
        }

        private void OnEnlargeFontSizeCommandExecuted(object p)
        {
            var fontSize = (double)_fontSizes
                .Where(s => (double)s.Value > GeneralWindow.FontSize)
                .OrderBy(s => (double)s.Value)
                .First().Value;

            GlobalSetting.SetFontSize(fontSize);
        }

        #endregion

        #region Увеличить размер шрифта

        public ICommand ReduceFontSize { get; }

        private bool CanReduceFontSizeCommandExecute(object p)
        {
            return _fontSizes.Min(s => (double)(s.Value)) < GeneralWindow.FontSize;
        }

        private void OnReduceFontSizeCommandExecuted(object p)
        {
            var fontSize = (double)_fontSizes
                .Where(s => (double)s.Value < GeneralWindow.FontSize)
                .OrderByDescending(s => (double)s.Value)
                .First().Value;

            GlobalSetting.SetFontSize(fontSize);
        }

        #endregion

        #region Изменить тему

        public ICommand ThemeChangeCommand { get; }

        private bool CanThemeChangeCommandExecute(object p)
        {
            return true;
        }

        private void OnThemeChangeCommandExecuted(object p)
        {
            switch (Theme.GetSkin(GeneralWindow))
            {
                case HandyControl.Data.SkinType.Default:
                    {
                        GlobalSetting.SetTheme(HandyControl.Data.SkinType.Dark);
                        break;
                    }
                case HandyControl.Data.SkinType.Dark:
                    {
                        GlobalSetting.SetTheme(HandyControl.Data.SkinType.Violet);
                        break;
                    }
                case HandyControl.Data.SkinType.Violet:
                    {
                        GlobalSetting.SetTheme(HandyControl.Data.SkinType.Default);
                        break;
                    }
            }
        }

        #endregion

        #endregion
    }
}
