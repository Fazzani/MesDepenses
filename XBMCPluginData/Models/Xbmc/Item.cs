using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XBMCPluginData.Models.Xbmc
{
    [DataContract]
    public class Item
    {
        public string Label { get; set; }
        public string Label2 { get; set; }
        public string IconImage { get; set; }
        public string ThumbnailImage { get; set; }
        public string Path { get; set; }
        [DataMember]
        public Info Info { get; set; }
        [DataMember]
        public Dictionary<string, string> Bag { get; set; }
    }
}