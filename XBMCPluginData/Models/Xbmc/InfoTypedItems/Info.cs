using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace XBMCPluginData.Models.Xbmc.InfoTypedItems
{
  [DataContract]
  public class Info
  {
    DateTime _date = DateTime.Now;
    /*
     *  count: integer (12) - can be used to store an id for later, or for sorting purposes
size: long (1024) - size in bytes
date: string (%d.%m.%Y / 01.01.2009) - file date
     */
    [DataMember]
    public int Count { get; set; }
    [DataMember]
    public long Size { get; set; }
    [DataMember]
    public string Date
    {
      get
      {
        return _date.ToString("dd.MM.yyyy");
      }
      set
      {
        _date = DateTime.Parse(value);
      }
    }
  }
}