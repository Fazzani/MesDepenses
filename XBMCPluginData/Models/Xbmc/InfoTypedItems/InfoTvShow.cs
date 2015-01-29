using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace XBMCPluginData.Models.Xbmc.InfoTypedItems
{
  public class InfoTvShow: InfoMovie
  {
    [DataMember]
    public int Episode { get; set; }
    [DataMember]
    public int Season { get; set; }
    [DataMember]
    public string Tvshowtitle { get; set; }
  }
}