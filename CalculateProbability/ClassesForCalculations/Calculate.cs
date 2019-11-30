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
        public string Error;
        public bool HasError
        {
            get
            {
                return !string.IsNullOrEmpty(Error);
            }
        }
        public void StartCalculate()
        {
            if (isCalculate)
            {
                Console.WriteLine("Расчет уже был запущен");
                return;
            }
            isCalculate = true;
            ParameterValues = new double[CountDots];
            P = new double[CountDots];
            countdownEvent = new CountdownEvent(CountDots + 1);       
            CalculationBW.RunWorkerAsync();         
            Console.WriteLine("Расчет запущен");
            countdownEvent.Wait();
            Console.WriteLine("Расчет закончен");
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
            if(this.HasError)
            {
                return;
            }
            calculator.RunCalculation();
            if(calculator.HasError)
            {
                this.Error = calculator.Error;
            }
            Console.WriteLine("Получено значение точки "+(parametr.Index+1).ToString());
            P[parametr.Index] = calculator.CurrentP;
            countdownEvent.Signal();
        }

        public void StopCalculate()
        {
            if (!isCalculate)
                return;
        }
        private void Set(string _ParameterSelect, double _From, double _To, int _CountDots, double _Tn, double _T0, int _S, double _F, double _Fv, double _Eps)
        {
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
        }
        public string CheckAndSet(string _ParameterSelect, double? _From, double? _To, int? _CountDots, double? _Tn, double? _T0, int? _S, double? _F, double? _Fv, double? _Eps)
        {

            if (string.IsNullOrEmpty(_ParameterSelect))
                return "не выбран параметр";

            if (!_From.HasValue)
                return "левая граница диапазона не указана или указана неверно";

            if (!_To.HasValue)
                return "правая граница диапазона не указана или указана неверно";

            if (!_CountDots.HasValue)
                return "количество точек не указано или указано неверно";


            if(_ParameterSelect=="Tn")
            {
                _Tn = 0;
            }
            else if(!_Tn.HasValue)
            {
                return "параметр Tn не указан или указан неверно";
            }

            if (_ParameterSelect == "T0")
            {
                _T0 = 0;
            }
            else if (!_T0.HasValue)
            {
                return "параметр T0 не указан или указан неверно";
            }

            if (_ParameterSelect == "S")
            {
                _S = 0;
            }
            else if (!_S.HasValue)
            {
                return "параметр S не указан или указан неверно";
            }

            if (_ParameterSelect == "F")
            {
                _F = 0;
            }
            else if (!_F.HasValue)
            {
                return "параметр F не указан или указан неверно";
            }

            if (_ParameterSelect == "Fv")
            {
                _Fv = 0;
            }
            else if (!_Fv.HasValue)
            {
                return "параметр Fv не указан или указан неверно";
            }


            if (!_Eps.HasValue)
            {
                return "параметр Eps не указан или указан неверно";
            }

            if (_From> _To)
                return "левая граница не может быть выше правой";
            if(_CountDots<1)
                return "количество точек должно быть больше 0";
            if (_Eps < 0)
                return "Eps должно быть больше 0";



            Set(_ParameterSelect, _From.Value, _To.Value, _CountDots.Value, _Tn.Value, _T0.Value, _S.Value, _F.Value, _Fv.Value, _Eps.Value);
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
