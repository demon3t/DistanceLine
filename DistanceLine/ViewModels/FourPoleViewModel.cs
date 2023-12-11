using DistanceLine.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DistanceLine.ViewModels
{
    public class FourPoleViewModel : ObserverViewModel
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
        public FourPoleViewModel() : base()
        {
            ViewItems = new ObservableCollection<DataShell>();
        }

        /// <summary>
        /// Обновление данных страницы.
        /// </summary>
        public override Task Update()
        {
            ViewItems.Clear();
            ViewItems.Add(new DataShell("A",
                InputData.FourPole.A, InputData.FourPole.Azero));
            ViewItems.Add(new DataShell("B, Ом",
                InputData.FourPole.B, InputData.FourPole.Bzero));
            ViewItems.Add(new DataShell("C, См",
                InputData.FourPole.C, InputData.FourPole.Czero));
            ViewItems.Add(new DataShell("D",
                InputData.FourPole.D, InputData.FourPole.Dzero));

            return Task.CompletedTask;
        }
    }
}
