using System.Collections.ObjectModel;
using System.Drawing.Text;
using System.IO;
using System.Text.Json;
using System.Windows.Media;

namespace DistanceLine.Models
{
    internal class Shell
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public Shell(object value, string name)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Размеры шрифтов.
        /// </summary>
        public static ObservableCollection<Shell> FontSizes
        {
            get
            {
                return new ObservableCollection<Shell>()
                {
                    new Shell(10d, "10"),
                    new Shell(11d, "11"),
                    new Shell(12d, "12"),
                    new Shell(14d, "14"),
                    new Shell(16d, "16"),
                    new Shell(18d, "18"),
                    new Shell(20d, "20"),
                    new Shell(22d, "22"),
                    new Shell(24d, "24"),
                    new Shell(26d, "26"),
                    new Shell(28d, "28"),
                    new Shell(30d, "30"),
                };
            }
        }

        /// <summary>
        /// Шрифты.
        /// </summary>
        public static ObservableCollection<Shell> FontFamilies
        {
            get
            {
                var installedFontCollection = new InstalledFontCollection();
                var fontFamilies = installedFontCollection.Families;

                var collection = new ObservableCollection<Shell>();

                foreach (var fontFamily in fontFamilies)
                {
                    collection.Add(new Shell(new FontFamily(fontFamily.Name), fontFamily.Name));
                }

                return collection;
            }
        }


        /// <summary>
        /// Шрифты.
        /// </summary>
        public static ObservableCollection<Shell> Themes
        {
            get
            {
                return new ObservableCollection<Shell>()
                {
                    new Shell(HandyControl.Data.SkinType.Default, "Светлая"),
                    new Shell(HandyControl.Data.SkinType.Dark, "Тёмная"),
                    new Shell(HandyControl.Data.SkinType.Violet, "Фиолетовая"),
                };
            }
        }
    }
}
