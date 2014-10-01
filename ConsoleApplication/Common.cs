using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Server;
using Rx = System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    public class Common
    {
        /// <summary>
        /// Get Pattern of rest Url
        /// ex: /get/35/all => /get/{0}/all
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetPatternUrl(string url, char sep = '/', string wilcard="{0}")
        {
            foreach (var chaine in url.Split(sep))
            {
                int Num;
                if (int.TryParse(chaine, out Num))
                {
                    url = url.Replace(chaine, wilcard);
                }

            }
            //var qariRegex = new Rx.Regex(@"^([a-z]*/)*([0-9]*)/?(.*)", Rx.RegexOptions.IgnoreCase);
            //Rx.MatchCollection mc = qariRegex.Matches(url);
            //if (mc[0].Success && !string.IsNullOrEmpty( mc[0].Groups[2].ToString()))
            //    return url.Replace(mc[0].Groups[2].ToString(), "{0}");
            return url;
        }
    }
}
