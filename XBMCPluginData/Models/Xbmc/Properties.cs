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

        /// <summary>
        /// Fanart
        /// </summary>
        [DataMember]
        public string Fanart_image
        {
            get;
            set;
        }

        /// <summary>
        /// leechers
        /// </summary>
        [DataMember]
        public string Sources
        {
            get { return _sources; }
            set { _sources = value; }
        }

        /// <summary>
        /// Seeders
        /// </summary>
        [DataMember]
        public string Clients
        {
            get { return _clients; }
            set { _clients = value; }
        }

        /// <summary>
        /// Is saison link (le lien n'est pas d'une épisode mais d'une saison)
        /// </summary>
        [DataMember]
        public string IsSaison
        {
            get { return _isSaison; }
            set { _isSaison = value; }
        }

        /// <summary>
        /// TvShow TMDB id
        /// </summary>
        [DataMember]
        public string TvSerieId
        {
            get;
            set;
        }

        /// <summary>
        /// Le nombre des saisons
        /// </summary>
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

        /// <summary>
        /// the torrent file name
        /// </summary>
        [DataMember]
        public string TorrentFileName
        {
            get;
            set;
        }

        /// <summary>
        /// Le titre de la série
        /// </summary>
        [DataMember]
        public string TvShowName
        {
            get;
            set;
        }

        /// <summary>
        /// the name of Task in the torrent file
        /// </summary>
        [DataMember]
        public string TorrentName
        {
            get;
            set;
        }

        /// <summary>
        /// le numéro de la saison
        /// </summary>
        [DataMember]
        public string SaisonNumber
        {
            get { return _saisonNumber; }
            set { _saisonNumber = value; }
        }
    }
}