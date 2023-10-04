using System;
using System.Numerics;

namespace DistanceLine.Models.CalculateData.OutputData
{
    /// <summary>
    /// Реактор.
    /// </summary>
    public class Reactor
    {
        private readonly InputData _inputData;

        private readonly CorrectionFactor _correctionFactor;

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="inputData">Входные данные.</param>
        public Reactor(InputData inputData)
        {
            _inputData = inputData;
            _correctionFactor = new(_inputData);
        }

        #region Четырёхполюсник

        private const double Areactor = 1;
        private const double Breactor = 0;
        private const double Dreactor = 1;

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
        public Complex GetA_Reactor(int value) =>
            A1 * Areactor * A2 + A1 * Breactor * C2 + B1 * 
            new Complex(0, -value * _correctionFactor.TotalConductivityReactor) * A2 + B1 * Dreactor * C2;

        /// <summary>
        /// Значение коэффициента B.
        /// </summary>
        /// <param name="value">Степень компенсации.</param>
        /// <returns>Коэффициент B.</returns>
        public Complex GetB_Reactor(int value) =>
            A1 * Areactor * B2 + A1 * Breactor * D2 + B1 *
            new Complex(0, -value * _correctionFactor.TotalConductivityReactor) * B2 + B1 * Dreactor * D2;

        /// <summary>
        /// Значение коэффициента C.
        /// </summary>
        /// <param name="value">Степень компенсации.</param>
        /// <returns>Коэффициент C.</returns>
        public Complex GetC_Reactor(int value) =>
            C1 * Areactor * A2 + C1 * Breactor * C2 + D1 * 
            new Complex(0, -value * _correctionFactor.TotalConductivityReactor) * A2 + D1 * Dreactor * C2;

        /// <summary>
        /// Значение коэффициента D.
        /// </summary>
        /// <param name="value">Степень компенсации.</param>
        /// <returns>Коэффициент D.</returns>
        public Complex GetD_Reactor(int value) =>
            A1 * Areactor * A2 + A1 * Breactor * C2 + B1 * 
            new Complex(0, -value * _correctionFactor.TotalConductivityReactor) * A2 + B1 * Dreactor * C2;

        /// <summary>
        /// Значение волнового сопротивления.
        /// </summary>
        /// <param name="value">Степень компенсации.</param>
        /// <returns>Волновое сопротивление.</returns>
        public double GetZw_Reactor(int value) =>
             Math.Sqrt(GetB_Reactor(value).Magnitude / GetC_Reactor(value).Magnitude);

        /// <summary>
        /// Значение угла.
        /// </summary>
        /// <param name="value">Степень компенсации.</param>
        /// <returns>Угол.</returns>
        public double GetCorner_Reactor(int value) =>
              Math.Atan(Math.Sqrt(GetB_Reactor(value).Magnitude * GetC_Reactor(value).Magnitude /
                  GetA_Reactor(value).Magnitude / GetD_Reactor(value).Magnitude)) * 180 / Math.PI;

        /// <summary>
        /// Значение натуральной мощности.
        /// </summary>
        /// <param name="value">Степень компенсации.</param>
        /// <returns>натуральная мощность.</returns>
        public double GetPc_Reactor(int value) =>
             Math.Pow(_inputData.NominalVoltage, 2) / GetZw_Reactor(value);

        #endregion

    }
}
