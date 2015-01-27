using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication.VistorPattern;
using MesDepensesServices.DAL;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            //var t = new[]{
            //    "Get/3","put/test/25","Get/mm","user/22/page/5"
            //};

            //foreach (var s in t)
            //{
            //    Console.WriteLine(string.Format("traitement de la chaine {0}", s));
            //    Console.WriteLine(Common.GetPatternUrl(s));
            //}


            //using (var repository = new MesdepensesContext())
            //{
            //    var tmp = repository.Categories.Count();
            //    Console.WriteLine(tmp);
            //}
            var p = new ExpressionPrinter(Console.Out);
            Expression<Func<int, int>> e = i => (0 == i % 2) ? -i * i : Math.Abs(i);
            p.Print(e.Body);
            Console.ReadKey();
        }
    }
}
