using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ToolsLibrary
{
  public static class EnumerableExtensions
  {

    /// <summary>
    /// OrderBy property String and sort Direction
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="property"></param>
    /// <param name="sortDirection"></param>
    /// <returns></returns>
    public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> list, string property, SortDirection sortDirection) where T : class
    {
      PropertyInfo prop;
      if (string.IsNullOrEmpty(property))
        throw new ArgumentNullException("property");
      return sortDirection == SortDirection.asc ? list.OrderByDescending(x => GetPropertyValue(x, property)) : list.OrderBy(x => GetPropertyValue(x, property));
    }

    public static object GetPropertyValue(object obj, string propertyName)
    {
      foreach (var prop in propertyName.Split('.').Select(s => obj.GetType().GetProperty(s)))
        obj = prop.GetValue(obj, null);

      return obj;
    }

    public enum SortDirection
    {
      asc,
      desc
    }

  }
}
