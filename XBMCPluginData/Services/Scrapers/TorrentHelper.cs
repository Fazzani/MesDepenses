using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;


namespace XBMCPluginData.Services.Scrapers
{
    public class TorrentHelper
    {
        //Revenge S04E05 FASTSUB VOSTFR HDTV XviD
        private const string tvRegex = @"^(.*)S(?<season>\d{1,2})E(?<episode>\d{1,2})(.*)$";
        //10 bonnes raisons de te larguer FRENCH DVDRip XviD-NoTag
        private const string movieRegex = @"^(.*?)(dvdrip|xvid| cd[0-9]|dvdscr|brrip|divx|[\{\(\[]?[0-9]{4}).*$";
        /// <summary>
        /// Get Tv série Info From Torrent Name
        /// </summary>
        /// <param name="torrentName"></param>
        /// <returns></returns>
        public static InfoFromTorrentName GetTvInfoFromTorrentName(string torrentName)
        {
            Regex reg = new Regex(tvRegex);
            var match = reg.Match(torrentName);
            if (match.Success)
            {
                return new InfoFromTorrentName
                {
                    EpisodeNumber = Convert.ToInt32(match.Groups["episode"].Value),
                    Label = match.Groups[1].Value,
                    Quality = match.Groups["episode"].Value,
                    SaisonNumber = Convert.ToInt32(match.Groups["season"].Value)
                };
            }
            return new InfoFromTorrentName{Label = torrentName};
        }

        /// <summary>
        /// Get Tv série Info From Torrent Name
        /// </summary>
        /// <param name="torrentName"></param>
        /// <returns></returns>
        public static InfoFromTorrentName GetMovieInfoFromTorrentName(string torrentName)
        {
            Regex reg = new Regex(movieRegex);
            var match = reg.Match(torrentName);
            if (match.Success)
            {
                return new InfoFromTorrentName
                {
                    Label = match.Groups[1].Value,
                };
            }
            return new InfoFromTorrentName { Label = torrentName };
        }
    }

    public class InfoFromTorrentName
    {
        public string Label { get; set; }
        public int SaisonNumber { get; set; }
        public int EpisodeNumber { get; set; }
        public string Quality { get; set; }
    }

}