using DistanceLine.Models.CalculateData;
using HandyControl.Data;
using System.Windows.Media;

namespace DistanceLine.Infrastructure
{
    /// <summary>
    /// Объект сохранения.
    /// </summary>
    public class SaveObject
    {
        public SaveObject(InputData inputData, double fontSize, FontFamily fontFamily, SkinType skinType)
        {
            InputData = inputData;
            FontSize = fontSize;
            FontFamily = fontFamily;
            SkinType = skinType;
        }

        public InputData InputData { get; set; }

        public double FontSize { get; set; }

        public FontFamily FontFamily { get; set; }

        public SkinType SkinType { get; set; }
    }
}
