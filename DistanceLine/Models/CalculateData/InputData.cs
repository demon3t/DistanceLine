using DistanceLine.Infrastructure.Base;
using DistanceLine.Models.CalculateData.OutputData;
using System;
using System.Numerics;

namespace DistanceLine.Models.CalculateData
{
    /// <summary>
    /// Входные данные.
    /// </summary>
    [Serializable]
    public class InputData : ViewModel
    {
        #region Конструкторы

        /// <summary>
        /// Конструктор без параметров.
        /// </summary>
        public InputData()
        {
            NominalVoltage = 500;
            LineLength = 700;
            NumberSplitWires = 3;
            ActiveResistance = 0.02;
            DiameterWire = 37.5;
            DistanceBetweenPhases = 11.7;
            SteelSection = 500;
            AluminumSection = 336;
            DistanceBetweenSplitWires = 48;
            K1 = 0.55;
            K2 = 1.45;
        }

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="inputData">Входные данные.</param>
        /// <param name="activeResistance">Активное сопротивление.</param>
        public InputData(InputData inputData, double activeResistance)
        {
            NominalVoltage = inputData.NominalVoltage;
            LineLength = inputData.LineLength;
            NumberSplitWires = inputData.NumberSplitWires;
            ActiveResistance = activeResistance;
            DiameterWire = inputData.DiameterWire;
            DistanceBetweenPhases = inputData.DistanceBetweenPhases;
            SteelSection = inputData.SteelSection;
            AluminumSection = inputData.AluminumSection;
            DistanceBetweenSplitWires = inputData.DistanceBetweenSplitWires;
            K1 = inputData.K1;
            K2 = inputData.K2;
        }

        #endregion

        /// <summary>
        /// Волновые параметры.
        /// </summary>
        public WaveParameters WaveParameters { get { return new(this); } }

        /// <summary>
        /// Четырёхполюсник.
        /// </summary>
        public FourPole FourPole { get { return new(this); } }

        /// <summary>
        /// Поправочные коэффициенты.
        /// </summary>
        public CorrectionFactor CorrectionFactor { get { return new(this); } }

        /// <summary>
        /// Реактор.
        /// </summary>
        public Reactor Reactor { get { return new(this); } }

        /// <summary>
        /// Продольная компенсация.
        /// </summary>
        public LongitudinalCompensation LongitudinalCompensation { get { return new(this); } }

        /// <summary>
        /// Распределение напряжения.
        /// </summary>
        public VoltageDistribution VoltageDistribution { get { return new(this); } }

        #region Свойства

        #region Номинальное напряжение [VoltNom]

        /// <summary>
        /// Номинальное напряжение [VoltNom].
        /// </summary>
        public double NominalVoltage
        {
            get { return _nominalVoltage; }
            set { Set(ref _nominalVoltage, value); }
        }

        private double _nominalVoltage;

        #endregion

        #region Длина линии [Length]

        /// <summary>
        /// Длина линии [Length].
        /// </summary>
        public double LineLength
        {
            get { return _lineLength; }
            set { Set(ref _lineLength, value); }
        }

        private double _lineLength;

        #endregion

        #region Количество расщеплённых проводов [N_split]

        /// <summary>
        /// Количество расщеплённых проводов [N_split].
        /// </summary>
        public int NumberSplitWires
        {
            get { return _numberSplitWires; }
            set { Set(ref _numberSplitWires, value); }
        }

        private int _numberSplitWires;

        #endregion

        #region Активное сопротивление [R0]

        /// <summary>
        /// Активное сопротивление [R0].
        /// </summary>
        public double ActiveResistance
        {
            get { return _activeResistance; }
            set { Set(ref _activeResistance, value); }
        }

        private double _activeResistance;

        #endregion

        #region Расстояние между фазами [D_phase]

        /// <summary>
        /// Расстояние между фазами [D_phase].
        /// </summary>
        public double DistanceBetweenPhases
        {
            get { return _distanceBetweenPhases; }
            set { Set(ref _distanceBetweenPhases, value); }
        }

        private double _distanceBetweenPhases;

        #endregion

        #region Расстояние между расщеплёнными проводами [A_splitwires]

        /// <summary>
        /// Расстояние между расщеплёнными проводами [A_splitwires].
        /// </summary>
        public double DistanceBetweenSplitWires
        {
            get { return _distanceBetweenSplitWires; }
            set { Set(ref _distanceBetweenSplitWires, value); }
        }

        private double _distanceBetweenSplitWires;

        #endregion

        #region Диаметр провода [D_wire]

        /// <summary>
        /// Диаметр провода [D_wire].
        /// </summary>
        public double DiameterWire
        {
            get { return _diameterWire; }
            set { Set(ref _diameterWire, value); }
        }

        private double _diameterWire;

        #endregion

        #region Сечение стали [F_st]

        /// <summary>
        /// Сечение стали [F_st].
        /// </summary>
        public double SteelSection
        {
            get { return _steelSection; }
            set { Set(ref _steelSection, value); }
        }

        private double _steelSection;

        #endregion

        #region Сечение алюминия [F_al]

        /// <summary>
        /// Сечение алюминия [F_al].
        /// </summary>
        public double AluminumSection
        {
            get { return _aluminumSection; }
            set { Set(ref _aluminumSection, value); }
        }

        private double _aluminumSection;

        #endregion

        #region Коэффициент 1

        /// <summary>
        /// Коэффициент 1.
        /// </summary>
        public double K1
        {
            get { return _k1; }
            set { Set(ref _k1, value); }
        }

        private double _k1;

        #endregion

        #region Коэффициент 2

        /// <summary>
        /// Коэффициент 2.
        /// </summary>
        public double K2
        {
            get { return _k2; }
            set { Set(ref _k2, value); }
        }

        private double _k2;

        #endregion

        #endregion

        #region Волновые параметры одиночного провода

        /// <summary>
        /// Средне-геометрическое расстояние между проводами фаз одиночного провода [Dsr].
        /// </summary>
        internal double MeanDistanceBetweenPhaseWires_Single =>
            Math.Pow(2, 1 / (double)3) * DistanceBetweenPhases;

        /// <summary>
        /// Радиус одиночного провода [R_pr].
        /// </summary>
        internal double RadiusWire_Single =>
            Math.Sqrt(3 * (SteelSection + AluminumSection) / Math.PI);

        /// <summary>
        /// Сопротивление одиночного провода [X0_single].
        /// </summary>
        internal double ResistanceWire_Single =>
            0.144 * Math.Log10(MeanDistanceBetweenPhaseWires_Single * 1000 / RadiusWire_Single + 0.0157);

        /// <summary>
        /// Емкостная проводимость одиночного провода [B0_single].
        /// </summary>
        internal double CapacitiveConductivityWire_Single =>
            7.58 / Math.Log10(MeanDistanceBetweenPhaseWires_Single * 1000 / RadiusWire_Single) * Math.Pow(10, -6);

        /// <summary>
        /// Комплексное сопротивление одиночного провода [Z0_single].
        /// </summary>
        internal Complex ComplexResistanceWire_Single =>
            new((double)ActiveResistance, ResistanceWire_Single);

        /// <summary>
        /// Комплексная проводимость одиночного провода [Y0_single].
        /// </summary>
        internal Complex ComplexConductivityWire_Single =>
            new(0, CapacitiveConductivityWire_Single);

        /// <summary>
        /// Коэффициент распространения волны одиночного провода [Gamma0_single].
        /// </summary>
        internal Complex WavePropagationCoefficient_Single =>
            Complex.Sqrt(Complex.Multiply(ComplexResistanceWire_Single, ComplexConductivityWire_Single));

        /// <summary>
        /// Коэффициент затухания одиночного провода [Alfa0_single].
        /// </summary>
        internal double AttenuatioCoefficient_Single =>
            WavePropagationCoefficient_Single.Real;

        /// <summary>
        /// Коэффициент фазы одиночного провода (rad) [Beta0_rad_single].
        /// </summary>
        internal double PhaseCoefficient_Single =>
            WavePropagationCoefficient_Single.Magnitude * Math.Sin(WavePropagationCoefficient_Single.Phase);

        /// <summary>
        /// Коэффициент фазы одиночного провода (deg) [Beta0_deg_single].
        /// </summary>
        internal double PhaseCoefficientDeg_Single =>
            PhaseCoefficient_Single * 180 / Math.PI;

        /// <summary>
        /// Волновое сопротивление одиночного провода [Zc_single].
        /// </summary>
        internal Complex WaveResistanceLine_Single =>
            Complex.Sqrt(Complex.Divide(ComplexResistanceWire_Single, ComplexConductivityWire_Single));

        /// <summary>
        /// Натуральная мощность одиночного провода [Pc_single].
        /// </summary>
        internal double NaturalPower_Single =>
            Math.Pow(NominalVoltage, 2) / WaveResistanceLine_Single.Magnitude;

        #endregion

        #region Волновые параметры расщеплённого провода

        /// <summary>
        /// Средне-геометрическое расстояние между проводами фаз расщеплённого провода [Dsr_].
        /// </summary>
        internal double MeanDistanceBetweenPhaseWires =>
            double.NaN;

        /// <summary>
        /// Радиус расщеплённого провода [R_eq].
        /// </summary>
        internal double RadiusWire =>
            Math.Pow((double)DiameterWire / 20 * (double)DistanceBetweenSplitWires * (double)DistanceBetweenSplitWires, 1.0 / 3.0);

        /// <summary>
        /// Сопротивление расщеплённого провода [X0].
        /// </summary>
        internal double ResistanceWire =>
             0.144 * Math.Log10(MeanDistanceBetweenPhaseWires_Single * 100 / RadiusWire) + 0.0157 / NumberSplitWires;

        /// <summary>
        /// Емкостная проводимость расщеплённого провода [B0].
        /// </summary>
        internal double CapacitiveConductivityWire =>
            7.58 / Math.Log10(MeanDistanceBetweenPhaseWires_Single * 100 / RadiusWire) * Math.Pow(10, -6);

        /// <summary>
        /// Комплексное сопротивление расщеплённого провода [Z0].
        /// </summary>
        internal Complex ComplexResistanceWire =>
            new(ActiveResistance, ResistanceWire);

        /// <summary>
        /// Комплексная проводимость расщеплённого провода [Y0].
        /// </summary>
        internal Complex ComplexConductivityWire =>
            new(0, CapacitiveConductivityWire);

        /// <summary>
        /// Коэффициент распространения волны расщеплённого провода [Gamma0].
        /// </summary>
        internal Complex WavePropagationCoefficient =>
            Complex.Sqrt(Complex.Multiply(ComplexResistanceWire, ComplexConductivityWire));

        /// <summary>
        /// Коэффициент затухания расщеплённого провода [Alfa0].
        /// </summary>
        internal double AttenuatioCoefficient =>
            WavePropagationCoefficient.Real;

        /// <summary>
        /// Коэффициент фазы (rad) расщеплённого провода [Beta0_rad].
        /// </summary>
        internal double PhaseCoefficient =>
            WavePropagationCoefficient.Magnitude * Math.Sin(WavePropagationCoefficient.Phase);

        /// <summary>
        /// Коэффициент фазы (deg) расщеплённого провода [Beta0_deg].
        /// </summary>
        internal double PhaseCoefficientDeg =>
            PhaseCoefficient * 180 / Math.PI;

        /// <summary>
        /// Волновое сопротивление расщеплённого провода [Zc].
        /// </summary>
        internal Complex WaveResistanceLine =>
            Complex.Sqrt(Complex.Divide(ComplexResistanceWire, ComplexConductivityWire));

        /// <summary>
        /// Натуральная мощность расщеплённого провода [Pc].
        /// </summary>
        internal double NaturalPower =>
            Math.Pow(NominalVoltage, 2) / WaveResistanceLine.Magnitude;

        #endregion

        public override string ToString()
        {
            return
                $"Номинальное напряжение                                {NominalVoltage + Environment.NewLine}" +
                $"Длина линии                                           {LineLength + Environment.NewLine}" +
                $"Количество расщеплённых проводов                      {NumberSplitWires + Environment.NewLine}" +
                $"Активное сопротивление                                {ActiveResistance + Environment.NewLine}" +
                $"Расстояние между фазами                               {DistanceBetweenPhases + Environment.NewLine}" +
                $"Расстояние между расщеплёнными проводами              {DistanceBetweenSplitWires + Environment.NewLine}" +
                $"Диаметр провода                                       {DiameterWire + Environment.NewLine}" +
                $"Сечение стали                                         {SteelSection + Environment.NewLine}" +
                $"Сечение алюминия                                      {AluminumSection + Environment.NewLine}" +
                $"Коэффициент 1                                         {K1 + Environment.NewLine}" +
                $"Коэффициент 2                                         {K2 + Environment.NewLine}" +
                $"Одиночный провод                                      ============================" + Environment.NewLine +
                $"Средне-геометрическое расстояние между проводами фаз  {MeanDistanceBetweenPhaseWires_Single + Environment.NewLine}" +
                $"Радиус провода                                        {RadiusWire_Single + Environment.NewLine}" +
                $"Сопротивление провода                                 {ResistanceWire_Single + Environment.NewLine}" +
                $"Емкостная проводимость провода                        {CapacitiveConductivityWire_Single + Environment.NewLine}" +
                $"Комплексное сопротивление провода                     {ComplexResistanceWire_Single + Environment.NewLine}" +
                $"Комплексная проводимость провода                      {ComplexConductivityWire_Single + Environment.NewLine}" +
                $"Коэффициент распространения волны                     {WavePropagationCoefficient_Single + Environment.NewLine}" +
                $"Коэффициент затухания                                 {AttenuatioCoefficient_Single + Environment.NewLine}" +
                $"Коэффициент фазы                                      {PhaseCoefficientDeg_Single + Environment.NewLine}" +
                $"Волновое сопротивление                                {WaveResistanceLine_Single.Magnitude + Environment.NewLine}" +
                $"Натуральная мощность                                  {NaturalPower_Single + Environment.NewLine}" +
                $"Расщеплённый провод                                   ============================" + Environment.NewLine +
                $"Средне-геометрическое расстояние между проводами фаз  {MeanDistanceBetweenPhaseWires + Environment.NewLine}" +
                $"Радиус провода                                        {RadiusWire + Environment.NewLine}" +
                $"Сопротивление провода                                 {ResistanceWire + Environment.NewLine}" +
                $"Емкостная проводимость провода                        {CapacitiveConductivityWire + Environment.NewLine}" +
                $"Комплексное сопротивление провода                     {ComplexResistanceWire + Environment.NewLine}" +
                $"Комплексная проводимость провода                      {ComplexConductivityWire + Environment.NewLine}" +
                $"Коэффициент распространения волны                     {WavePropagationCoefficient + Environment.NewLine}" +
                $"Коэффициент затухания                                 {AttenuatioCoefficient + Environment.NewLine}" +
                $"Коэффициент фазы                                      {PhaseCoefficientDeg + Environment.NewLine}" +
                $"Волновое сопротивление                                {WaveResistanceLine.Magnitude + Environment.NewLine}" +
                $"Натуральная мощность                                  {NaturalPower + Environment.NewLine}";
        }
    }
}
