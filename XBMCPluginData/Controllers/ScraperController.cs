using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using XBMCPluginData.Models.Xbmc;
using XBMCPluginData.Services.Scrapers.OmgTorrent;

namespace XBMCPluginData.Controllers
{
  [System.Web.Http.RoutePrefix("scraper")]
  [System.Web.Http.Route("{action=index}")]
  public class ScraperController : ApiController
  {
    /// <summary>
    /// GET api/values
    /// </summary>
    /// <returns></returns>
    [System.Web.Http.Route("index/{site}/{path}", Order = 1)]
    [System.Web.Http.HttpGet]
    public IEnumerable<Item> Index(string site, string path = "")
    {
      if (site == "omg")
      {
        var scraperService = new OmgScraperService(ConfigurationManager.AppSettings[site]);
        return scraperService.ListTorrents(null);
      }
      return null;
    }

    /// <summary>
    /// GET api/values
    /// </summary>
    /// <returns></returns>
    [System.Web.Http.Route("{site}/search/{query}/{page}", Order = 2)]
    [System.Web.Http.HttpGet]
    public IEnumerable<Item> Index(string site, string query = "", int page = 0)
    {
      if (site == "omg")
      {
        var scraperService = new OmgScraperService(ConfigurationManager.AppSettings[site]);
        return scraperService.Search(query, page);
      }
      return null;
    }

    /// <summary>
    /// GET api/values #http://localhost:1307/scraper/omg/series/Elementary/2/1
    /// </summary>
    /// <returns></returns>
    [System.Web.Http.Route("{site}/series_all/{serieName}/{serieId:int}/{saisonNumber:int}", Order = 3)]
    [System.Web.Http.HttpGet]
    public IEnumerable<Item> Index(string site, string serieName, int serieId, int saisonNumber)
    {
      if (site == "omg")
      {
        var scraperService = new OmgScraperService(ConfigurationManager.AppSettings[site]);
        return scraperService.GetSaison(serieName, serieId, saisonNumber);
      }
      return null;
    }

    /// <summary>
    /// GET api/values
    /// </summary>
    /// <returns></returns>
    [System.Web.Http.Route("{site}/{path}/{page:int:min(1)?}/{orderBy:int?}/{order:int?}", Order = 4)]
    [OutputCache(Duration = 10000, VaryByParam = "site;path;category;page;orderBy")]
    [System.Web.Http.HttpGet]
    public IEnumerable<Item> List(string site, [FromUri]List<string> formatfilters, string path = "", int page = 1, OmgScraperService.OrderByEnum orderBy = OmgScraperService.OrderByEnum.DateAjout, OmgScraperService.OrderEnum order = OmgScraperService.OrderEnum.Desc)
    {
      //http://localhost:1307/scraper/omg/films/action/1/1/1
      //http://localhost:1307/scraper/omg/films/1/1/1
      //http://localhost:1307/scraper/omg/series-episodes/1/1/1
      var filters = formatfilters.ToDictionary(x => x, y => "id_cat[]");
      if (site == "omg")
      {
        var scraperService = new OmgScraperService(ConfigurationManager.AppSettings[site]);
        if (path.Equals("series"))
          return scraperService.ListTorrents(filters, string.Empty, path, page, orderBy, order, OmgScraperService.TypeExtractEnum.SerieBlock);

        return scraperService.ListTorrents(filters, string.Empty, path, page, orderBy, order);
      }
      return null;
    }

    /// <summary>
    /// GET api/values
    /// </summary>
    /// <returns></returns>
    [System.Web.Http.Route("{site}/{path}/{category}/{page:int:min(1)?}/{orderBy:int?}/{order:int?}", Order = 5)]
    [OutputCache(Duration = 10000, VaryByParam = "site;path;category;page;orderBy")]
    [System.Web.Http.HttpGet]
    public IEnumerable<Item> List(string site, string category, [FromUri]List<string> formatfilters, string path = "", int page = 1, OmgScraperService.OrderByEnum orderBy = OmgScraperService.OrderByEnum.DateAjout, OmgScraperService.OrderEnum order = OmgScraperService.OrderEnum.Desc)
    {
      //http://localhost:1307/scraper/omg/films/action/1/1/1
      //http://localhost:1307/scraper/omg/films/1/1/1
      //http://localhost:1307/scraper/omg/series-episodes/1/1/1
      if (site == "omg")
      {
        var filters = formatfilters.ToDictionary(x => "id_cat[]", y => y);
        var scraperService = new OmgScraperService(ConfigurationManager.AppSettings[site]);
        if (path.Equals("series"))
          return scraperService.ListTorrents(filters, category, path, page, orderBy, order, OmgScraperService.TypeExtractEnum.SerieBlock);

        return scraperService.ListTorrents(filters, category, path, page, orderBy, order);
      }
      return null;
    }



  }
}
