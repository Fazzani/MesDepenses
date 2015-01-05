using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XBMCPluginData.Models.Xbmc
{
  [DataContract]
  public class Item
  {
    [DataMember]
    public string Label { get; set; }
    [DataMember]
    public string Label2 { get; set; }
    [DataMember]
    public string IconImage { get; set; }
    [DataMember]
    public string ThumbnailImage { get; set; }
    [DataMember]
    public string Path { get; set; }
    [DataMember]
    public Info Info { get; set; }
    [DataMember]
    public Dictionary<string, string> Bag { get; set; }
  }
}