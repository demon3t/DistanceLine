using DistanceLine.Infrastructure;
using DistanceLine.Infrastructure.Base;
using DistanceLine.Infrastructure.Converters;
using DistanceLine.Models;
using DistanceLine.Models.CalculateData;
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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WpfApplication.Views;
using JsonSerializer = System.Text.Json.JsonSerializer;
using TabItem = System.Windows.Controls.TabItem;
using Window = HandyControl.Controls.Window;

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
        private readonly MainWindow MainWindow;


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

            using (var services = MainWindow.Services.BuildServiceProvider())
            {
                MainWindow = services.GetService<MainWindow>() ?? throw new ApplicationException("Не инициализирован MainWindow.");
                MainWindow.Closing += (sender, e) =>
                {
                    var directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InternalSave");
                    Directory.CreateDirectory(directoryPath);

                    var filePath = Path.Combine(directoryPath, "data.json");

                    var jsonData = JsonConvert.SerializeObject(new SaveObject(InputData, MainWindow.FontSize, MainWindow.FontFamily, Theme.GetSkin(MainWindow)));

                    File.WriteAllText(filePath, jsonData);
                };
            }

            var directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InternalSave");
            var filePath = Path.Combine(directoryPath, "data.json");

            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                var so = JsonConvert.DeserializeObject<SaveObject>(jsonData);

                if (so is not null)
                {
                    InputData = so.InputData;
                    MainWindow.FontFamily = so.FontFamily;
                    MainWindow.FontSize = so.FontSize;
                    Theme.SetSkin(MainWindow, so.SkinType);
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
                /*
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
                },*/
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

        private async void OnComplexCommandExecuted(object p)
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

        #region Изменить тему

        public ICommand ThemeChangeCommand { get; }

        private bool CanThemeChangeCommandExecute(object p)
        {
            return true;
        }

        private void OnThemeChangeCommandExecuted(object p)
        {
            if (p is Window window)
            {
                Theme.SetSkin(window, Theme.GetSkin(window) == HandyControl.Data.SkinType.Default ?
                    HandyControl.Data.SkinType.Dark :
                    HandyControl.Data.SkinType.Default);
            }

        }

        #endregion

        #endregion
    }
}
