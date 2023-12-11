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
        public static MainWindow Instanse { get; private set; }

        /// <summary>
        /// Зависимости.
        /// </summary>
        public static ServiceCollection Services = new ServiceCollection();

        public MainWindow()
        {
            Instanse = Instanse is null ? this : Instanse;

            Services.AddTransient<MainWindow>(sp => this)
                .BuildServiceProvider();

            InitializeComponent();
        }
    }
}
