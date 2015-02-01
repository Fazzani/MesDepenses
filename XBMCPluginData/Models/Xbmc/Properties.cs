using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace XBMCPluginData.Models.Xbmc
{
    [DataContract]
    public class Properties
    {
        string _sources = "0";
        string _clients = "0";
        string _isSaison = "0";
        string _saisonNumber = "0";
        [DataMember]
        public string Fanart_image { get; set; }
        [DataMember]
        public string Sources
        {
            get { return _sources; }
            set { _sources = value; }
        }
        [DataMember]
        public string Clients
        {
            get { return _clients; }
            set { _clients = value; }
        }
        [DataMember]
        public string IsSaison
        {
            get { return _isSaison; }
            set { _isSaison = value; }
        }
        [DataMember]
        public string TvSerieId
        {
            get;
            set;
        }

        [DataMember]
        public string SaisonCount
        {
            get;
            set;
        }

        [DataMember]
        public string Label
        {
          get;
          set;
        }

        [DataMember]
        public string TorrentFileName
        {
          get;
          set;
        }

        [DataMember]
        public string TvShowName
        {
          get;
          set;
        }

        [DataMember]
        public string SaisonLabel
        {
          get;
          set;
        }

        [DataMember]
        public string SaisonNumber
        {
          get { return _saisonNumber; }
          set { _saisonNumber = value; }
        }
    }
}