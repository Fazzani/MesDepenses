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
        [System.Web.Http.Route("{site}/{path}/{page:int:min(1)?}/{orderBy:int?}/{order:int?}")]
        [OutputCache(Duration = 10000, VaryByParam = "site;path;category;page;orderBy")]
        [System.Web.Http.HttpGet]
        public IEnumerable<Item> List(string site, string path = "", int page = 1, OmgScraperService.OrderByEnum orderBy = OmgScraperService.OrderByEnum.DateAjout, OmgScraperService.OrderEnum order = OmgScraperService.OrderEnum.Desc)
        {
          //http://localhost:1307/scraper/omg/films/action/1/1/1
            //http://localhost:1307/scraper/omg/films/1/1/1
            //http://localhost:1307/scraper/omg/series-episodes/1/1/1
            if (site == "omg")
            {
                var scraperService = new OmgScraperService(ConfigurationManager.AppSettings[site]);
                if (path.Equals("series"))
                  return scraperService.ListTorrents(string.Empty,path, page, orderBy, order, OmgScraperService.TypeExtractEnum.Block);

                return scraperService.ListTorrents(string.Empty, path, page, orderBy, order);
            }
            return null;
        }

        /// <summary>
        /// GET api/values
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.Route("{site}/{path}/{category}/{page:int:min(1)?}/{orderBy:int?}/{order:int?}")]
        [OutputCache(Duration = 10000, VaryByParam = "site;path;category;page;orderBy")]
        [System.Web.Http.HttpGet]
        public IEnumerable<Item> List(string site, string category, string path = "", int page = 1, OmgScraperService.OrderByEnum orderBy = OmgScraperService.OrderByEnum.DateAjout, OmgScraperService.OrderEnum order = OmgScraperService.OrderEnum.Desc)
        {
          //http://localhost:1307/scraper/omg/films/action/1/1/1
          //http://localhost:1307/scraper/omg/films/1/1/1
          //http://localhost:1307/scraper/omg/series-episodes/1/1/1
          if (site == "omg")
          {
            var scraperService = new OmgScraperService(ConfigurationManager.AppSettings[site]);
            if (path.Equals("series"))
              return scraperService.ListTorrents(category, path, page, orderBy, order, OmgScraperService.TypeExtractEnum.Block);

            return scraperService.ListTorrents(category, path, page, orderBy, order);
          }
          return null;
        }

        /// <summary>
        /// GET api/values
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.Route("index/{site}/{path}")]
        [System.Web.Http.HttpGet]
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
