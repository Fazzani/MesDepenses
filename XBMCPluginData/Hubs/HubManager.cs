using Microsoft.AspNet.SignalR;

namespace XBMCPluginData.Hubs
{
    /// <summary>
    /// Hub Manager
    /// </summary>
    public class HubManager
    {
        #region Events

        /// <summary>
        /// context Hub Siagnal
        /// </summary>
        public static readonly IHubContext CtxSignalHub;
        #endregion

        static HubManager()
        {
            CtxSignalHub = GlobalHost.ConnectionManager.GetHubContext<LoggerHub>();
        }

    }

}