using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Torrent;
using System.Net.Torrent.BEncode;
using System.Threading.Tasks;
using System.Web;
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

    [System.Web.Http.Route("index")]
    [System.Web.Http.HttpGet]
    public async Task<dynamic> TestDecodeFile()
    {
      WebClient webClient = new WebClient();
      webClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36");
      var bytes = await webClient.DownloadDataTaskAsync("http://www.omgtorrent.com/clic_dl.php?id=18911");
      //var f = File.OpenRead(HttpContext.Current.Server.MapPath("~/Test/24 heures chrono S01 FRENCH DVDRip XviD [www.OMGTORRENT.com].torrent"));
      var res = BencodingUtils.Decode(bytes)as BDict;
      var infos = (res.SingleOrDefault(x => x.Key == "info").Value as BDict).FirstOrDefault().Value;
      var tmp = (infos as BList).Select(x => x).Cast<BDict>().Select(x => new { Label = x.FirstOrDefault(k => k.Key == "path").Value, size = x.FirstOrDefault(k => k.Key == "length").Value });
        Metadata meta = new Metadata(new MemoryStream(bytes));
        var magnetLinkMeta =
            MagnetLink.ResolveToMetadata(
                "magnet:?xt=urn:btih:FA58ACBFFFEB1A3C24BDF4346A9D93290DA5870A&dn=24.S01.FRENCH.DVDRiP.XviD-FiG0LU&tr=udp%3A%2F%2Ftracker.publicbt.com%3A80&tr=udp%3A%2F%2Ftracker.openbittorrent.com%3A80&tr=udp%3A%2F%2Ftracker.ccc.de%3A80&tr=udp%3A%2F%2Ftracker.istole.it%3A80");
        var lenght = "MjQuUzAxRTEyLkZSRU5DSC5EVkRSaVAuWHZpRC1GaUcwTFUuYXZp".Length;
        //http://en.wikipedia.org/wiki/Magnet_URI_scheme
       // BencodingUtils.EncodeBytes()
      return tmp;
      //BencodingUtils.Decode()
    }

  }
}
