using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CalculateProbability
{
    
    public class Calculate : IDisposable
    {
        public Calculate()
        {
            CalculationBW.WorkerReportsProgress = true;
            CalculationBW.WorkerSupportsCancellation = true;
            CalculationBW.DoWork += Calculation;
        }
        /// <summary>
        /// Выбранный параметр
        /// </summary>
        public string SelectedParameterName="Tn";
        /// <summary>
        /// Начальное значение этого параметра
        /// </summary>
        public double From = 20;
        /// <summary>
        /// Конечное значение этого параметра
        /// </summary>
        public double To=21;
        /// <summary>
        /// Количество точек этого параметра
        /// </summary>
        public int CountDots=1;
        public double Tn;
        public double T0=6;
        public int S=4;
        public double F=1;
        public double Fv=2.5;
        public double Eps = 0.01;
        public double[] P;
        public double[] ParameterValues;
        [NonSerialized]
        private BackgroundWorker CalculationBW = new BackgroundWorker();
        [NonSerialized]
        public CountdownEvent countdownEvent;
        public bool isCalculate = false;
        public void StartCalculate()
        {
            System.Diagnostics.Stopwatch myStopwatch = new System.Diagnostics.Stopwatch();
            myStopwatch.Start(); //запуск
            if (isCalculate)
                return;
            ParameterValues = new double[CountDots];
            P = new double[CountDots];
            CalculationBW.RunWorkerAsync();
            isCalculate = true;
            countdownEvent = new CountdownEvent(CountDots+1);
            countdownEvent.Wait();
            myStopwatch.Stop(); //остановить
            isCalculate = false;
        }
        private void Calculation(object sender, DoWorkEventArgs e)
        {
            double step = (To - From) / CountDots;
                double CurentValue = From - step;
                for (int i = 0; i < CountDots; i++)
                {
                    CurentValue += step;
                    SetForParametr(CurentValue);
                    ParameterValues[i] = CurentValue;
                ThreadPool.QueueUserWorkItem(new WaitCallback(Сomputation), new Parametr
                {
                    e = e,
                    Index = i,
                    Tn = this.Tn,
                    T0 = this.T0,
                    S = this.S,
                    F = this.F,
                    Fv = this.Fv,
                    Eps = this.Eps

                });
                }
            countdownEvent.Signal();
        }

        private void Сomputation(object state)
        {
            Parametr parametr = (Parametr)state;
            Calculator calculator =   new Calculator(parametr.F, parametr.Fv, parametr.S, parametr.Eps, parametr.T0, parametr.Tn, 100);
            if (CalculationBW.CancellationPending)
            {
                parametr.e.Cancel = true;
                return;
            }
            calculator.RunCalculation();
            P[parametr.Index] = calculator.CurrentP;
            countdownEvent.Signal();
        }

        public void StopCalculate()
        {
            if (!isCalculate)
                return;
        }
        public string Set(string _ParameterSelect, double _From, double _To, int _CountDots, double _Tn, double _T0, int _S, double _F, double _Fv, double _Eps)
        {
            string error = Check(_ParameterSelect, _From, _To, _CountDots, _Tn, _T0, _S, _F, _Fv, _Eps);
            if (!string.IsNullOrEmpty(error))
                return error;

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
            return "";
        }
        private string Check(string _ParameterSelect, double _From, double _To, int _CountDots, double _Tn, double _T0, int _S, double _F, double _Fv, double _Eps)
        {
            if (string.IsNullOrEmpty(_ParameterSelect))
                return "не выбран параметр";
            if (_From> _To)
                return "левая граница не может быть выше правой";
            if(_CountDots<1)
                return "количество точек должно быть больше 0";

            return "";
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

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
