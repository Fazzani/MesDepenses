using System.Collections.Generic;
using System.Runtime.Serialization;
using TMDbLib.Objects.TvShows;
using XBMCPluginData.Models.Xbmc.InfoTypedItems;

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

        /// <summary>
        /// 
        /// </summary>
        public void CompleteInfo(TvShow tvShow, int saison, int episode)
        {
            if (Info == null)
                Info = new Info();
            Info.InfoLabels = new InfoMovie
            {
                Season = saison,
                Episode = episode,
                Plot = tvShow.Overview,
                Plotoutline = tvShow.Overview,
                Title = tvShow.Name,
                Originaltitle = tvShow.OriginalName,
                Votes = tvShow.VoteAverage.ToString(),
                Status = tvShow.Status,
                Tvshowtitle = tvShow.Name,
                Year = tvShow.FirstAirDate.Value.Year
            };

        }
    }
}