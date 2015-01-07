using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace XBMCPluginData.Models.Xbmc
{
  [DataContract]
  public class Properties
  {
    [DataMember]
    public string Fanart_image { get; set; }
  }
}