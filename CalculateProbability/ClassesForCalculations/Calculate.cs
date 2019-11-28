using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;

namespace CalculateProbability
{
    
    public class Calculate
    {
        public Calculate()
        {
            CalculationBW.WorkerReportsProgress = true;
            CalculationBW.WorkerSupportsCancellation = true;
            CalculationBW.DoWork += Calculation;
        }
        public Random rnd = new Random();
        /// <summary>
        /// Выбранный параметр
        /// </summary>
        public string SelectedParameterName;
        /// <summary>
        /// Начальное значение этого параметра
        /// </summary>
        public double From=-5;
        /// <summary>
        /// Конечное значение этого параметра
        /// </summary>
        public double To=5;
        /// <summary>
        /// Количество точек этого параметра
        /// </summary>
        public int CountDots;
        public double Tn;
        public double T0;
        public int S;
        public double F;
        public double Fv;
        public double Eps;
        public double[] P;
        public double[] ParameterValues;
        private BackgroundWorker CalculationBW = new BackgroundWorker();
        public bool isCalculate = false;
        public void StartCalculate()
        {   
            if (isCalculate)
                return;
            ParameterValues = new double[CountDots];
            P = new double[CountDots];
            CalculationBW.RunWorkerAsync();
            Console.WriteLine("Нажмите Enter в течении следующих пяти секунд, чтобы прервать работу");
        }
        private void Calculation(object sender, DoWorkEventArgs e)
        {       
            double step = (To - From) / CountDots;
            double CurentValue = From - step;
            for (int i = 0; i< CountDots; i++)
            {
                CurentValue += step;                
                SetForParametr(CurentValue);
                ParameterValues[i]=CurentValue;
                if (CalculationBW.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                ThreadPool.QueueUserWorkItem(Сomputation, new Parametr 
                {
                    Index=i,
                    Tn = this.Tn,
                    T0 = this.To,
                    S = this.S,
                    F = this.F,
                    Fv = this.Fv,
                    Eps = this.Eps

                });
            }
        }

        private void Сomputation(object state)
        {
            Parametr parametr = (Parametr)state;
            Calculator calculator =   new Calculator(parametr.F, parametr.Fv, parametr.S, parametr.Eps, parametr.T0, parametr.Tn, 100);
            calculator.RunCalculation();
            P[parametr.Index] = calculator.CurrentP;
        }

        public void StopCalculate()
        {
            if (!isCalculate)
                return;
        }
        public bool Set(string _ParameterSelect, double _From, double _To, int _CountDots, double _Tn, double _T0, int _S, double _F, double _Fv, double _Eps)
        {
            if (!Check(_ParameterSelect, _From, _To, _CountDots, _Tn, _T0, _S, _F, _Fv, _Eps))
                return false;

            SelectedParameterName = _ParameterSelect;
            From = _From;
            To = _To;
            CountDots = _CountDots;
            Tn = _Tn;
            T0 = _T0;
            S = _S;
            F = _F;
            Fv = _Fv;
            Eps = _Eps;
            return true;
        }
        private bool Check(string _ParameterSelect, double _From, double _To, int _CountDots, double _Tn, double _T0, int _S, double _F, double _Fv, double _Eps)
        {
            if (string.IsNullOrEmpty(_ParameterSelect))
                return false;
            if (_From> _To)
                return false;
            if(_CountDots<1)
                return false;

            return true;
        }
        private void SetForParametr(double value)
        {
            switch(SelectedParameterName)
            {
                case "Tn":
                    {
                        Tn = value;
                        break;
                    }
                case "T0":
                    {
                        T0 = value;
                        break;
                    }
                case "S":
                    {
                        S = (int)value;
                        break;
                    }
                case "F":
                    {
                        F = value;
                        break;
                    }
                case "Fv":
                    {
                        Fv = value;
                        break;
                    }
            }
        }
        public Dictionary<string, double> GetParametersForSelect()
        {
            Dictionary<string, double> ParametersForSelect = new Dictionary<string, double>();
            ParametersForSelect.Add("Tn", Tn);
            ParametersForSelect.Add("T0", T0);
            ParametersForSelect.Add("S", S);
            ParametersForSelect.Add("F", F);
            ParametersForSelect.Add("Fv", Fv);
            return ParametersForSelect;
        }
        public Dictionary<string, double> GetParameters()
        {
            Dictionary<string, double> Parameters = new Dictionary<string, double>(GetParametersForSelect());
            Parameters.Add("Eps", Eps);
            return Parameters;
        }
    }
}
