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
        [Route("scraper/{site}/{method}")]
        public IEnumerable<Item> Get(string site, string method)
        {
            if (site == "omg")
            {
                var scraperService = new OmgScraperService(ConfigurationManager.AppSettings[site]);
                return scraperService.LastMovies();
            }
            return null;
        }

        /// <summary>
        /// GET api/values/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// POST api/values
        /// </summary>
        /// <param name="value"></param>
        public void Post([FromBody]string value)
        {
        }

        /// <summary>
        /// PUT api/values/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public void Put(int id, [FromBody]string value)
        {
        }

        /// <summary>
        ///  DELETE api/values/5
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
        }
    }
}
