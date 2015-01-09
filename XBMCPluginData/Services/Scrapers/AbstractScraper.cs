using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using HtmlAgilityPack;
using TMDbLib.Client;
using XBMCPluginData.Helpers;

namespace XBMCPluginData.Services.Scrapers
{
  public abstract class AbstractScraper
  {
    protected const string UserAgent =
        "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36";

    protected HtmlDocument HtmlDoc { get; set; }
    protected readonly string _baseUrl;
    protected readonly string BaseUrlTmdb;
    protected readonly TmdbConfig TmdbConfig;
    protected readonly TMDbClient TMDbClient;

    private HtmlWeb _htmlWeb;

    protected AbstractScraper(string baseUrl)
    {
      _baseUrl = baseUrl;
      HtmlDoc = new HtmlDocument();
      TmdbConfig = new TmdbConfig();
      TMDbClient = new TMDbClient(TmdbConfig.ApiKey);
    }

    protected string FullUrl(string urlSuffix)
    {
      return string.Format("{0}{1}", _baseUrl, urlSuffix);
    }

    protected string FullUrlTorrent(string urlSuffix)
    {
      return string.Format("plugin://plugin.video.xbmctorrent/play/{0}{1}", _baseUrl, urlSuffix);
    }

    public bool OnPreRequest2(HttpWebRequest request)
    {
      if (PostData != null && !string.IsNullOrEmpty(PostData.ToString()))
      {
        ASCIIEncoding ascii = new ASCIIEncoding();
        byte[] postBytes = ascii.GetBytes(PostData.ToString());
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = postBytes.Length;
        request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        // add post data to request
        Stream postStream = request.GetRequestStream();
        postStream.Write(postBytes, 0, postBytes.Length);
        postStream.Flush();
        postStream.Close();
      }
      //request.CookieContainer = new CookieContainer();
      //var domain = _baseUrl.Replace("http://www", "");
      //request.CookieContainer.Add(new Cookie("ot_affiche", "value", "/", domain));
      return true;
    }

    public StringBuilder PostData { get; set; }

    protected HtmlWeb HtmlWeb
    {
      get
      {
        if (_htmlWeb == null)
        {
          _htmlWeb = new HtmlWeb
          {
            AutoDetectEncoding = true,
            UseCookies = true,
            UserAgent = UserAgent,
            PreRequest = OnPreRequest2
          };
        }
        return _htmlWeb;
      }
    }
    public enum OrderByEnum : int
    {
      Top,
      Taille,
      DateAjout,
      Clients,
      Categorie,
      Source,
      Nom,
      Telecharger
    }
    public enum OrderEnum : int
    {
      Asc,
      Desc
    }
  }
}