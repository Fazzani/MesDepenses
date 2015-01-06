using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using FluentSharp.CoreLib.API;
using XBMCPluginData.Models.Xbmc.InfoTypedItems;

namespace XBMCPluginData.Models.Xbmc
{
    [DataContract]
    public class Info
    {
        [DataMember]
        public TypeInfo Type { get; set; }
        [DataMember]
        public InfoMediaBase InfoLabels { get; set; }
    }

    public enum TypeInfo
    {
        [EnumMember]
        General,
        [EnumMember]
        Video,
        [EnumMember]
        Music,
        [EnumMember]
        Pictures
    }
}