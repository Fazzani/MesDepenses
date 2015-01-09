using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using FluentSharp.CoreLib;
using HtmlAgilityPack;
using FluentSharp.HtmlAgilityPacK;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;
using XBMCPluginData.Models.Xbmc;
using XBMCPluginData.Models.Xbmc.InfoTypedItems;

namespace XBMCPluginData.Services.Scrapers.OmgTorrent
{
  public class OmgScraperService : AbstractScraper
  {

    public OmgScraperService(string url)
      : base(url)
    {
    }

    public IEnumerable<Item> Search(string query, int page)
    {
      HtmlDocument doc = HtmlWeb.Load(BuildUrl(string.Empty, string.Empty,query, page, OrderByEnum.Nom, OrderEnum.Desc));
      return doc.DocumentNode.SelectNodes("//table[@class='table_corps']/tr")
              .AsParallel()
              .Select(GetMediaItemRow)
              .Where(x => x != null);
    }
    /// <summary>
    /// List Torrents
    /// </summary>
    /// <param name="path"></param>
    /// <param name="page"></param>
    /// <param name="orderby"></param>
    /// <param name="order"></param>
    /// <returns></returns>
    public IEnumerable<Item> ListTorrents(Dictionary<string, string> filters, string category = "", string path = "", int page = 1, OrderByEnum orderby = OrderByEnum.DateAjout, OrderEnum order = OrderEnum.Desc, TypeExtractEnum typeExtract = TypeExtractEnum.Raw)
    {
      if (filters != null && filters.Count > 0)
      {
        PostData = new System.Text.StringBuilder();
        foreach (var filter in filters)
          PostData.AppendFormat("{0}={1}&", filter.Value, filter.Key);
      }
      HtmlDocument doc = HtmlWeb.Load(BuildUrl(category, path,string.Empty, page, orderby, order), "POST");
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

    private string GetOrderBy(OrderByEnum orderby)
    {
      switch (orderby)
      {
        case OrderByEnum.Top:
          return "top";
        case OrderByEnum.Taille:
          break;
        case OrderByEnum.DateAjout:
          return "id";
        case OrderByEnum.Clients:
          break;
        case OrderByEnum.Categorie:
          break;
        case OrderByEnum.Source:
          break;
        case OrderByEnum.Nom:
          break;
        case OrderByEnum.Telecharger:
          break;
      }
      return "id";
    }

    /// <summary>
    /// Build Site Url
    /// </summary>
    /// <param name="path"></param>
    /// <param name="page"></param>
    /// <param name="orderby"></param>
    /// <param name="order"></param>
    /// <returns></returns>
    private string BuildUrl(string category, string path, string query, int page, OrderByEnum orderby, OrderEnum order)
    {
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
        Debug.WriteLine(x.Descendants("div").FirstOrDefault().Element("a").Element("img").attribute("alt").Value);
        var i = new Item { Is_playable = true };
        i.Label = x.Descendants("div").FirstOrDefault().Element("a").Element("img").attribute("alt").Value;
        //i.IconImage =
        //    FullUrl(x.Descendants("div").FirstOrDefault().Element("a").Element("img").attribute("src").value());
        i.Path = FullUrl(x.SelectSingleNode(".//div[2]").Element("a").attribute("href").Value);
        i.Thumbnail =
            FullUrl(x.Descendants("div").FirstOrDefault().Element("a").Element("img").attribute("src").value());
        //i.Bag = new Dictionary<string, string>
        //        {
        //            {
        //                "sources",
        //                x.SelectSingleNode(".//span[@class='sources']")
        //                    .InnerText
        //            },
        //            {
        //                "clients",
        //                x.SelectSingleNode(".//span[@class='clients']")
        //                    .InnerText
        //            }
        //        };
        i.Info = new InfoMovie
          {
            Genre =
                x.SelectSingleNode(".//div[@class='torrents_genre']").InnerText,
            Year =
                Convert.ToInt32(
                    x.SelectSingleNode(".//div[@class='torrents_annee_de_production']")
                        .InnerText)
          };
        return i;
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
        var item = new Item { Is_playable = false };
        item.Label = x.Element("h1").InnerText;
        item.Icon = FullUrl(x.Element("img").attribute("src").Value);
        //item.Path = FullUrl(x.SelectSingleNode(".//div[2]").Element("a").attribute("href").Value);
        item.Thumbnail = item.Icon;
        SetInfoTvSerie(item);

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
        var item = new Item { Is_playable = true, Properties = new Properties() };
        item.Label = node.SelectSingleNode(".//td[2]").Element("a").InnerText;
        item.Path = FullUrl(node.SelectSingleNode(".//td[6]").Element("a").attribute("href").Value);
        //item.Bag = new Dictionary<string, string>
        //        {
        //            {
        //                "sources",
        //                node.SelectSingleNode(".//td[4]").InnerText
        //            },
        //            {
        //                "clients",
        //                node.SelectSingleNode(".//td[5]").InnerText
        //            }
        //        };
        item.Info = new InfoMovie();

        var match = new Regex("([0-9]+.[0-9]*)(.)*").Match(node.SelectSingleNode(".//td[3]").InnerText);
        if (match.Success)
          item.Info.Size = (long)Convert.ToDouble(match.Groups[1].Value.replace(".", ","));
        if (node.SelectSingleNode(".//td[1]").InnerText.ToLowerInvariant().Contains("série"))
        {
          SetInfoTvSerie(item);
        }
        else
        {
          var infos = TorrentHelper.GetMovieInfoFromTorrentName(item.Label);
          if (infos != null)
          {
            var res = TMDbClient.SearchMovie(infos.Label);
            if (res.TotalResults > 0)
            {
              item.CompleteInfo(TMDbClient.GetMovie(res.Results.FirstOrDefault().Id, "fr"));
              item.Label2 = res.Results.FirstOrDefault().OriginalTitle;
              //item.IconImage = string.Concat(TmdbConfig.ImageSmallBaseUrl, res.Results.FirstOrDefault().PosterPath);
              item.Thumbnail = string.Concat(TmdbConfig.ImageLargeBaseUrl,
                  res.Results.FirstOrDefault().PosterPath);

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

    private void SetInfoTvSerie(Item item)
    {
      item.Properties = new Properties();
      var infos = TorrentHelper.GetTvInfoFromTorrentName(item.Label);
      if (infos != null)
      {
        var res = TMDbClient.SearchTvShow(infos.Label);
        if (res.TotalResults > 0)
        {
          item.CompleteInfo(TMDbClient.GetTvShow(res.Results.FirstOrDefault().Id, language: "fr"), infos.SaisonNumber, infos.EpisodeNumber);
          item.Label2 = res.Results.FirstOrDefault().OriginalName;
          item.Icon = string.Concat(TmdbConfig.ImageSmallBaseUrl,
              res.Results.FirstOrDefault().PosterPath);
          item.Thumbnail = string.Concat(TmdbConfig.ImageLargeBaseUrl,
              res.Results.FirstOrDefault().PosterPath);
          item.Properties.Fanart_image = string.Concat(TmdbConfig.ImageLargeBaseUrl,
              res.Results.FirstOrDefault().PosterPath);
        }
      }
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

        //item.Info = new Info
        //{
        //  Type = TypeInfo.Video,
        //  InfoLabels = new InfoMovie()
        //};

        var infos = TorrentHelper.GetTvInfoFromTorrentName(item.Label);
        if (infos != null && !string.IsNullOrEmpty(infos.Label))
        {
          var res = TMDbClient.SearchTvShow(infos.Label);
          if (res.TotalResults > 0)
          {
            item.CompleteInfo(TMDbClient.GetTvShow(res.Results.FirstOrDefault().Id, language: "fr"), infos.SaisonNumber, infos.EpisodeNumber);
            item.Label2 = res.Results.FirstOrDefault().OriginalName;
            //item.IconImage = string.Concat(TmdbConfig.ImageSmallBaseUrl,
            //    res.Results.FirstOrDefault().PosterPath);
            item.Thumbnail = string.Concat(TmdbConfig.ImageLargeBaseUrl,
                res.Results.FirstOrDefault().PosterPath);
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

    public enum TypeExtractEnum
    {
      Container,
      Raw,
      Block,
      SerieBlock
    }
  }
}