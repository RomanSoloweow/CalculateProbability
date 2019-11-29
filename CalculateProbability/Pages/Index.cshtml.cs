using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Web;
using Microsoft.AspNetCore;
using System.Threading;

namespace CalculateProbability.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public Calculate calculate = new Calculate();
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            GetCalculate();
        }
        public ActionResult OnPostGetData(string ParameterName,double From, double To, int CountDote, double Tn,double T0, int S, double F, double Fv, double Eps)
        {
            Dictionary<string, object> Data = new Dictionary<string, object>();
            string error = calculate.Set(ParameterName, From, To, CountDote, Tn, T0, S, F, Fv, Eps);
            if (!string.IsNullOrEmpty(error))
            {
                Data.Add("ErrorMessage", error);
            }
            else
            {
                 calculate.StartCalculate();  
                Data.Add("Names", new string[] { ParameterName, "P" });
                Data.Add("ParameterValues", calculate.ParameterValues.ToArray());
                Data.Add("P", calculate.P.ToArray());
                SaveCalculate();
            }

            var result = JsonConvert.SerializeObject(Data);
            return new JsonResult(result);
        }
        public void OnPostStopCalculate()
        {
            calculate.StopCalculate();
        }
        private void SaveCalculate()
        {
           HttpContext.Session.SetString("Calculate", JsonConvert.SerializeObject(calculate));
        }
        private void GetCalculate()
        {
            var calc = HttpContext.Session.GetString("Calculate");
            if (calc != null)
                calculate = JsonConvert.DeserializeObject<Calculate>(calc);
        }
    }
}
