using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Torrent.BEncode;

namespace XBMCPluginData.Helpers
{
  public class TorrentHelper
  {
    //Revenge S04E05 FASTSUB VOSTFR HDTV XviD
    private const string tvRegex = @"^(.*)S(?<season>\d{1,2})E(?<episode>\d{1,2})(.*)$";
    //10 bonnes raisons de te larguer FRENCH DVDRip XviD-NoTag
    private const string movieRegex = @"^(.*?)(STV|FRENCH|dvdrip|(?:XviD-?(?:.*))|cd[0-9]|dvdscr|brrip|divx|[\{\(\[]?[0-9]{4}).*$";
    private const string dateRegex = @"^\w* ([0-9]{4} )+.*$";

    /// <summary>
    /// Get Tv série Info From Torrent Name
    /// </summary>
    /// <param name="torrentName"></param>
    /// <param name="saisonLabel"></param>
    /// <returns></returns>
    public static InfoFromTorrentName GetTvInfoFromTorrentName(string torrentName, string saisonLabel = "")
    {
      Regex reg = new Regex(tvRegex);
      var match = reg.Match(torrentName.Replace(".", " ").Replace("-", " "));
      if (match.Success)
      {
        return new InfoFromTorrentName
        {
          EpisodeNumber = Convert.ToInt32(match.Groups["episode"].Value),
          Label = match.Groups[1].Value,
          Quality = match.Groups["episode"].Value,
          SaisonNumber = Convert.ToInt32(match.Groups["season"].Value),
          SaisonLabel = saisonLabel
        };
      }
      return new InfoFromTorrentName { Label = torrentName };
    }

    /// <summary>
    /// Get Tv série Info From Torrent Name
    /// </summary>
    /// <param name="torrentName"></param>
    /// <returns></returns>
    public static InfoFromTorrentName GetMovieInfoFromTorrentName(string torrentName)
    {
      Regex reg = new Regex(movieRegex);
      var matchLabel = reg.Match(torrentName);
      if (matchLabel.Success)
      {
        reg = new Regex(dateRegex);
        var matchDate = reg.Match(torrentName);
        if (!matchDate.Success)
          return new InfoFromTorrentName
          {
            Label = matchLabel.Groups[1].Value,
          };
        return new InfoFromTorrentName
        {
          Label = matchLabel.Groups[1].Value,
          Year = Int32.Parse(matchDate.Groups[1].Value)
        };
      }
      return new InfoFromTorrentName { Label = torrentName };
    }

    /// <summary>
    /// Get Torrent info
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    public static Tuple<string, IEnumerable<KeyValuePair<string, string>>> GetTorrentInfoAsync(string uri)
    {
      WebClient webClient = new WebClient();
      webClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36");
      var bytes = webClient.DownloadData(uri);

      var res = BencodingUtils.Decode(bytes) as BDict;
      var infos = res.SingleOrDefault(x => x.Key == "info").Value as BDict;
      var name = infos.FirstOrDefault(x => x.Key == "name").Value.ToString();
      var files = (infos.FirstOrDefault().Value as BList).Select(x => x).Cast<BDict>().Select(x => new KeyValuePair<string, string>((x.FirstOrDefault(k => k.Key == "path").Value as BList)[0].ToString(), x.FirstOrDefault(k => k.Key == "length").Value.ToString()));
      return new Tuple<string, IEnumerable<KeyValuePair<string, string>>>(name, files);
    }
  }

  public class InfoFromTorrentName
  {
    public string Label { get; set; }
    public int SaisonNumber { get; set; }
    public int EpisodeNumber { get; set; }
    public string Quality { get; set; }
    public int Year { get; set; }
    public string SaisonLabel { get; set; }
  }

}