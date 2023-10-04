using System.Windows;

namespace DistanceLine.Infrastructure
{
    /// <summary>
    /// Менеджер View.
    /// </summary>
    internal static class ViewManager
    {
        /// <summary>
        /// Получить View с установленным DataContext.
        /// </summary>
        /// <returns></returns>
        public static TView GetView<TView, TViewModel>()
            where TView : FrameworkElement, new()
            where TViewModel : new()
        {
            return new TView()
            {
                DataContext = new TViewModel(),
            };
        }

    }
}
