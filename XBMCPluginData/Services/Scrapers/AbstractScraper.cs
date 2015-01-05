using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using HtmlAgilityPack;

namespace XBMCPluginData.Services.Scrapers
{
    public abstract class AbstractScraper
    {
        protected const string UserAgent =
            "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36";

        protected HtmlDocument HtmlDoc { get; set; }
        protected readonly string _baseUrl;
        private HtmlWeb _htmlWeb;

        protected AbstractScraper(string baseUrl)
        {
            _baseUrl = baseUrl;
            HtmlDoc = new HtmlDocument();
        }

        protected string FullUrl(string urlSuffix)
        {
            return string.Format("{0}{1}", _baseUrl, urlSuffix);
        }

        public bool OnPreRequest2(HttpWebRequest request)
        {
            request.CookieContainer = new CookieContainer();
            var domain = _baseUrl.Replace("http://www", "");
            request.CookieContainer.Add(new Cookie("ot_affiche", "value", "/", domain));
            return true;
        }

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
        public enum OrderByEnum
        {
          Top,
          Taille,
          DateAjout,
          Clients,
          Categorie,
          Source,
          Nom,
          Telechager
        }
        public enum OrderEnum
        {
          Asc,
          Desc
        }
    }
}