using System;
using System.Numerics;

namespace DistanceLine.Models.CalculateData.OutputData
{
    /// <summary>
    /// Продольная компенсация.
    /// </summary>
    public class LongitudinalCompensation
    {
        private readonly InputData _inputData;

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="inputData">Входные данные.</param>
        public LongitudinalCompensation(InputData inputData)
        {
            _inputData = inputData;
        }

        #region Четырёхполюсник

        private const double Upk_A = 1;
        private const double Upk_C = 0;
        private const double Upk_D = 1;

        /// <summary>
        /// Коэффициента A1.
        /// </summary>
        private double A1 =>
            Math.Cos(_inputData.PhaseCoefficient * _inputData.LineLength / 2);

        /// <summary>
        /// Коэффициента A2.
        /// </summary>
        private double A2 =>
            A1;

        /// <summary>
        /// Коэффициента B1.
        /// </summary>
        private Complex B1 =>
            new(0, _inputData.WaveResistanceLine.Magnitude * Math.Sin(_inputData.PhaseCoefficient * _inputData.LineLength / 2));

        /// <summary>
        /// Коэффициента B2.
        /// </summary>
        private Complex B2 =>
            B1;

        /// <summary>
        /// Коэффициента C1.
        /// </summary>
        private Complex C1 =>
            new(0, Math.Sin(_inputData.PhaseCoefficient * _inputData.LineLength / 2) / _inputData.WaveResistanceLine.Magnitude);

        /// <summary>
        /// Коэффициента C2.
        /// </summary>
        private Complex C2 =>
            C1;

        /// <summary>
        /// Коэффициента D1.
        /// </summary>
        private double D1 =>
            A1;

        /// <summary>
        /// Коэффициента D2.
        /// </summary>
        private double D2 =>
            A1;

        #endregion

        #region Расчёт параметров

        /// <summary>
        /// Значение коэффициента A.
        /// </summary>
        /// <param name="value">Степень компенсации.</param>
        /// <returns>Коэффициент A.</returns>
        public Complex GetA_Upk(double value) =>
            A1 * Upk_A * A2 + A1 * new Complex(0, -value * _inputData.ResistanceWire * _inputData.LineLength) *
            C2 + B1 * Upk_C * A2 + B1 * Upk_D * C2;

        /// <summary>
        /// Значение коэффициента B.
        /// </summary>
        /// <param name="value">Степень компенсации.</param>
        /// <returns>Коэффициент B.</returns>
        public Complex GetB_Upk(double value) =>
            A1 * Upk_A * B2 + A1 * new Complex(0, -value * _inputData.ResistanceWire * _inputData.LineLength) *
            D2 + B1 * Upk_C * B2 + B1 * Upk_D * D2;

        /// <summary>
        /// Значение коэффициента C.
        /// </summary>
        /// <param name="value">Степень компенсации.</param>
        /// <returns>Коэффициент C.</returns>
        public Complex GetC_Upk(double value) =>
            C1 * Upk_A * A2 + C1 * new Complex(0, -value * _inputData.ResistanceWire * _inputData.LineLength) *
            C2 + D1 * Upk_C * A2 + D1 * Upk_D * C2;

        /// <summary>
        /// Значение коэффициента D.
        /// </summary>
        /// <param name="value">Степень компенсации.</param>
        /// <returns>Коэффициент D.</returns>
        public Complex GetD_Upk(double value) =>
            A1 * Upk_A * A2 + A1 * new Complex(0, -value * _inputData.ResistanceWire * _inputData.LineLength) *
            C2 + B1 * Upk_C * A2 + B1 * Upk_D * C2;

        /// <summary>
        /// Значение волнового сопротивления.
        /// </summary>
        /// <param name="value">Степень компенсации.</param>
        /// <returns>Волновое сопротивление.</returns>
        public double GetZw_Reactor(double value) =>
             Math.Sqrt(GetB_Upk(value).Magnitude / GetC_Upk(value).Magnitude);

        /// <summary>
        /// Значение угла.
        /// </summary>
        /// <param name="value">Степень компенсации.</param>
        /// <returns>Угол.</returns>
        public double GetCorner_Reactor(double value) =>
              Math.Atan(Math.Sqrt(GetB_Upk(value).Magnitude * GetC_Upk(value).Magnitude / GetA_Upk(value).Magnitude / GetA_Upk(value).Magnitude)) * 180 / Math.PI;

        /// <summary>
        /// Значение натуральной мощности.
        /// </summary>
        /// <param name="value">Степень компенсации.</param>
        /// <returns>натуральная мощность.</returns>
        public double GetPc_Reactor(double value) =>
             Math.Pow(_inputData.NominalVoltage, 2) / Math.Sqrt(GetB_Upk(value).Magnitude / GetC_Upk(value).Magnitude);

        #endregion

    }
}
