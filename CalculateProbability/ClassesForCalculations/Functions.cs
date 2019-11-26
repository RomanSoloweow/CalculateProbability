using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateProbability
{
    public static class Functions
    {
        public static double Lambda;
        public static double Mu;
        public static double FunctionP(double x)
        {
            double v = 1 - FunctionF(x);
            return v;
        }
        public static double FunctionF(double x)
        {
            double v = 1 - Math.Exp(-Lambda * x);
            return v;
        }
        public static double FunctionDF(double x)
        {
            double v = Lambda * Math.Exp(-Lambda * x);
            return v;
        }
        public static double FunctionFv(double x)
        {
            double v = 1 - Math.Exp(-Mu * x);
            return v;
        }
        public static double FunctionDFv(double x)
        {
            double v = Mu * Math.Exp(-Mu * x);
            return v;
        }
    }
}
