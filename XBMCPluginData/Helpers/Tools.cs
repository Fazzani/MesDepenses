using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Torrent;
using System.Net.Torrent.BEncode;
using System.Threading.Tasks;
using System.Web;

namespace XBMCPluginData.Helpers
{
  public static class Tools
  {
    /// <summary>
    /// TryGetValue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    public static T TryGetValue<T>(Func<T> func)
    {
      try
      {
        return func();
      }
      catch
      {
      }
      return default(T);
    }

    /// <summary>
    /// TryGetValue with default value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="func"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static T TryGetValue<T>(Func<T> func, T defaultValue)
    {
      try
      {
        return func();
      }
      catch
      {
      }
      return defaultValue;
    }

   
  }
}