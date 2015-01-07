using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace XBMCPluginData.Helpers
{
    public class TmdbConfig
    {
        public string ApiKey { get; set; }
        public string ApiBaseUrl { get; set; }
        public string ImageSmallBaseUrl { get; set; }
        public string ImageLargeBaseUrl { get; set; }

        public TmdbConfig()
        {
            ApiKey = ConfigurationManager.AppSettings["API_KEY"];
            ApiBaseUrl = ConfigurationManager.AppSettings["BASE_URL_TMDB"];
            ImageSmallBaseUrl = ConfigurationManager.AppSettings["Image_Small_Base_Url"];
            ImageLargeBaseUrl = ConfigurationManager.AppSettings["Image_Large_Base_Url"];
        }
        
    }
}