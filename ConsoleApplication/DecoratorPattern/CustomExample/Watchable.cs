using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.DecoratorPattern.CustomExample
{
    public abstract class Watchable
    {
        public String Title { get; set; }
        public bool IsWatched { get; set; }
        public abstract void Watch();
        public abstract void MarqueCommeVu();
    }
}
