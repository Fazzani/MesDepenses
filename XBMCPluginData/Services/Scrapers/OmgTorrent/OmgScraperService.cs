using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HtmlAgilityPack;
using FluentSharp.HtmlAgilityPacK;
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

    /// <summary>
    /// List Torrents
    /// </summary>
    /// <param name="path"></param>
    /// <param name="page"></param>
    /// <param name="orderby"></param>
    /// <param name="order"></param>
    /// <returns></returns>
    public IEnumerable<Item> ListTorrents(string path = "", int page = 1, OrderByEnum orderby = OrderByEnum.DateAjout, OrderEnum order = OrderEnum.Desc)
    {
      HtmlDocument doc = this.HtmlWeb.Load(BuildUrl(path, page, orderby, order));
      var movies = doc.DocumentNode.SelectNodes("//div[@class='cadre torrents_conteneur']");
      return movies.AsParallel().Select(GetMediaItemContainer);
      /*
     <div class="cadre torrents_conteneur">
              <div style="margin-bottom: 9px;">
                  <a href="/films/super-trash-2013-truefrench-dvdrip-xvid-ptb_21775.html" title="Télécharger le torrent Super Trash">
                      <img src="/img/torrents/films/222792.jpg" alt="Super Trash" class="film_img_taille">
                      <br/>
                      Super Trash
                  </a>
              </div>
         <div>
        <img src="/img/s.png" alt="Sources" title="Sources" style="vertical-align: middle;"/> <span class="sources">14231</span>
        <img src="/img/c.png" alt="Clients" title="Clients" style="vertical-align: middle;"/> <span class="clients">1297</span>
        <a href="/clic_dl.php?id=21775"><img src="/img/telechargement_mini.png" alt="Télécharger ce torrent" title="Télécharger le torrent Super Trash" style="vertical-align: middle;"/></a>
    </div>
    <div class="torrents_genre"><a href="/films/genre/documentaire/" style="font-size: 11px;">Documentaire</a></div>
    <div class="torrents_annee_de_production">2012</div>
    </div>
   */
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
        case OrderByEnum.Telechager:
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
    private string BuildUrl(string path, int page, OrderByEnum orderby, OrderEnum order)
    {
      if(string.IsNullOrEmpty(path))
        return FullUrl("/");
      return FullUrl(string.Format("/{3}/?order={0}&orderby={1}&page={2}", GetOrderBy(orderby), GetOrder(order), page,path));
    }

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
        var i = new Item();
        i.Label = x.Descendants("div").FirstOrDefault().Element("a").Element("img").attribute("alt").Value;
        i.IconImage =
            FullUrl(x.Descendants("div").FirstOrDefault().Element("a").Element("img").attribute("src").value());
        i.Path = FullUrl(x.SelectSingleNode(".//div[2]").Element("a").attribute("href").Value);
        i.ThumbnailImage =
            FullUrl(x.Descendants("div").FirstOrDefault().Element("a").Element("img").attribute("src").value());
        i.Bag = new Dictionary<string, string>
                {
                    {
                        "sources",
                        x.SelectSingleNode(".//span[@class='sources']")
                            .InnerText
                    },
                    {
                        "clients",
                        x.SelectSingleNode(".//span[@class='clients']")
                            .InnerText
                    }
                };
        i.Info = new Info
        {
          Type = TypeInfo.Video,
          InfoLabels = new InfoMovie
          {
            Genre =
                x.SelectSingleNode(".//div[@class='torrents_genre']").InnerText,
            Year =
                Convert.ToInt32(
                    x.SelectSingleNode(".//div[@class='torrents_annee_de_production']")
                        .InnerText)
          }
        };
        return i;
      }
      catch (Exception)
      {

      }
      return null;
    }

  }
}