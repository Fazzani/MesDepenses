using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication.VistorPattern;
using MesDepensesServices.DAL;
using ConsoleApplication.DecoratorPattern;
using ConsoleApplication.DecoratorPattern.CustomExample;

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
            CreatingAnExpressionFromAnotherExpression();

            //TestCustomDecoratorPattern();
            //var p = new ExpressionPrinter(Console.Out);
            //Expression<Func<int, int>> e = i => (0 == i % 2) ? -i * i : Math.Abs(i);
            //p.Print(e.Body);
            Console.ReadKey();
        }

        public static void TestDecoratorPattern()
        {
            // Create book
            Book book = new Book("Worley", "Inside ASP.NET", 10);
            book.Display();

            // Create video
            Video video = new Video("Spielberg", "Jaws", 23, 92);
            video.Display();

            // Make video borrowable, then borrow and display
            Console.WriteLine("\nMaking video borrowable:");

            Borrowable borrowvideo = new Borrowable(video);
            borrowvideo.BorrowItem("Customer #1");
            borrowvideo.BorrowItem("Customer #2");

            borrowvideo.Display();

            // Wait for user
            Console.ReadKey();
        }


        public static void TestCustomDecoratorPattern()
        {
            TvShow tvShowMentalist = new TvShow { Title = "Mentalist", SaisonCount = 6 };
            tvShowMentalist.Watch();

            Movie movieGreenMile = new Movie { Title = "The green mile" };
            movieGreenMile.Watch();
            movieGreenMile.MarqueCommeVu();

            // Make video borrowable, then borrow and display
            Console.WriteLine("\nMaking video Dowloading:");

            DownloadDecorator dowloadedVideo = new DownloadDecorator(movieGreenMile);
            dowloadedVideo.Download("http://linkMovie");
            Console.WriteLine(string.Format("{0} is downloaded statut {1}", dowloadedVideo.Title, dowloadedVideo.IsDowloaded));
            // Wait for user
            Console.ReadKey();
        }

        public static void CreatingAnExpressionFromAnotherExpression()
        {
            Expression<Func<int, int>> square = x => x * x;

            BinaryExpression spuareMoins5 = Expression.Add(square.Body, Expression.Negate(Expression.Constant(5)));
            BinaryExpression squareplus2 = Expression.Add(square.Body, Expression.Constant(2));
            ConditionalExpression codExpression = Expression.Condition(Expression.IsTrue(Expression.GreaterThan(square.Parameters.FirstOrDefault(), Expression.Constant(10))), spuareMoins5, squareplus2);
            Expression<Func<int, int>> expr = Expression.Lambda<Func<int, int>>(codExpression, square.Parameters);

            Func<int, int> compile = expr.Compile();
            Console.WriteLine(compile(10));//102
            Console.WriteLine(compile(11));//116
        }
    }
}
