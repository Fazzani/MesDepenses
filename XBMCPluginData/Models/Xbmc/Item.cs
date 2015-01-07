using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.Ajax.Utilities;
using TMDbLib.Objects.Movies;
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
        /// CompleteInfo tvShow
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

        /// <summary>
        /// CompleteInfo movie
        /// </summary>
        public void CompleteInfo(Movie movie)
        {
            if (Info == null)
                Info = new Info();
            Info.InfoLabels = new InfoMovie
            {
                Plot = movie.Overview,
                Plotoutline = movie.Overview,
                Title = movie.Title,
                Originaltitle = movie.OriginalTitle,
                Votes = movie.VoteAverage.ToString(),
                Status = movie.Status,
                Tvshowtitle = movie.Title,
                Year = movie.ReleaseDate.Value.Year,
                Genre = movie.Genres.Select(x => x.Name).Aggregate((x, y) => string.Format("{0} {1}", x, y)),
                Trailer = GetValidTailers(movie.Trailers),
                Tagline = movie.Tagline,
                Studio = movie.ProductionCompanies.Select(x => x.Name).Aggregate((x, y) => string.Format("{0} {1}", x, y))
            };
        }

        /// <summary>
        /// Get valid tailers
        /// </summary>
        /// <param name="trailers"></param>
        /// <returns></returns>
        private string GetValidTailers(Trailers trailers)
        {
            if (trailers != null)
            {
                if (trailers.Youtube != null)
                    return trailers.Youtube.FirstOrDefault().Source;
            }
            return string.Empty;
        }
    }
}