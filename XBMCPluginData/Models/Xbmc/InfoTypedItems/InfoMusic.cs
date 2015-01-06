using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XBMCPluginData.Models.Xbmc.InfoTypedItems
{
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
        public int Tracknumber { get; set; }
        public int Duration { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Rating { get; set; }
        public string Lyrics { get; set; }
        public int Playcount { get; set; }
        public string Lastplayed { get; set; }
    }
}