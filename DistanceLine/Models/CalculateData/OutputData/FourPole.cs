using System.Numerics;

namespace DistanceLine.Models.CalculateData.OutputData
{
    /// <summary>
    /// Четырёхполюсник.
    /// </summary>
    public class FourPole
    {
        private readonly InputData _inputData;

        private readonly InputData _inputDataZero;

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="inputData">Входные данные.</param>
        public FourPole(InputData inputData)
        {
            _inputData = inputData;
            _inputDataZero = new InputData(inputData, 0);
        }

        #region С активным сопротивлением.

        /// <summary>
        /// Коэффициент A.
        /// </summary>
        public Complex A =>
            Complex.Cosh(Complex.Multiply(_inputData.WavePropagationCoefficient, _inputData.LineLength));

        /// <summary>
        /// Коэффициент B.
        /// </summary>
        public Complex B =>
            Complex.Multiply(_inputData.WaveResistanceLine, Complex.Sinh(Complex.Multiply(_inputData.WavePropagationCoefficient, _inputData.LineLength)));

        /// <summary>
        /// Коэффициент C.
        /// </summary>
        public Complex C =>
            Complex.Divide(Complex.Sinh(Complex.Multiply(_inputData.WavePropagationCoefficient, _inputData.LineLength)), _inputData.WaveResistanceLine);

        /// <summary>
        /// Коэффициент D.
        /// </summary>
        public Complex D => A;

        #endregion

        #region Без активного сопротивления.

        /// <summary>
        /// Коэффициент A.
        /// </summary>
        public Complex Azero =>
            Complex.Cosh(Complex.Multiply(_inputDataZero.WavePropagationCoefficient, _inputDataZero.LineLength));

        /// <summary>
        /// Коэффициент B.
        /// </summary>
        public Complex Bzero =>
            Complex.Multiply(_inputDataZero.WaveResistanceLine, Complex.Sinh(Complex.Multiply(_inputDataZero.WavePropagationCoefficient, _inputDataZero.LineLength)));

        /// <summary>
        /// Коэффициент C.
        /// </summary>
        public Complex Czero =>
            Complex.Divide(Complex.Sinh(Complex.Multiply(_inputDataZero.WavePropagationCoefficient, _inputDataZero.LineLength)), _inputDataZero.WaveResistanceLine);

        /// <summary>
        /// Коэффициент D.
        /// </summary>
        public Complex Dzero => Azero;

        #endregion
    }
}
