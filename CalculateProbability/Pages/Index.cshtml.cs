using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CalculateProbability.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private int[,] data = new int[3, 2] { { 1, 3 }, { 6, 2 }, { 8, 5 } };
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
        public ActionResult OnPostGetData(string ParametrName,double From, double To, double CountDote, double Tn,double T0, double S, double F, double Fv, double Eps )
        {
            var result = JsonConvert.SerializeObject(data);
            return new JsonResult(result);
        }
    }
}
