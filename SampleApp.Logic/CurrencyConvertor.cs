using SampleApp.Model.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SampleApp.Logic
{
    public static class CurrencyConvertor
    {
        public static decimal ConvertCurrency(CurrencyModel model)
        {
            WebClient web = new WebClient();
            string url = string.Format("https://www.google.com/finance/converter?a={2}&from={0}&to={1}", model.From.ToUpper(), model.To.ToUpper(), model.Amount);
            string response = web.DownloadString(url);
            System.Threading.Thread.Sleep(3000);
            var split = response.Split((new string[] { "<span class=bld>" }), StringSplitOptions.None);
            var value = split[1].Split(' ')[0];
            decimal rate = decimal.Parse(value, CultureInfo.InvariantCulture);
            return rate;
        }
    }
}