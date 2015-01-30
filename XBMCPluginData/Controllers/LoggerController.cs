using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XBMCPluginData.Hubs;

namespace XBMCPluginData.Controllers
{
    [RoutePrefix("logger")]
    [Route("{action=post}")]
    public class LoggerController : ApiController
    {
        /// <summary>
        /// POST logger/send
        /// </summary>
        /// <param name="message"></param>
        [Route("send")]
        public HttpResponseMessage SendMessage(LogMessage message)
        {
            HubManager.CtxSignalHub.Clients.All.NewMessage(message.Level, message.Source, message.Message);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

    }

    public class LogMessage
    {
        public LogLevelEnum Level { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public override string ToString()
        {
            return string.Format("Level : {0} Source : {1} Message : {2}", Level, Source, Message);
        }
    }

    public enum LogLevelEnum
    {
        Error,
        Warning,
        Info,
        Debug
    }
}
