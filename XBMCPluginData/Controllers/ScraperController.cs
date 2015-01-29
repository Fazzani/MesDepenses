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
using XBMCPluginData.Helpers;
using XBMCPluginData.Models.Xbmc;
using XBMCPluginData.Services.Scrapers.OmgTorrent;

namespace XBMCPluginData.Controllers
{
    [System.Web.Http.RoutePrefix("scraper")]
    [System.Web.Http.Route("{action=index}")]
    public class ScraperController : ApiController
    {
        /// <summary>
        /// Index
        /// </summary>
        /// <param name="site"></param>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [System.Web.Http.Route("{site}/search/{query}/{page}", Order = 2)]
        [System.Web.Http.HttpGet]
        public IEnumerable<Item> Index(string site, string query = "", int page = 0)
        {
            if (site != "omg") return null;
            var scraperService = new OmgScraperService(ConfigurationManager.AppSettings[site]);
            return scraperService.Search(query, page);
        }

        /// <summary>
        /// GET ex: http://localhost:1307/scraper/omg/series/Elementary/2/1
        /// </summary>
        /// <param name="site"></param>
        /// <param name="serieName"></param>
        /// <param name="serieId"></param>
        /// <param name="saisonNumber"></param>
        /// <returns></returns>
        [System.Web.Http.Route("{site}/series_all/{serieName}/{serieId:int}/{saisonNumber:int}", Order = 3)]
        [System.Web.Http.HttpGet]
        public IEnumerable<Item> Index(string site, string serieName, int serieId, int saisonNumber)
        {
            if (site != "omg") return null;
            var scraperService = new OmgScraperService(ConfigurationManager.AppSettings[site]);
            return scraperService.GetSaison(serieName, serieId, saisonNumber);
        }

        /// <summary>
        /// List
        /// </summary>
        /// <param name="site"></param>
        /// <param name="formatfilters"></param>
        /// <param name="path"></param>
        /// <param name="page"></param>
        /// <param name="orderBy"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        [System.Web.Http.Route("{site}/{path}/{page:int:min(1)?}/{orderBy:int?}/{order:int?}", Order = 4)]
        [OutputCache(Duration = 10000, VaryByParam = "site;path;category;page;orderBy")]
        [System.Web.Http.HttpGet]
        public IEnumerable<Item> List(string site, [FromUri]List<string> formatfilters, string path = "", int page = 1, OmgScraperService.OrderByEnum orderBy = OmgScraperService.OrderByEnum.DateAjout, OmgScraperService.OrderEnum order = OmgScraperService.OrderEnum.Desc)
        {
            //http://localhost:1307/scraper/omg/films/action/1/1/1
            //http://localhost:1307/scraper/omg/films/1/1/1
            //http://localhost:1307/scraper/omg/series-episodes/1/1/1
            if (site != "omg") return null;
            var filters = formatfilters.ToDictionary(x => x, y => "id_cat[]");
            var scraperService = new OmgScraperService(ConfigurationManager.AppSettings[site]);
            if (path.Equals("series"))
                return scraperService.ListTorrents(filters, string.Empty, path, page, orderBy, order, OmgScraperService.TypeExtractEnum.SerieBlock);

            return scraperService.ListTorrents(filters, string.Empty, path, page, orderBy, order);
        }

        /// <summary>
        /// List
        /// </summary>
        /// <param name="site"></param>
        /// <param name="category"></param>
        /// <param name="formatfilters"></param>
        /// <param name="path"></param>
        /// <param name="page"></param>
        /// <param name="orderBy"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        [System.Web.Http.Route("{site}/{path}/{category}/{page:int:min(1)?}/{orderBy:int?}/{order:int?}", Order = 5)]
        [OutputCache(Duration = 10000, VaryByParam = "site;path;category;page;orderBy")]
        [System.Web.Http.HttpGet]
        public IEnumerable<Item> List(string site, string category, [FromUri]List<string> formatfilters, string path = "", int page = 1, OmgScraperService.OrderByEnum orderBy = OmgScraperService.OrderByEnum.DateAjout, OmgScraperService.OrderEnum order = OmgScraperService.OrderEnum.Desc)
        {
            //http://localhost:1307/scraper/omg/films/action/1/1/1
            //http://localhost:1307/scraper/omg/films/1/1/1
            //http://localhost:1307/scraper/omg/series-episodes/1/1/1
            if (site != "omg") return null;
            var filters = formatfilters.ToDictionary(x => "id_cat[]", y => y);
            var scraperService = new OmgScraperService(ConfigurationManager.AppSettings[site]);
            if (path.Equals("series"))
                return scraperService.ListTorrents(filters, category, path, page, orderBy, order, OmgScraperService.TypeExtractEnum.SerieBlock);

            return scraperService.ListTorrents(filters, category, path, page, orderBy, order);
        }

        //[System.Web.Http.Route("index")]
        //[System.Web.Http.HttpGet]
        //public async Task<IEnumerable<KeyValuePair<string, string>>> TestDecodeFile()
        //{
        //  return await Tools.GetTorrentInfoAsync("http://www.omgtorrent.com/clic_dl.php?id=18911");
        //}

    }
}
