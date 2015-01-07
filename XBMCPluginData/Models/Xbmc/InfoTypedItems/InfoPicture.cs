using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace XBMCPluginData.Models.Xbmc.InfoTypedItems
{
  [DataContract]
  public class InfoPicture : Info
  {
    /*
     *  title: string (In the last summer-1)
picturepath: string (/home/username/pictures/img001.jpg)
exif*: string (See CPictureInfoTag::TranslateString in PictureInfoTag.cpp for valid strings)
     */
    [DataMember]
    public String Title { get; set; }
    [DataMember]
    String Picturepath { get; set; }
    [DataMember]
    String Exif { get; set; }
  }
}