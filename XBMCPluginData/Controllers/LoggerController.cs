using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HtmlAgilityPack;

namespace XBMCPluginData.Controllers
{
    [System.Web.Http.RoutePrefix("logger")]
    [System.Web.Http.Route("{action=post}")]
    public class LoggerController : ApiController
    {
        /// <summary>
        /// POST api/values
        /// </summary>
        /// <param name="value"></param>
        public void Post([FromBody]string message)
        {
            Console.WriteLine(message);
        }

    }
}
