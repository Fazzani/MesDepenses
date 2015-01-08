using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
    public string Icon { get; set; }
    [DataMember]
    public string Thumbnail { get; set; }
    [DataMember]
    public string Path { get; set; }
    [DataMember]
    public Info Info { get; set; }
    //[DataMember]
    //public Dictionary<string, string> Bag { get; set; }
    [DataMember]
    public bool Is_playable { get; set; }
    [DataMember]
    public Properties Properties { get; set; }

    /// <summary>
    /// CompleteInfo tvShow
    /// </summary>
    public void CompleteInfo(TvShow tvShow, int saison, int episode)
    {
      Info = new InfoMovie
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
        Year = tvShow.FirstAirDate.Value.Year,
        Rating = (float)tvShow.Popularity,
        Code = tvShow.ExternalIds != null ? tvShow.ExternalIds.ImdbId : "0",
        
      };
    }

    /// <summary>
    /// CompleteInfo movie
    /// </summary>
    public void CompleteInfo(Movie movie)
    {
      Info = new InfoMovie
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
        Studio = movie.ProductionCompanies.Select(x => x.Name).Aggregate((x, y) => string.Format("{0} {1}", x, y)),
        Rating = (float)movie.Popularity,
        Code = movie.ImdbId,
        Duration = ""
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