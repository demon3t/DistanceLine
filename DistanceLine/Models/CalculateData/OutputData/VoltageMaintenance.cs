using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace DistanceLine.Models.CalculateData.OutputData
{
    /// <summary>
    /// Поддержание напряжения.
    /// </summary>
    public class VoltageMaintenance
    {
        private readonly InputData _inputData;

        private readonly FourPole _fourPole;

        /// <summary>
        /// Конструктор с параметром.
        /// </summary>
        /// <param name="inputData">Входные данные.</param>
        public VoltageMaintenance(InputData inputData)
        {
            _inputData = inputData;
            _fourPole = inputData.FourPole;
        }

        #region По данным начала

        /// <summary>
        /// Получить распределение реактивной мощности по данным начала.
        /// </summary>
        /// <returns>Распределение реактивной мощности.</returns>
        public IEnumerable<(double Xs, double Ys)> Q1_n()
        {
            var count = (int)(_inputData.NaturalPower + _inputData.NaturalPower * 0.1) < 3 ?
                3 : (int)(_inputData.NaturalPower + _inputData.NaturalPower * 0.15);

            var _B = _fourPole.B;
            var _D = _fourPole.D;

            for (int i = 0; i < count; i++)
            {
                yield return (i,
                    (Complex.Conjugate(_D) * _B).Imaginary * _inputData.NominalVoltage * _inputData.NominalVoltage / _B.Magnitude / _B.Magnitude -
                Math.Sqrt(Math.Pow(_inputData.NominalVoltage * _inputData.NominalVoltage / _B.Magnitude, 2) -
                Math.Pow(i - (Complex.Conjugate(_D) * _B).Real * _inputData.NominalVoltage * _inputData.NominalVoltage / _B.Magnitude / _B.Magnitude, 2)));
            }

            yield break;
        }

        /// <summary>
        /// Получить реактивную мощность по данным начала при определённой активной мощности.
        /// </summary>
        /// <param name="P1">Активная мощность.</param>
        /// <returns>Реактивная мощность</returns>
        public double Q1_n(double P1)
        {
            var _B = _fourPole.B;
            var _D = _fourPole.D;

            return (Complex.Conjugate(_D) * _B).Imaginary * _inputData.NominalVoltage * _inputData.NominalVoltage / _B.Magnitude / _B.Magnitude -
                Math.Sqrt(Math.Pow(_inputData.NominalVoltage * _inputData.NominalVoltage / _B.Magnitude, 2) -
                Math.Pow(P1 - (Complex.Conjugate(_D) * _B).Real * _inputData.NominalVoltage * _inputData.NominalVoltage / _B.Magnitude / _B.Magnitude, 2));
        }


        /// <summary>
        /// Получить распределение реактивной мощности по данным начала.
        /// </summary>
        /// <returns>Распределение реактивной мощности.</returns>
        public IEnumerable<(double Xs, double Ys)> Q2_n()
        {
            var count = (int)(_inputData.NaturalPower + _inputData.NaturalPower * 0.1) < 3 ?
                3 : (int)(_inputData.NaturalPower + _inputData.NaturalPower * 0.15);

            var _A = _fourPole.A;
            var _B = _fourPole.B;
            var _C = _fourPole.C;
            var _D = _fourPole.D;

            for (int i = 0; i < count; i++)
            {
                var h1 = -_D * Complex.Conjugate(_C) * _inputData.NominalVoltage * _inputData.NominalVoltage;
                var h2 = -_B * Complex.Conjugate(_A) * (Math.Pow(i, 2) + Math.Pow(Q1_n(i), 2)) / _inputData.NominalVoltage / _inputData.NominalVoltage;
                var h3 = (_B * Complex.Conjugate(_C) + _D * Complex.Conjugate(_A)) * i;
                var h4 = Complex.ImaginaryOne * (-_B * Complex.Conjugate(_C) + _D * Complex.Conjugate(_A)) * Q1_n(i);

                yield return (i, (h1 + h2 + h3 + h4).Imaginary);
            }

            yield break;
        }

        /// <summary>
        /// Получить реактивную мощность по данным начала при определённой активной мощности.
        /// </summary>
        /// <param name="P1">Активная мощность.</param>
        /// <returns>Реактивная мощность</returns>
        public double Q2_n(double P1)
        {
            var _A = _fourPole.A;
            var _B = _fourPole.B;
            var _C = _fourPole.C;
            var _D = _fourPole.D;

            var h1 = -_D * Complex.Conjugate(_C) * _inputData.NominalVoltage * _inputData.NominalVoltage;
            var h2 = -_B * Complex.Conjugate(_A) * (Math.Pow(P1, 2) + Math.Pow(Q1_n(P1), 2)) / _inputData.NominalVoltage / _inputData.NominalVoltage;
            var h3 = (_B * Complex.Conjugate(_C) + _D * Complex.Conjugate(_A)) * P1;
            var h4 = Complex.ImaginaryOne * (-_B * Complex.Conjugate(_C) + _D * Complex.Conjugate(_A)) * Q1_n(P1);

            return (h1 + h2 + h3 + h4).Imaginary;
        }


        #endregion

        #region По данный конца

        /// <summary>
        /// Получить распределение реактивной мощности по данным конца.
        /// </summary>
        /// <returns>Распределение реактивной мощности.</returns>
        public IEnumerable<(double Xs, double Ys)> Q2_k()
        {
            var count = (int)(_inputData.NaturalPower + _inputData.NaturalPower * 0.1) < 3 ?
                3 : (int)(_inputData.NaturalPower + _inputData.NaturalPower * 0.15);

            var _A = _fourPole.A;
            var _B = _fourPole.B;

            for (int i = 0; i < count; i++)
            {
                yield return (i,
                    -1 * (Complex.Conjugate(_A) * _B).Imaginary * _inputData.NominalVoltage * _inputData.NominalVoltage / _B.Magnitude / _B.Magnitude +
                    Math.Sqrt(Math.Pow(_inputData.NominalVoltage * _inputData.NominalVoltage / _B.Magnitude, 2) -
                    Math.Pow(i + (Complex.Conjugate(_A) * _B).Real * _inputData.NominalVoltage * _inputData.NominalVoltage / _B.Magnitude / _B.Magnitude, 2)));
            }

            yield break;
        }

        /// <summary>
        /// Получить реактивную мощность по данным конца при определённой активной мощности.
        /// </summary>
        /// <param name="P1">Активная мощность.</param>
        /// <returns>Реактивная мощность</returns>
        public double Q2_k(double P2)
        {
            var _A = _fourPole.A;
            var _B = _fourPole.B;

            return -1 * (Complex.Conjugate(_A) * _B).Imaginary * _inputData.NominalVoltage * _inputData.NominalVoltage / _B.Magnitude / _B.Magnitude +
                Math.Sqrt(Math.Pow(_inputData.NominalVoltage * _inputData.NominalVoltage / _B.Magnitude, 2) - Math.Pow(P2 + 
                (Complex.Conjugate(_A) * _B).Real * _inputData.NominalVoltage * _inputData.NominalVoltage / _B.Magnitude / _B.Magnitude, 2));
        }

        /// <summary>
        /// Получить распределение реактивной мощности по данным конца.
        /// </summary>
        /// <returns>Распределение реактивной мощности.</returns>
        public IEnumerable<(double Xs, double Ys)> Q1_k()
        {
            var count = (int)(_inputData.NaturalPower + _inputData.NaturalPower * 0.1) < 3 ?
                3 : (int)(_inputData.NaturalPower + _inputData.NaturalPower * 0.15);

            var _A = _fourPole.A;
            var _B = _fourPole.B;
            var _C = _fourPole.C;
            var _D = _fourPole.D;

            for (int i = 0; i < count; i++)
            {
                var h1 = _A * Complex.Conjugate(_C) * _inputData.NominalVoltage * _inputData.NominalVoltage;
                var h2 = _B * Complex.Conjugate(_D) * (Math.Pow(i, 2) + Math.Pow(Q2_k(i), 2)) / _inputData.NominalVoltage / _inputData.NominalVoltage;
                var h3 = (_B * Complex.Conjugate(_C) + _A * Complex.Conjugate(_D)) * i;
                var h4 = Complex.ImaginaryOne * (-_B * Complex.Conjugate(_C) + _A * Complex.Conjugate(_D)) * Q2_k(i);

                yield return (i, (h1 + h2 + h3 + h4).Imaginary);
            }

            yield break;
        }

        /// <summary>
        /// Получить реактивную мощность по данным конца при определённой активной мощности.
        /// </summary>
        /// <param name="P1">Активная мощность.</param>
        /// <returns>Реактивная мощность</returns>
        public double Q1_k(double P2)
        {
            var _A = _fourPole.A;
            var _B = _fourPole.B;
            var _C = _fourPole.C;
            var _D = _fourPole.D;

            var h1 = _A * Complex.Conjugate(_C) * _inputData.NominalVoltage * _inputData.NominalVoltage;
            var h2 = _B * Complex.Conjugate(_D) * (Math.Pow(P2, 2) + Math.Pow(Q2_k(P2), 2)) / _inputData.NominalVoltage / _inputData.NominalVoltage;
            var h3 = (_B * Complex.Conjugate(_C) + _A * Complex.Conjugate(_D)) * P2;
            var h4 = Complex.ImaginaryOne * (-_B * Complex.Conjugate(_C) + _A * Complex.Conjugate(_D)) * Q2_k(P2);

            return (h1 + h2 + h3 + h4).Imaginary;
        }

        #endregion
    }
}
