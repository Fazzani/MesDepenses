using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XBMCPluginData.Models.Xbmc.InfoTypedItems
{
    public class InfoPicture : InfoMediaBase
    {
        /*
         *  title: string (In the last summer-1)
    picturepath: string (/home/username/pictures/img001.jpg)
    exif*: string (See CPictureInfoTag::TranslateString in PictureInfoTag.cpp for valid strings)
         */
        public String Title { get; set; }
        public String Picturepath { get; set; }
        public String Exif { get; set; }
    }
}