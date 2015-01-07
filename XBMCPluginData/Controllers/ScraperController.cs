using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
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
        //[OutputCache(Duration = 2000)]
        [System.Web.Http.HttpGet]
        public IEnumerable<Item> List(string site, string path = "", int page = 1, OmgScraperService.OrderByEnum orderBy = OmgScraperService.OrderByEnum.DateAjout, OmgScraperService.OrderEnum order = OmgScraperService.OrderEnum.Desc)
        {
            //http://localhost:1307/scraper/omg/films/1/1/1
            //http://localhost:1307/scraper/omg/series-episodes/1/1/1
            if (site == "omg")
            {
                var scraperService = new OmgScraperService(ConfigurationManager.AppSettings[site]);
                return scraperService.ListTorrents(path, page, orderBy, order,OmgScraperService.TypeExtractEnum.Raw);
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
