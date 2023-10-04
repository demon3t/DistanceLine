using DistanceLine.Infrastructure;
using HandyControl.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace DistanceLine.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Зависимости.
        /// </summary>
        public static ServiceCollection Services = new ServiceCollection();

        public MainWindow()
        {
            Services.AddTransient<MainWindow>(sp => this)
                .BuildServiceProvider();

            PlotManager.MainWindow = this;

            InitializeComponent();
        }
    }
}
