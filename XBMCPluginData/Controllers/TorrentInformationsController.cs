using System.Collections.Generic;
using System.Web.Http;
using XBMCPluginData.Helpers;
using XBMCPluginData.Services.Scrapers.OmgTorrent;

namespace XBMCPluginData.Controllers
{
    [RoutePrefix("torrent")]
    public class TorrentInformationsController : ApiController
    {
        /// <summary>
        /// GetTorrentInfos ex: http://localhost:1307/torrent/torrentLink
        /// </summary>
        /// <param name="torrentLink"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Get")]
        public IEnumerable<KeyValuePair<string, InfoFromTorrentName>> Get([FromBody]string torrentLink)
        {
            return OmgScraperService.GetTorrentInfos(torrentLink);
        }
    }
}