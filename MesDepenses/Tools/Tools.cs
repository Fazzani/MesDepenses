using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace System
{
    public static class Tools
    {
        /// <summary>
        /// Converts a CSV string to a Json array format.
        /// </summary>
        /// <remarks>First line in CSV must be a header with field name columns.</remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string CsvToJson(this string value)
        {
            // Get lines.
            if (value == null) return null;
            string[] lines = value.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 2) throw new InvalidDataException("Must have header line.");

            // Get headers.
            string[] headers = lines.First().Split(';');

            // Build JSON array.
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[");
            for (int i = 1; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                if (fields.Length != headers.Length) continue;
                var jsonElements = headers.Zip(fields, (header, field) => string.Format("\"{0}\": \"{1}\"", header, field.Replace(" ", "_"))).ToArray();
                string jsonObject = "{" + string.Format("{0}", string.Join(",", jsonElements)) + "}";
                if (i < lines.Length - 1)
                    jsonObject += ",";
                sb.AppendLine(jsonObject);
            }
            sb.AppendLine("]");
            return sb.ToString().Replace(" ", "").Replace("_"," ").Replace('é', 'e');
        }

        public static T FromJson<T>(this string json)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            return ser.Deserialize<T>(json);
        }
    }
}