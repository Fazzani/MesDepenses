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
    string _sources = "0";
    string _clients = "0";
    string isSaison = "0";
    [DataMember]
    public string Fanart_image { get; set; }
    [DataMember]
    public string Sources
    {
      get { return _sources; }
      set { _sources = value; }
    }
    [DataMember]
    public string Clients
    {
      get { return _clients; }
      set { _clients = value; }
    }
    [DataMember]
    public string IsSaison
    {
      get { return isSaison; }
      set { isSaison = value; }
    }
  }
}