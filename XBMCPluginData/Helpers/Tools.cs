using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XBMCPluginData.Helpers
{
  public static class Tools
  {
    public static T TryGetValue<T>(Func<T> func)
    {
      try
      {
        return func();
      }
      catch (Exception)
      {
      }
      return default(T);
    }
  }
}