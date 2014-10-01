using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = new[]{
                "Get/3","put/test/25","Get/mm","user/22/page/5"
            };

            foreach (var s in t)
            {
                Console.WriteLine(string.Format("traitement de la chaine {0}", s));
                Console.WriteLine(Common.GetPatternUrl(s));
            }
            Console.ReadKey();
        }
    }
}
