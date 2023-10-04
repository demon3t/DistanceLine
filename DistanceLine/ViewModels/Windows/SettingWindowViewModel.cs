using DistanceLine.Infrastructure;
using DistanceLine.Infrastructure.Base;
using DistanceLine.Views.Windows;
using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Themes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using WpfApplication.Infrastructure;

namespace DistanceLine.ViewModels.Windows
{
    /// <summary>
    /// Контекст SettingWindow.
    /// </summary>
    public class SettingWindowViewModel : ViewModel
    {
        /// <summary>
        /// Конструктор без параметров.
        /// </summary>
        public SettingWindowViewModel()
        {
            CloseCommand = new RelayCommand(OnCloseCommandExecuted, CanCloseCommandExecute);
            SaveCommand = new RelayCommand(OnSaveCommandExecuted, CanSaveCommandExecute);
            DefaultSettingsCommand = new RelayCommand(OnDefaultSettingsCommandExecuted, CanDefaultSettingsCommandExecute);

            using (var services = MainWindow.Services.BuildServiceProvider())
            {
                MainWindowViewModel = services.GetService<MainWindowViewModel>() ?? throw new ApplicationException("Не инициализирован ViewModel главного окна.");
                MainWindow = services.GetService<MainWindow>() ?? throw new ApplicationException("Не инициализирован MainWindow.");
            }

            TempValues = new Dictionary<Type, object>()
            {
                { typeof(double), MainWindow.FontSize },
                { typeof(FontFamily), MainWindow.FontFamily },
                { typeof(SkinType), Theme.GetSkin(MainWindow) }
            };

            _applicationWindowFontSize = MainWindow.FontSize;
            _applicationWindowFontFamily = MainWindow.FontFamily;
            _applicationWindowSkinType = Theme.GetSkin(MainWindow);
        }

        #region Приватные свойства 

        /// <summary>
        /// Экземпляр ViewModel главного окна.
        /// </summary>
        private MainWindowViewModel MainWindowViewModel { get; }

        /// <summary>
        /// Экземпляр главного окна.
        /// </summary>
        public MainWindow MainWindow { get; }

        /// <summary>
        /// Временные стандартные значения.
        /// </summary>
        private Dictionary<Type, object> TempValues { get; }

        #endregion

        #region Свойства 

        /// <summary>
        /// Размер шрифта приложения.
        /// </summary>
        public double ApplicationWindowFontSize
        {
            get { return _applicationWindowFontSize; }
            set
            {
                Set(ref _applicationWindowFontSize, value);
                MainWindow.FontSize = value;
                foreach (var tabItem in MainWindowViewModel.TabItems)
                {
                    tabItem.FontSize = value;
                }
            }
        }

        private double _applicationWindowFontSize;

        /// <summary>
        /// Шрифт приложения.
        /// </summary>
        public FontFamily ApplicationWindowFontFamily
        {
            get { return _applicationWindowFontFamily; }
            set
            {
                Set(ref _applicationWindowFontFamily, value);
                MainWindow.FontFamily = value;
                foreach (var tabItem in MainWindowViewModel.TabItems)
                {
                    tabItem.FontFamily = value;
                }
            }
        }

        private FontFamily _applicationWindowFontFamily;

        /// <summary>
        /// Тема приложения.
        /// </summary>
        public SkinType ApplicationWindowSkinType
        {
            get { return _applicationWindowSkinType; }
            set
            {
                Set(ref _applicationWindowSkinType, value);
                Theme.SetSkin(MainWindow, value);
                PlotManager.SkinType = value;
            }
        }

        private SkinType _applicationWindowSkinType;

        #endregion

        #region Команды

        #region Закрыть окно

        public ICommand CloseCommand { get; }

        private bool CanCloseCommandExecute(object p)
        {
            return true;
        }

        private void OnCloseCommandExecuted(object p)
        {
            ApplicationWindowFontSize = (double)TempValues[typeof(double)];
            ApplicationWindowFontFamily = (FontFamily)TempValues[typeof(FontFamily)];
            ApplicationWindowSkinType = (SkinType)TempValues[typeof(SkinType)];

            ControlCommands.Close?.Execute(p, null);
            MainWindowViewModel.ModalWindow = null;
        }

        #endregion

        #region Сохранить настройка

        public ICommand SaveCommand { get; }

        private bool CanSaveCommandExecute(object p)
        {
            return ApplicationWindowFontSize != (double)TempValues[typeof(double)] ||
                ApplicationWindowFontFamily != (FontFamily)TempValues[typeof(FontFamily)] ||
                ApplicationWindowSkinType != (SkinType)TempValues[typeof(SkinType)];
        }

        private void OnSaveCommandExecuted(object p)
        {
            TempValues[typeof(double)] = ApplicationWindowFontSize;
            TempValues[typeof(FontFamily)] = ApplicationWindowFontFamily;
            TempValues[typeof(SkinType)] = ApplicationWindowSkinType;

            CloseCommand?.Execute(p);
        }

        #endregion

        #region Сбросить настройки

        public ICommand DefaultSettingsCommand { get; }

        private bool CanDefaultSettingsCommandExecute(object p)
        {
            return ApplicationWindowFontSize != (double)TempValues[typeof(double)] ||
                ApplicationWindowFontFamily != (FontFamily)TempValues[typeof(FontFamily)] ||
                ApplicationWindowSkinType != (SkinType)TempValues[typeof(SkinType)];
        }

        private void OnDefaultSettingsCommandExecuted(object p)
        {
            ApplicationWindowFontSize = (double)TempValues[typeof(double)];
            ApplicationWindowFontFamily = (FontFamily)TempValues[typeof(FontFamily)];
            ApplicationWindowSkinType = (SkinType)TempValues[typeof(SkinType)];
        }

        #endregion

        #endregion
    }
}
