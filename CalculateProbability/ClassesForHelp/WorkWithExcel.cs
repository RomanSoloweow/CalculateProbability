using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateProbability
{
    public static class WorkWithExcel
    {
        public static void Open()
        {
            using (var stream = File.Open(@"Книга1.csv",FileMode.OpenOrCreate))
            {

            }
        }

    }
}
