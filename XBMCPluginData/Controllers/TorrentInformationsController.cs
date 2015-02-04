using System;
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
      public TorrentInfo Get([FromBody]string torrentLink)
      {
          return TorrentHelper.GetTorrentInfoAsync(torrentLink);
      }
  }
}