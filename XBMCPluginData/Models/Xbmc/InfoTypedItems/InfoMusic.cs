using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace XBMCPluginData.Models.Xbmc.InfoTypedItems
{
  [DataContract]
  public class InfoMusic : InfoMediaBase
  {
    /*
     *  tracknumber: integer (8)
duration: integer (245) - duration in seconds
year: integer (1998)
genre: string (Rock)
album: string (Pulse)
artist: string (Muse)
title: string (American Pie)
rating: string (3) - single character between 0 and 5
lyrics: string (On a dark desert highway...)
playcount: integer (2) - number of times this item has been played
lastplayed: string (%Y-%m-%d %h:%m:%s = 2009-04-05 23:16:04)
     */
    [DataMember]
    public int Tracknumber { get; set; }
    [DataMember]
    public int Duration { get; set; }
    [DataMember]
    public int Year { get; set; }
    [DataMember]
    public string Genre { get; set; }
    [DataMember]
    public string Album { get; set; }
    [DataMember]
    public string Artist { get; set; }
    [DataMember]
    public string Title { get; set; }
    [DataMember]
    public string Rating { get; set; }
    [DataMember]
    public string Lyrics { get; set; }
    [DataMember]
    public int Playcount { get; set; }
    [DataMember]
    public string Lastplayed { get; set; }
  }
}