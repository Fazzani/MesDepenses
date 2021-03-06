﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.TvShows;
using XBMCPluginData.Helpers;
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
    [DataMember]
    public bool Is_playable { get; set; }
    [DataMember]
    public Properties Properties { get; set; }

    /// <summary>
    /// CompleteInfo tvShow
    /// </summary>
    public void CompleteInfo(TvShow tvShow, int saison, int episode)
    {
      Info = new InfoTvShow
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
        Year = Tools.TryGetValue(() => tvShow.FirstAirDate.Value.Year),
        Rating = (float)tvShow.Popularity,
        Code = tvShow.ExternalIds != null ? tvShow.ExternalIds.ImdbId : "0",
      };
    }

    /// <summary>
    /// CompleteInfo tvShow
    /// </summary>
    public void CompleteInfo(TvEpisode tvEpisode)
    {
      if (!string.IsNullOrEmpty(tvEpisode.Name))
          Info = new InfoTvShow
        {
          Tvshowtitle = tvEpisode.Name,
          Season = Tools.TryGetValue(() => tvEpisode.SeasonNumber.Value),
          Episode = tvEpisode.EpisodeNumber,
          Plot = tvEpisode.Overview,
          Plotoutline = tvEpisode.Overview,
          Title = tvEpisode.Name,
          Votes = Tools.TryGetValue(() => tvEpisode.VoteCount.ToString()),
          Year = Tools.TryGetValue(() => tvEpisode.AirDate.Year),
          Rating = (float)tvEpisode.VoteAverage,
          Code = tvEpisode.ExternalIds != null ? tvEpisode.ExternalIds.ImdbId : "0",
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
        Votes = Tools.TryGetValue(() => movie.VoteAverage.ToString()),
        Status = movie.Status,
        Year = Tools.TryGetValue(() => movie.ReleaseDate.Value.Year),
        Genre = Tools.TryGetValue(() => movie.Genres.Select(x => x.Name).Aggregate((x, y) => string.Format("{0} {1}", x, y))),
        Trailer = GetValidTailers(movie.Trailers),
        Tagline = movie.Tagline,
        Studio = Tools.TryGetValue(() => movie.ProductionCompanies.Select(x => x.Name).Aggregate((x, y) => string.Format("{0} {1}", x, y))),
        Rating = Tools.TryGetValue(() => (float)movie.Popularity),
        Code = movie.ImdbId,
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