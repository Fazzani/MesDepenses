using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Heni.VSXbmcViewLog
{
    class LogWatcherEventArgs
    {
        public string Contents;
        public LogWatcherEventArgs(string contents)
        {
            Contents = contents;
        }
    }
}
