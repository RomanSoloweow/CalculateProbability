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
        public bool IsRuning
        {
            private set { isRuning = value; }
            get { return isRuning; }
        }
        private CancellationTokenSource cancelTokenSource;
        private CancellationToken token;

        public Calculator(int S, double EPS, double T0, double Tn, int N)
        {
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
            "Lambda(F) = " + Functions.Lambda.ToString() + ";\n" + Environment.NewLine +
            "Mu(Fv) = " + Functions.Mu.ToString() + ";\n" + Environment.NewLine +
            "EPS = " + EPS.ToString() + ";\n" + Environment.NewLine +
            "P(tn, to) = " + CurrentP.ToString() + ";\n";
            return text;
        }
        public void CancelCalculation()
        {
            if (cancelTokenSource != null)
                cancelTokenSource.Cancel();
        }
        private void StopCalculation()
        {
            IsRuning = false;
            cancelTokenSource.Cancel();
            if (OnProgress != null)
                OnProgress("Reset", 0);
        }
        public void RunCalculation()
        {
            try
            {
                cancelTokenSource = new CancellationTokenSource();
                token = cancelTokenSource.Token;
                double r = 0.0;
                CurrentP = 0.0;
                IsRuning = true;
                Task task = new Task(() =>
                {
                    do
                    {
                        Prepare();
                        for (int i = 0; i <= S; i++)
                        {
                            if (token.IsCancellationRequested)
                            {
                                StopCalculation();
                                return;
                            }
                            ConvolutionP(i);
                            if (OnProgress != null)
                                OnProgress("Iteration", i);
                            if (token.IsCancellationRequested)
                            {
                                StopCalculation();
                                return;
                            }
                            ConvolutionFv(i);
                            if (OnProgress != null)
                                OnProgress("Iteration", i);
                            if (token.IsCancellationRequested)
                            {
                                StopCalculation();
                                return;
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
                        if (OnProgress != null)
                            OnProgress("Reset", 0);
                    } while (r > EPS && N < N_MAX);
                    if (OnCalculation != null)
                        OnCalculation(CurrentP);
                    IsRuning = false;
                });
                task.Start();
            }
            catch (Exception ex)
            {
                IsRuning = false;
                if (OnError != null)
                    OnError(ex.Message);
            }
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
                double value = Functions.FunctionDF(ti);
                DFList.Add(value);
                value = Functions.FunctionDFv(ti);
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
                    double value = 1 - Functions.FunctionF(ti);
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
                    double value = Functions.FunctionFv(ti);
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
    }
}
