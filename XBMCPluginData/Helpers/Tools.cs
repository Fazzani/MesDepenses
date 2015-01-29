using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Torrent;
using System.Net.Torrent.BEncode;
using System.Threading.Tasks;
using System.Web;

namespace XBMCPluginData.Helpers
{
    public static class Tools
    {
        /// <summary>
        /// TryGetValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T TryGetValue<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch
            {
            }
            return default(T);
        }

        /// <summary>
        /// TryGetValue with default value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T TryGetValue<T>(Func<T> func, T defaultValue)
        {
            try
            {
                return func();
            }
            catch
            {
            }
            return defaultValue;
        }

        public static async Task<IEnumerable<KeyValuePair<string, string>>> GetTorrentInfoAsync(string uri)
        {
            WebClient webClient = new WebClient();
            webClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36");
            var bytes = await webClient.DownloadDataTaskAsync(uri);
            //var f = File.OpenRead(HttpContext.Current.Server.MapPath("~/Test/24 heures chrono S01 FRENCH DVDRip XviD [www.OMGTORRENT.com].torrent"));
            var res = BencodingUtils.Decode(bytes) as BDict;
            var infos = (res.SingleOrDefault(x => x.Key == "info").Value as BDict).FirstOrDefault().Value;
            var tmp = (infos as BList).Select(x => x).Cast<BDict>().Select(x => new System.Collections.Generic.KeyValuePair<string, string>((x.FirstOrDefault(k => k.Key == "path").Value as BList)[0].ToString(), x.FirstOrDefault(k => k.Key == "length").Value.ToString()));
            //Metadata meta = new Metadata(new MemoryStream(bytes));
            // var magnetLinkMeta =
            //     MagnetLink.ResolveToMetadata(
            //         "magnet:?xt=urn:btih:FA58ACBFFFEB1A3C24BDF4346A9D93290DA5870A&dn=24.S01.FRENCH.DVDRiP.XviD-FiG0LU&tr=udp%3A%2F%2Ftracker.publicbt.com%3A80&tr=udp%3A%2F%2Ftracker.openbittorrent.com%3A80&tr=udp%3A%2F%2Ftracker.ccc.de%3A80&tr=udp%3A%2F%2Ftracker.istole.it%3A80");
            // //http://en.wikipedia.org/wiki/Magnet_URI_scheme
            //// BencodingUtils.EncodeBytes()
            return tmp;
        }
    }
}