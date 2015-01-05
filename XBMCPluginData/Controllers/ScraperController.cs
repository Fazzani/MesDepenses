using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HtmlAgilityPack;
using XBMCPluginData.Models.Xbmc;
using XBMCPluginData.Services.Scrapers.OmgTorrent;

namespace XBMCPluginData.Controllers
{
  public class ScraperController : ApiController
  {
    /// <summary>
    /// GET api/values
    /// </summary>
    /// <returns></returns>
    [Route("scraper/{site}/{path}/{page}/{orderBy}/{order}")]
    public IEnumerable<Item> List(string site, string path = "", int page = 1, OmgScraperService.OrderByEnum orderBy = OmgScraperService.OrderByEnum.DateAjout, OmgScraperService.OrderEnum order = OmgScraperService.OrderEnum.Desc)
    {
      if (site == "omg")
      {
        var scraperService = new OmgScraperService(ConfigurationManager.AppSettings[site]);
        return scraperService.ListTorrents(path, page, orderBy, order);
      }
      return null;
    }

    /// <summary>
    /// GET api/values
    /// </summary>
    /// <returns></returns>
    [Route("scraper/index/{site}/{path}")]
    public IEnumerable<Item> Index(string site, string path = "")
    {
      if (site == "omg")
      {
        var scraperService = new OmgScraperService(ConfigurationManager.AppSettings[site]);
        return scraperService.ListTorrents();
      }
      return null;
    }

  }
}
