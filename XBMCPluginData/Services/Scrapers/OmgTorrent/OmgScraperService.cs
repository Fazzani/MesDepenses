using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using FluentSharp.CoreLib;
using HtmlAgilityPack;
using FluentSharp.HtmlAgilityPacK;
using XBMCPluginData.Models.Xbmc;
using XBMCPluginData.Models.Xbmc.InfoTypedItems;
using XBMCPluginData.Helpers;

namespace XBMCPluginData.Services.Scrapers.OmgTorrent
{
  public class OmgScraperService : AbstractScraper
  {
    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="url"></param>
    public OmgScraperService(string url)
      : base(url)
    {
    }

    /// <summary>
    /// Search
    /// </summary>
    /// <param name="query"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    public IEnumerable<Item> Search(string query, int page)
    {
      HtmlDocument doc = HtmlWeb.Load(BuildUrl(string.Empty, string.Empty, query, page, OrderByEnum.Nom));
      ParallelQuery<Item> items = null;
      try
      {
        items = doc.DocumentNode.SelectNodes("//table[@class='table_corps']/tr")
              .AsParallel()
              .Select(GetMediaItemRow)
              .Where(x => x != null);
      }
      catch
      {
      }
      try
      {
        if (items != null)
          return items.Union(doc.DocumentNode.SelectNodes("//div[@class='cadre']")
              .AsParallel()
              .Select(GetMediaItemSerieBlock)
              .Where(x => x != null));
        return doc.DocumentNode.SelectNodes("//div[@class='cadre']")
                .AsParallel()
                .Select(GetMediaItemSerieBlock)
                .Where(x => x != null);
      }
      catch
      {
      }

      return items;
    }

    /// <summary>
    /// Get Saison
    /// </summary>
    /// <param name="serieName"></param>
    /// <param name="serieId"></param>
    /// <param name="saisonNumber"></param>
    /// <returns></returns>
    public IEnumerable<Item> GetSaison(string serieName, int serieId, int saisonNumber)
    {
      HtmlDocument doc = HtmlWeb.Load(BuildUrl(string.Empty, string.Empty, string.Empty, 0, OrderByEnum.Nom, OrderEnum.Desc, serieName, serieId, saisonNumber));
      var saisonLink = Tools.TryGetValue(() => FullUrl(doc.DocumentNode.SelectSingleNode("//p[@class='serie_saison']").Element("a").attribute("href").Value));
      var torrentInfo = TorrentHelper.GetTorrentInfoAsync(saisonLink);
      var keyInfosTorrents = torrentInfo.Item2.Select(x => new KeyValuePair<string, InfoFromTorrentName>(x.Key, TorrentHelper.GetTvInfoFromTorrentName(x.Key, torrentInfo.Item1)));
      return doc.DocumentNode.SelectNodes("//table[@class='table_corps']/tr")
             .AsParallel()
             .Select((item, epIndex) => GetTvEpisodeItem(item, serieName, saisonNumber, epIndex, saisonLink, keyInfosTorrents))
             .Where(x => x != null);
    }

      /// <summary>
      /// Get Media Container Html bloc
      /// </summary>
      /// <param name="node"></param>
      /// <param name="serieName"></param>
      /// <param name="saisonNumber"></param>
      /// <param name="episodeNumber"></param>
      /// <param name="saisonLink"></param>
      /// <param name="keyInfosTorrents"></param>
      /// <returns></returns>
      private Item GetTvEpisodeItem(HtmlNode node, string serieName, int saisonNumber, int episodeNumber, string saisonLink, IEnumerable<KeyValuePair<string, InfoFromTorrentName>> keyInfosTorrents)
    {
      try
      {
        var torrentEpInfo = keyInfosTorrents.FirstOrDefault(x => x.Value.EpisodeNumber == episodeNumber);
        var item = new Item
        {
          Is_playable = true,
          Properties = new Properties { TorrentFileName = torrentEpInfo.Key, SaisonLabel=torrentEpInfo.Value.SaisonLabel },
          Label = node.SelectSingleNode(".//td[2]").InnerText
        };
        item.Label2 = item.Label;
        if (!string.IsNullOrEmpty(saisonLink))
        {
          item.Properties.IsSaison = "1";
          item.Path = saisonLink;
          item.Properties.TvShowName = serieName;
          item.Properties.SaisonNumber = saisonNumber.ToString();
        }
        else
          item.Path = Tools.TryGetValue(() => FullUrl(node.SelectSingleNode(".//td[4]").Element("a").attribute("href").Value));
        //int episodeNumber = Convert.ToInt32(node.SelectSingleNode(".//td[2]").InnerText);
        SetInfoTvSerie(item, serieName, saisonNumber, episodeNumber + 1);
        return item;
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
      }
      return null;
    }

    /// <summary>
    /// List Torrents
    /// </summary>
    /// <param name="filters"></param>
    /// <param name="category"></param>
    /// <param name="path"></param>
    /// <param name="page"></param>
    /// <param name="orderby"></param>
    /// <param name="order"></param>
    /// <param name="typeExtract"></param>
    /// <returns></returns>
    public IEnumerable<Item> ListTorrents(Dictionary<string, string> filters, string category = "", string path = "", int page = 1, OrderByEnum orderby = OrderByEnum.DateAjout, OrderEnum order = OrderEnum.Desc, TypeExtractEnum typeExtract = TypeExtractEnum.Raw)
    {
      if (filters != null && filters.Count > 0)
      {
        PostData = new System.Text.StringBuilder();
        foreach (var filter in filters)
          PostData.AppendFormat("{0}={1}&", filter.Value, filter.Key);
      }
      HtmlDocument doc = HtmlWeb.Load(BuildUrl(category, path, string.Empty, page, orderby, order), "POST");
      switch (typeExtract)
      {
        case TypeExtractEnum.Raw:
          return doc.DocumentNode.SelectNodes("//table[@class='table_corps']/tr")
              .AsParallel()
              .Select(GetMediaItemRow)
              .Where(x => x != null);

        case TypeExtractEnum.Container:
          return
              doc.DocumentNode.SelectNodes("//div[@class='cadre torrents_conteneur']")
                  .AsParallel()
                  .Select(GetMediaItemContainer)
                  .Where(x => x != null);
        case TypeExtractEnum.Block:
          return doc.DocumentNode.SelectNodes("//select/option")
              .Skip(1)
                   .AsParallel()
                   .Select(GetMediaItemBlock)
                   .Where(x => x != null);
        case TypeExtractEnum.SerieBlock:
          return doc.DocumentNode.SelectNodes("//div[@class='cadre']")
                   .AsParallel()
                   .Select(GetMediaItemSerieBlock)
                   .Where(x => x != null);
      }
      return null;
    }

    /// <summary>
    /// Build Site Url
    /// </summary>
    /// <param name="category"></param>
    /// <param name="path"></param>
    /// <param name="query"></param>
    /// <param name="page"></param>
    /// <param name="orderby"></param>
    /// <param name="order"></param>
    /// <param name="serieName"></param>
    /// <param name="serieId"></param>
    /// <param name="saisonNumber"></param>
    /// <returns></returns>
    private string BuildUrl(string category, string path, string query, int page, OrderByEnum orderby = OrderByEnum.DateAjout, OrderEnum order = OrderEnum.Desc, string serieName = "", int serieId = 0, int saisonNumber = 0)
    {
      if (!string.IsNullOrEmpty(serieName))//http://www.omgtorrent.com/series/elementary_saison_1_146.html
        return FullUrl(string.Format("/series/{0}_saison_{1}_{2}.html", serieName, saisonNumber, serieId));
      if (!string.IsNullOrEmpty(query))//http://www.omgtorrent.com/recherche/?query=men
        return FullUrl(string.Format("/recherche/?query={0}&page={1}", query, page));
      if (string.IsNullOrEmpty(path))
        return FullUrl("/");
      if (!string.IsNullOrEmpty(category))
        return FullUrl(string.Format("/{3}/genre/{4}/?order={0}&orderby={1}&page={2}", GetOrderBy(orderby), GetOrder(order), page, path, category));

      return FullUrl(string.Format("/{3}/?order={0}&orderby={1}&page={2}", GetOrderBy(orderby), GetOrder(order), page, path));
    }

    #region Extract torrent Info

    /// <summary>
    /// Get Media Container Html bloc
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    private Item GetMediaItemContainer(HtmlNode x)
    {
      try
      {
        var item = new Item { Is_playable = true };
        item.Label = x.Descendants("div").FirstOrDefault().Element("a").Element("img").attribute("alt").Value;
        item.Label2 = item.Label;
        //i.IconImage =
        //    FullUrl(x.Descendants("div").FirstOrDefault().Element("a").Element("img").attribute("src").value());
        item.Path = FullUrl(x.SelectSingleNode(".//div[2]").Element("a").attribute("href").Value);
        item.Thumbnail =
            Tools.TryGetValue(() => FullUrl(x.Descendants("div").FirstOrDefault().Element("a").Element("img").attribute("src").value()));
        if (item.Properties == null)
          item.Properties = new Properties();

        item.Properties.Sources = Tools.TryGetValue(() => x.SelectSingleNode(".//span[@class='sources']")
                            .InnerText);
        item.Properties.Sources = Tools.TryGetValue(() => x.SelectSingleNode(".//span[@class='clients']")
                            .InnerText);
        item.Info = new InfoMovie
          {
            Genre =
                Tools.TryGetValue(() => x.SelectSingleNode(".//div[@class='torrents_genre']").InnerText),
            Year =
                Tools.TryGetValue(() => Convert.ToInt32(
                    x.SelectSingleNode(".//div[@class='torrents_annee_de_production']")
                        .InnerText))
          };
        return item;
      }
      catch (Exception)
      {

      }
      return null;
    }

    /// <summary>
    /// Get Media Container Html bloc
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    private Item GetMediaItemSerieBlock(HtmlNode x)
    {
      try
      {
        var item = new Item { Is_playable = false, Label = x.Element("h1").InnerText };
        var iconPath = Tools.TryGetValue(() => x.Element("img").attribute("src").Value);
        item.Icon = iconPath.Contains("http") ? iconPath : FullUrl(iconPath);
        item.Thumbnail = item.Icon;
        item.Properties = new Properties
        {
          Label = item.Label,
          TvSerieId = Tools.TryGetValue(() => GetTvSerieId(FullUrl(x.SelectSingleNode(".//p[2]").Element("a").attribute("href").Value)), "0"),
          SaisonCount = Tools.TryGetValue(() => x.SelectNodes(".//p[2]/a").Count.ToString(), "0")
        };
        SetInfoTvSerie(item);
        item.Label2 = item.Properties.SaisonCount;
        return item;
      }
      catch (Exception)
      {
      }
      return null;
    }

    /// <summary>
    /// Get Media Container Html bloc
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private Item GetMediaItemRow(HtmlNode node)
    {
      try
      {
        var item = new Item
        {
          Is_playable = true,
          Properties = new Properties(),
          Label = node.SelectSingleNode(".//td[2]").Element("a").InnerText
        };
        item.Properties.Label = item.Label;
        item.Label2 = item.Label;
        item.Path = FullUrl(node.SelectSingleNode(".//td[6]").Element("a").attribute("href").Value);
        item.Info = new InfoMovie();
        item.Properties.Clients = Tools.TryGetValue(() => node.SelectSingleNode(".//td[5]").InnerText, "0");
        item.Properties.Sources = Tools.TryGetValue(() => node.SelectSingleNode(".//td[4]").InnerText, "0");
        var match = new Regex("([0-9]+.[0-9]*)(.)*").Match(node.SelectSingleNode(".//td[3]").InnerText);
        if (match.Success)
          item.Info.Size = Tools.TryGetValue(() => (long)Convert.ToDouble(match.Groups[1].Value.replace(".", ",")));
        if (node.SelectSingleNode(".//td[1]").InnerText.ToLowerInvariant().Contains("série"))
        {
          SetInfoTvSerie(item);
        }
        else
        {
          var infos = TorrentHelper.GetMovieInfoFromTorrentName(item.Label);
          if (infos != null)
          {
            var res = infos.Year > 0 ? TMDbClient.SearchMovie(infos.Label, infos.Year) : TMDbClient.SearchMovie(infos.Label);
            if (res.TotalResults > 0)
            {
              item.CompleteInfo(TMDbClient.GetMovie(res.Results.FirstOrDefault().Id, "fr"));
              item.Label2 = Tools.TryGetValue(() => res.Results.FirstOrDefault().OriginalTitle);
              //item.IconImage = string.Concat(TmdbConfig.ImageSmallBaseUrl, res.Results.FirstOrDefault().PosterPath);
              item.Thumbnail = Tools.TryGetValue(() => string.Concat(TmdbConfig.ImageLargeBaseUrl,
                  res.Results.FirstOrDefault().PosterPath));
            }
          }
        }
        return item;
      }
      catch (Exception)
      {
      }
      return null;
    }

    /// <summary>
    /// Set Info TvSerie
    /// </summary>
    /// <param name="item"></param>
    private void SetInfoTvSerie(Item item)
    {
      var infos = TorrentHelper.GetTvInfoFromTorrentName(item.Label);
      if (infos != null)
      {
        var res = TMDbClient.SearchTvShow(infos.Label);
        if (res.TotalResults > 0)
        {
          item.CompleteInfo(TMDbClient.GetTvShow(res.Results.FirstOrDefault().Id, language: "fr"), infos.SaisonNumber, infos.EpisodeNumber);

          //item.Label2 = res.Results.FirstOrDefault().OriginalName;
          item.Icon = Tools.TryGetValue(() => string.Concat(TmdbConfig.ImageSmallBaseUrl,
              res.Results.FirstOrDefault().PosterPath));
          item.Thumbnail = Tools.TryGetValue(() => string.Concat(TmdbConfig.ImageLargeBaseUrl,
              res.Results.FirstOrDefault().PosterPath));
          item.Properties.Fanart_image = Tools.TryGetValue(() => string.Concat(TmdbConfig.ImageLargeBaseUrl,
              res.Results.FirstOrDefault().PosterPath));
        }
        else
          item.Info = new InfoTvShow { Tvshowtitle = item.Label };
      }
      else
        item.Info = new InfoTvShow { Tvshowtitle = item.Label };
    }

    /// <summary>
    /// Set Info TvSerie
    /// </summary>
    /// <param name="item"></param>
    /// <param name="serieName"></param>
    /// <param name="saisonNumber"></param>
    /// <param name="episodeNumber"></param>
    private void SetInfoTvSerie(Item item, string serieName, int saisonNumber, int episodeNumber)
    {
      if (item.Properties == null)
        item.Properties = new Properties { Label = item.Label };
      var res = TMDbClient.SearchTvShow(serieName);
      if (res.TotalResults > 0)
      {
        var tvEp = TMDbClient.GetTvEpisode(res.Results.FirstOrDefault().Id, saisonNumber, episodeNumber, language: "fr");
        item.CompleteInfo(tvEp);
        //item.Label2 = res.Results.FirstOrDefault().OriginalName;
        item.Icon = Tools.TryGetValue(() => string.Concat(TmdbConfig.ImageSmallBaseUrl,
            res.Results.FirstOrDefault().PosterPath));
        item.Thumbnail = Tools.TryGetValue(() => string.Concat(TmdbConfig.ImageLargeBaseUrl,
            tvEp.StillPath));
        item.Properties.Fanart_image = Tools.TryGetValue(() => string.Concat(TmdbConfig.ImageLargeBaseUrl,
            res.Results.FirstOrDefault().PosterPath));
      }
    }

    /// <summary>
    /// Get série Id from Url
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    private string GetTvSerieId(string url)
    {
      Regex idregex = new Regex("(.*)/series/(.*)_saison_([0-9]+)_([0-9]+).*");
      var match = idregex.Match(url);
      if (match.Success)
        return match.Groups[4].Value;
      return string.Empty;
    }

    /// <summary>
    /// GetMediaItemBlock
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private Item GetMediaItemBlock(HtmlNode node)
    {
      try
      {
        var item = new Item();
        item.Label = node.NextSibling.InnerText;
        item.Label2 = item.Label;

        var infos = TorrentHelper.GetTvInfoFromTorrentName(item.Label);
        if (infos != null && !string.IsNullOrEmpty(infos.Label))
        {
          var res = TMDbClient.SearchTvShow(infos.Label);
          if (res.TotalResults > 0)
          {
            item.CompleteInfo(TMDbClient.GetTvShow(res.Results.FirstOrDefault().Id, language: "fr"), infos.SaisonNumber, infos.EpisodeNumber);
            item.Label2 = Tools.TryGetValue(() => res.Results.FirstOrDefault().OriginalName);
            //item.IconImage = string.Concat(TmdbConfig.ImageSmallBaseUrl,
            //    res.Results.FirstOrDefault().PosterPath);
            item.Thumbnail = Tools.TryGetValue(() => string.Concat(TmdbConfig.ImageLargeBaseUrl,
                res.Results.FirstOrDefault().PosterPath));
          }
        }
        return item;
      }
      catch (Exception)
      {

      }
      return null;
    }
    #endregion

    #region Enums
    /// <summary>
    /// Get Order Direction
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    private string GetOrder(OrderEnum order)
    {
      switch (order)
      {
        case OrderEnum.Asc:
          return "asc";
        case OrderEnum.Desc:
          return "desc";
        default:
          return "desc";
      }
    }

    /// <summary>
    /// Get Order By
    /// </summary>
    /// <param name="orderby"></param>
    /// <returns></returns>
    private string GetOrderBy(OrderByEnum orderby)
    {
      switch (orderby)
      {
        case OrderByEnum.Top:
          return "top";
        case OrderByEnum.Taille:
          return "taille";
        case OrderByEnum.DateAjout:
          return "id";
        case OrderByEnum.Clients:
          return "leechers";
        case OrderByEnum.Categorie:
          return "id_cat";
        case OrderByEnum.Source:
          return "seeders";
        case OrderByEnum.Nom:
          return "rls";
        case OrderByEnum.Telecharger:
          return "downloaded";
      }
      return "id";
    }

    /// <summary>
    /// Type Extract Enum
    /// </summary>
    public enum TypeExtractEnum
    {
      Container,
      Raw,
      Block,
      SerieBlock
    }
    #endregion
  }
}