using System;
using System.Numerics;

namespace DistanceLine.Models.CalculateData.OutputData
{
    /// <summary>
    /// Поправочные коэффициенты.
    /// </summary>
    public class CorrectionFactor
    {
        private readonly InputData _inputData;

        /// <summary>
        /// Конструктор с параметром.
        /// </summary>
        /// <param name="inputData">Входные данные.</param>
        public CorrectionFactor(InputData inputData)
        {
            _inputData = inputData;
        }

        #region До отключения реактора


        /// <summary>
        /// Поправочный коэффициент по активному сопротивлению [Kr_half].
        /// </summary>
        public double FactorActiveResistance =>
            1 - _inputData.LineLength / 2 * _inputData.LineLength / 2 / 3 * _inputData.ResistanceWire * _inputData.CapacitiveConductivityWire;

        /// <summary>
        /// Поправочный коэффициент по емкостному сопротивлению [Kx_half].
        /// </summary>
        public double FactorCapacitiveResistance =>
             1 - 0.25 * _inputData.LineLength * _inputData.LineLength *
            (_inputData.ResistanceWire * _inputData.CapacitiveConductivityWire - _inputData.ActiveResistance *
            _inputData.ActiveResistance * _inputData.CapacitiveConductivityWire / _inputData.ResistanceWire) / 6;

        /// <summary>
        /// Поправочный коэффициент по емкостной проводимости [Kb_half].
        /// </summary>
        public double FactorCapacitiveConductivity =>
            0.5 * (3 + FactorActiveResistance) / (1 + FactorActiveResistance);

        /// <summary>
        /// Активное сопротивление П-образной схемы [R_cell_half].
        /// </summary>
        public double PShemeActiveResistance =>
            _inputData.ActiveResistance * _inputData.LineLength / 2 * FactorActiveResistance;

        /// <summary>
        /// Индуктивное сопротивление П-образной схемы [X_cell_half].
        /// </summary>
        public double PShemeCapacitiveResistance =>
            _inputData.ResistanceWire * _inputData.LineLength / 2 * FactorCapacitiveResistance;

        /// <summary>
        /// Ёмкость П-образной схемы [B_cell_half].
        /// </summary>
        public double PShemeCapacitiveConductivity =>
            _inputData.CapacitiveConductivityWire / 2 * _inputData.LineLength / 2 * FactorCapacitiveConductivity;

        /// <summary>
        /// Суммарная проводимость компенсирующих реакторов [B_r].
        /// </summary>
        public double TotalConductivityReactor =>
            2 * PShemeCapacitiveConductivity / 3;

        /// <summary>
        /// Номинальная мощность одного реактора [Q_reactor].
        /// </summary>
        public double NominalPowerOneReactor =>
            _inputData.NominalVoltage * _inputData.NominalVoltage * TotalConductivityReactor;


        #endregion

        #region После отключения реактора

        /// <summary>
        /// Поправочный коэффициент по активному сопротивлению после отключения ректора [Kr].
        /// </summary>
        public double FactorActiveResistanceAfter =>
            1 - _inputData.LineLength * _inputData.LineLength * _inputData.ResistanceWire * _inputData.CapacitiveConductivityWire / 3;

        /// <summary>
        /// Поправочный коэффициент по емкостному сопротивлению после отключения ректора [Kx].
        /// </summary>
        public double FactorCapacitiveResistanceAfter =>
            1 - _inputData.LineLength * _inputData.LineLength * 
            (_inputData.ResistanceWire * _inputData.CapacitiveConductivityWire - _inputData.ActiveResistance *
            _inputData.ActiveResistance * _inputData.CapacitiveConductivityWire / _inputData.ResistanceWire) / 6;

        /// <summary>
        /// Поправочный коэффициент по емкостной проводимости после отключения ректора [Kb].
        /// </summary>
        public double FactorCapacitiveConductivityAfter =>
            0.5 * (3 + FactorActiveResistanceAfter) / (1 + FactorActiveResistanceAfter);

        /// <summary>
        /// Активное сопротивление П-образной схемы после отключения ректора [R_cell].
        /// </summary>
        public double PShemeActiveResistanceAfter =>
            _inputData.ActiveResistance * _inputData.LineLength * FactorActiveResistanceAfter;

        /// <summary>
        /// Индуктивное сопротивление П-образной схемы после отключения ректора [X_cell].
        /// </summary>
        public double PShemeCapacitiveResistanceAfter =>
            _inputData.ResistanceWire * _inputData.LineLength * FactorCapacitiveResistanceAfter;

        /// <summary>
        /// Ёмкость П-образной схемы после отключения ректора [B_cell].
        /// </summary>
        public double PShemeCapacitiveConductivityAfter =>
            _inputData.CapacitiveConductivityWire * _inputData.LineLength * FactorCapacitiveConductivityAfter / 2;

        #endregion

    }
}
