using DistanceLine.Infrastructure.Base;
using System;
using System.Windows.Data;

namespace DistanceLine.Models
{
    /// <summary>
    /// Оболочка для данных.
    /// </summary>
    public class DataShell : ViewModel
    {
        /// <summary>
        /// Заголовок.
        /// </summary>
        public string Header
        {
            get { return _header; }
            set { Set(ref _header, value); }
        }

        private string _header;

        /// <summary>
        /// Значение первое.
        /// </summary>
        public object ValueOne
        {
            get { return _valuesOne; }
            set { Set(ref _valuesOne, value); }
        }

        private object _valuesOne;

        /// <summary>
        /// Значение второе.
        /// </summary>
        public object ValueTwo
        {
            get { return _valuesTwo; }
            set { Set(ref _valuesTwo, value); }
        }

        private object _valuesTwo;


        public DataShell(string header, object valueOne, object valueTwo)
        {
            Header = header;

            ValueOne = valueOne;
            ValueTwo = valueTwo;
        }

        public DataShell(string header, object valueOne)
        {
            Header = header;

            ValueOne = valueOne;
        }
    }
}
