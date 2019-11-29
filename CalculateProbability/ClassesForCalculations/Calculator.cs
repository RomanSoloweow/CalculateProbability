using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CalculateProbability
{
    public class Calculator : IDisposable
    {
        private Functions functions;
        public delegate void MessageProgressDelegate(string message, int iteration);
        public delegate void MessageErrorDelegate(string message);
        public delegate void CaclulationDelegate(double value);
        public event MessageProgressDelegate OnProgress;
        public event MessageErrorDelegate OnError;
        public event CaclulationDelegate OnCalculation;
        private Dictionary<int, List<double>> PList = new Dictionary<int, List<double>>();
        private List<double> DFList = new List<double>();
        private Dictionary<int, List<double>> FvList = new Dictionary<int, List<double>>();
        private List<double> DFvList = new List<double>();
        private int S = 10;
        private double EPS = 10e-5;
        private double Tn = 1.0;
        private double T0 = 0.05;
        private int N = 40;
        private int N_MAX = 6000;
        public double CurrentP = 0.0;
        private bool isRuning = false;

        public string Error;
        public bool IsRuning
        {
            private set { isRuning = value; }
            get { return isRuning; }
        }
        private CancellationTokenSource cancelTokenSource;
        private CancellationToken token;

        public Calculator(double F, double Fv, int S, double EPS, double T0, double Tn, int N)
        {
            functions = new Functions(F,Fv);
            this.S = S;
            this.EPS = EPS;
            this.Tn = Tn;
            this.T0 = T0;
            this.N = N;
            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;
        }
        public string SaveToText()
        {
            string text = "Tn = " + Tn.ToString() + ";\n" + Environment.NewLine +
            "To = " + T0.ToString() + ";\n" + Environment.NewLine +
            "S = " + S.ToString() + ";\n" + Environment.NewLine +
            "Lambda(F) = " + functions.Lambda.ToString() + ";\n" + Environment.NewLine +
            "Mu(Fv) = " + functions.Mu.ToString() + ";\n" + Environment.NewLine +
            "EPS = " + EPS.ToString() + ";\n" + Environment.NewLine +
            "P(tn, to) = " + CurrentP.ToString() + ";\n";
            return text;
        }
        public void StopCalculation()
        {
            if ((IsRuning) && (cancelTokenSource != null))
                cancelTokenSource.Cancel();
        }
        private void StopingCalculation()
        {
            IsRuning = false;
            cancelTokenSource.Cancel();
            if (OnProgress != null)
                OnProgress("Reset", 0);
        }
        private void Progress(string str, int i)
        {
            if (OnProgress != null)
                OnProgress(str, i);
        }
        public double RunCalculation()
        {
            try
            {
                cancelTokenSource = new CancellationTokenSource();
                token = cancelTokenSource.Token;
                double r = 0.0;
                CurrentP = 0.0;
                IsRuning = true;
                    do
                    {
                        Prepare();
                        for (int i = 0; i <= S; i++)
                        {
                            if (token.IsCancellationRequested)
                            {
                                StopingCalculation();
                                 break;
                            }
                            ConvolutionP(i);
                            Progress("Iteration", i);
                            if (token.IsCancellationRequested)
                            {
                                StopingCalculation();
                                  break;
                            }
                            ConvolutionFv(i);
                            Progress("Iteration", i);
                            if (token.IsCancellationRequested)
                            {
                                StopingCalculation();
                                  break;
                            }
                        }
                        double result = 0.0;
                        for (int k = 0; k <= S; k++)
                        {
                            result += PList[k][N] * FvList[k][N];
                        }
                        r = Math.Abs(CurrentP - result) / Math.Abs(result);
                        N *= 2;
                        CurrentP = result;
                    } while (r > EPS && N < N_MAX);
                    if (OnCalculation != null)
                        OnCalculation(CurrentP);
                    IsRuning = false;
            }
            catch (Exception ex)
            {
                IsRuning = false;
                Error = ex.Message;
            }
            return CurrentP;
        }
        private void Prepare()
        {
            PList.Clear();
            DFList.Clear();
            FvList.Clear();
            DFvList.Clear();
            double h = Tn / N;
            for (int i = 0; i <= N; i++) //по всем узлам сетки
            {
                double ti = i * h;
                double value = functions.FunctionDF(ti);
                DFList.Add(value);
                value = functions.FunctionDFv(ti);
                DFvList.Add(value);
            }
        }
        private void ConvolutionP(int ind)
        {
            PList.Add(ind, new List<double>());
            double h = Tn / N;
            if (ind == 0)
            {
                for (int i = 0; i <= N; i++) //по всем узлам сетки
                {
                    double ti = i * h;
                    double value = 1 - functions.FunctionF(ti);
                    PList[0].Add(value);
                }
            }
            else
            {
                for (int i = 0; i <= N; i++) //по всем узлам сетки
                {
                    double ti = i * h;
                    double value = 0.0;
                    for (int k = 0; k <= i; k++)
                    {
                        double omega = ((k + 1) % (i + 1) == 0 ? 0.5 : 1.0);
                        value += PList[ind - 1][i - k] * DFList[k] * omega * h;
                    }
                    PList[ind].Add(value);
                }
            }
        }
        private void ConvolutionFv(int ind)
        {
            FvList.Add(ind, new List<double>());
            double h = T0 / N;
            if (ind == 0)
            {
                for (int i = 0; i <= N; i++) //по всем узлам сетки
                {
                    double ti = i * h;
                    double value = functions.FunctionFv(ti);
                    FvList[0].Add(value);
                }
            }
            else
            {
                for (int i = 0; i <= N; i++) //по всем узлам сетки
                {
                    double ti = i * h;
                    double value = 0.0;
                    for (int k = 0; k <= i; k++)
                    {
                        double omega = ((k + 1) % (i + 1) == 0 ? 0.5 : 1.0);
                        value += FvList[ind - 1][i - k] * DFvList[k] * omega * h;
                    }
                    FvList[ind].Add(value);
                }
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #region IntegralMethods
        public double CalculatePkDF(int k, double Tbeg, double Tend, double eps, int N)
        {
            double integral = 0.0;
            double current_integral = 0.0;
            double h = (Tend - Tbeg) / N;
            if (k == 1)
                current_integral += (1.0 - functions.FunctionF(Tend - Tbeg)) * functions.FunctionDF(Tbeg) +
                (1.0 - functions.FunctionF(Tend - Tend)) * functions.FunctionDF(Tend);
            else
                current_integral += CalculatePkDF(k - 1, Tbeg, Tend, eps, N) + CalculatePkDF(k - 1, Tbeg, Tend,
                eps, N);
            for (int i = 1; i <= N - 1; i++)
            {
                double ti = Tbeg + h * i;
                double fi = 0.0;
                if (k == 1)
                    fi = (1.0 - functions.FunctionF(Tend - ti)) * functions.FunctionDF(ti);
                else
                    fi = CalculatePkDF(k - 1, Tbeg, Tend - ti, eps, N);
                if (i % 2 == 0)
                    fi *= 2;
                else
                    fi *= 4;
                current_integral += fi;
            }
            current_integral = current_integral * h / 3.0;
            integral = current_integral;
            return integral;
        }
        public double CalculateFvkDFv(int k, double Tbeg, double Tend, double eps, int N)
        {
            double integral = 0.0;
            double current_integral = 0.0;
            double h = (Tend - Tbeg) / N;
            if (k == 1)
                current_integral += functions.FunctionFv(Tbeg) * functions.FunctionDFv(Tbeg) +
                functions.FunctionFv(Tend - Tend) * functions.FunctionDFv(Tend);
            else
                current_integral += CalculateFvkDFv(k - 1, Tbeg, Tend, eps, N) + CalculateFvkDFv(k - 1, Tbeg,
                Tend, eps, N);
            for (int i = 1; i <= N - 1; i++)
            {
                double ti = Tbeg + h * i;
                double fi = 0.0;
                if (k == 1)
                    fi = (1.0 - functions.FunctionFv(Tend - ti)) * functions.FunctionDFv(ti);
                else
                    fi = CalculateFvkDFv(k - 1, Tbeg, Tend - ti, eps, N);
                if (i % 2 == 0) fi *= 2;
                else fi *= 4;
                current_integral += fi;
            }
            current_integral = current_integral * h / 3.0;
            integral = current_integral;
            return integral;
        }
        #endregion IntegralMethods
    }
}
