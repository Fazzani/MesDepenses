using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MesDepenses.Models
{
    public class AsyncTestModel
    {
        public Dictionary<string, bool> DataDictionary { get; set; }
        public string ElapsedTime { get; set; }
        public int Count { get; set; }
        public int RealCount { get; set; }
        public string ErrorMessage { get; set; }
    }
}