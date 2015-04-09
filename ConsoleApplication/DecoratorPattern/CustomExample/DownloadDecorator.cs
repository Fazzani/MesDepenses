using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication.DecoratorPattern.CustomExample
{
    public class DownloadDecorator : WatchableDecorator
    {
        public bool IsDowloaded { get; set; }
        public DownloadDecorator(Watchable watchable)
            : base(watchable)
        {

        }

        public void Download(string link)
        {
            Console.WriteLine("Downloading {0} from {1}", Watchable.Title, link);
            IsDowloaded = true;
        }
    }
}
