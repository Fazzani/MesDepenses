using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.DecoratorPattern.CustomExample
{
    public class Movie : Watchable
    {
        public override void Watch()
        {
            Console.WriteLine("Watching Movie {0}", Title);
        }

        public override void MarqueCommeVu()
        {
            IsWatched = true;
            Console.WriteLine("This Movie {0} est marqué comme vu", Title);
        }
    }
}
