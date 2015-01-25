using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace XBMCPluginData.Models.Xbmc.InfoTypedItems
{
  public class InfoTvShow: InfoMovie
  {
    /*
       * genre: string (Comedy)
  year: integer (2009)
  episode: integer (4)
  season: integer (1)
  top250: integer (192)
  tracknumber: integer (3)
  rating: float (6.4) - range is 0..10
  playcount: integer (2) - number of times this item has been played
  overlay: integer (2) - range is 0..8.  See GUIListItem.h for values
cast: list (Michal C. Hall)
castandrole: list (Michael C. Hall|Dexter)
  director: string (Dagur Kari)
  mpaa: string (PG-13)
  plot: string (Long Description)
  plotoutline: string (Short Description)
  title: string (Big Fan)
  originaltitle: string (Big Fan)
  duration: string - duration in minutes (95)
  studio: string (Warner Bros.)
  tagline: string (An awesome movie) - short description of movie
  writer: string (Robert D. Siegel)
  tvshowtitle: string (Heroes)
  premiered: string (2005-03-04)
  status: string (Continuing) - status of a TVshow
  code: string (tt0110293) - IMDb code
  aired: string (2008-12-07)
  credits: string (Andy Kaufman) - writing credits
  lastplayed: string (%Y-%m-%d %h:%m:%s = 2009-04-05 23:16:04)
  album: string (The Joshua Tree)
  votes: string (12345 votes)
  trailer: string (/home/user/trailer.avi)
       */

    [DataMember]
    public int Episode { get; set; }
    [DataMember]
    public int Season { get; set; }
    [DataMember]
    public string Tvshowtitle { get; set; }
  }
}