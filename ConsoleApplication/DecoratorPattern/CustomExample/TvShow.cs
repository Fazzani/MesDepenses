using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.DecoratorPattern.CustomExample
{
    public class TvShow : Watchable
    {
        public int SaisonCount { get; set; }

        public override void Watch()
        {
            Console.WriteLine(string.Format("Watching TvShow {0}", Title));

        }

        public override void MarqueCommeVu()
        {
            IsWatched = true;
            Console.WriteLine(string.Format("This TvShow {0} est marqué comme vu", Title));
        }
    }
}
