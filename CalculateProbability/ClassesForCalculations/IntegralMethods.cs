using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateProbability
{
    public static class IntegralMethods
    {
        public static double CalculatePkDF(int k, double Tbeg, double Tend, double eps, int N)
        {
            double integral = 0.0;
            double current_integral = 0.0;
            double h = (Tend - Tbeg) / N;
            if (k == 1)
                current_integral += (1.0 - Functions.FunctionF(Tend - Tbeg)) * Functions.FunctionDF(Tbeg) +
                (1.0 - Functions.FunctionF(Tend - Tend)) * Functions.FunctionDF(Tend);
            else
                current_integral += CalculatePkDF(k - 1, Tbeg, Tend, eps, N) + CalculatePkDF(k - 1, Tbeg, Tend,
                eps, N);
            for (int i = 1; i <= N - 1; i++)
            {
                double ti = Tbeg + h * i;
                double fi = 0.0;
                if (k == 1)
                    fi = (1.0 - Functions.FunctionF(Tend - ti)) * Functions.FunctionDF(ti);
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
        public static double CalculateFvkDFv(int k, double Tbeg, double Tend, double eps, int N)
        {
            double integral = 0.0;
            double current_integral = 0.0;
            double h = (Tend - Tbeg) / N;
            if (k == 1)
                current_integral += Functions.FunctionFv(Tbeg) * Functions.FunctionDFv(Tbeg) +
                Functions.FunctionFv(Tend - Tend) * Functions.FunctionDFv(Tend);
            else
                current_integral += CalculateFvkDFv(k - 1, Tbeg, Tend, eps, N) + CalculateFvkDFv(k - 1, Tbeg,
                Tend, eps, N);
            for (int i = 1; i <= N - 1; i++)
            {
                double ti = Tbeg + h * i;
                double fi = 0.0;
                if (k == 1)
                    fi = (1.0 - Functions.FunctionFv(Tend - ti)) * Functions.FunctionDFv(ti);
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
    }
}
