using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateProbability
{
    public class Functions
    {
        public Functions(double F, double Fv)
        {
            Lambda = F;
            Mu = Fv;
        }
        public double Lambda;
        public double Mu;
        public double FunctionP(double x)
        {
            double v = 1 - FunctionF(x);
            return v;
        }
        public double FunctionF(double x)
        {
            double v = 1 - Math.Exp(-Lambda * x);
            return v;
        }
        public double FunctionDF(double x)
        {
            double v = Lambda * Math.Exp(-Lambda * x);
            return v;
        }
        public double FunctionFv(double x)
        {
            double v = 1 - Math.Exp(-Mu * x);
            return v;
        }
        public double FunctionDFv(double x)
        {
            double v = Mu * Math.Exp(-Mu * x);
            return v;
        }
    }
}
