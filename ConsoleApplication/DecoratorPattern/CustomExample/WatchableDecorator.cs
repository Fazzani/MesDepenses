using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.DecoratorPattern.CustomExample
{
    public abstract class WatchableDecorator : Watchable
    {
        protected Watchable Watchable { get; set; }

        protected WatchableDecorator(Watchable watchable)
        {
            Watchable = watchable;
        }

        public override void Watch()
        {
            Watchable.Watch();
        }

        public override void MarqueCommeVu()
        {
            Watchable.MarqueCommeVu();
        }
    }
}
