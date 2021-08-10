using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace WhatWasRead_Angular.Infrastructure
{
   public static class Helper
   {
      public static string CreateFilterPartOfLink(string currentPath, NameValueCollection currentQueryString, string queryWord, string value, out bool check)
      {
         check = false;
         if (currentPath.StartsWith("/api"))
         {
            currentPath = currentPath.Remove(0, 4);
         }
         currentPath = Regex.Replace(currentPath, @"\/page\d+", "/page1"); //link always redirect to first page
         if (currentQueryString == null) // no query parameters
         {
            return currentPath + "?" + queryWord + "=" + value;
         }
         NameValueCollection newQueryString = new NameValueCollection(currentQueryString.Count + 1);
         if (currentQueryString.AllKeys.Contains(queryWord))
         {
            string res;
            if (currentQueryString[queryWord].Contains(value))
            {
               if (currentQueryString[queryWord].Contains("," + value + ","))
               {
                  res = currentQueryString[queryWord].Replace("," + value + ",", ",");
                  check = true;
               }
               else if (currentQueryString[queryWord].StartsWith(value + ",")) //remove leading ','
               {
                  res = currentQueryString[queryWord].Replace(value + ",", "");
                  check = true;
               }
               else if (currentQueryString[queryWord].EndsWith("," + value)) //remove trailing ','
               {
                  res = currentQueryString[queryWord].Replace("," + value, "");
                  check = true;
               }
               else if (currentQueryString[queryWord] != value)
               {
                  res = currentQueryString[queryWord] + "," + value;
               }
               else //
               {
                  res = ""; //default
                  check = true;
               }

               if (!String.IsNullOrWhiteSpace(res))
               {
                  newQueryString[queryWord] = res;
               }
            }
            else
            {
               newQueryString[queryWord] = currentQueryString[queryWord] + "," + value;
            }
         }
         else
         {
            newQueryString.Add(queryWord, value);
         }
         for (int i = 0; i < currentQueryString.Count; i++)
         {
            if (currentQueryString.AllKeys[i] == queryWord)
            {
               continue;
            }
            newQueryString.Add(currentQueryString.AllKeys[i], currentQueryString[i]);
         }

         string queryString = BuildQueryString(newQueryString);
         if (!String.IsNullOrWhiteSpace(queryString))
         {
            if (currentPath.EndsWith("/"))
            {
               currentPath = currentPath.Remove(currentPath.Length - 1, 1); //remove last '/'
            }
         }
         return currentPath + queryString;
      }
      public static string BuildQueryString(NameValueCollection currentQueryString)
      {
         if (currentQueryString.Count == 0)
         {
            return "";
         }
         StringBuilder result = new StringBuilder();
         result.Append("?");
         for (int i = 0; i < currentQueryString.AllKeys.Length; i++)
         {
            result.Append(currentQueryString.AllKeys[i]);
            result.Append("=");
            result.Append(currentQueryString[i]);
            result.Append("&");
         }
         result.Remove(result.Length - 1, 1);
         return result.ToString();
      }
      public static string BuildQueryString(QueryString currentQueryString)
      {
         NameValueCollection collection = HttpUtility.ParseQueryString(currentQueryString.ToString());
         return BuildQueryString(collection);
      }
      public static string GetMimeType(string based64ImageSourceWithMime, out byte[] imageDataWithoutMime)
      {
         string withoutMime = based64ImageSourceWithMime.Substring(based64ImageSourceWithMime.IndexOf("base64,") + 7);
         imageDataWithoutMime = Convert.FromBase64String(withoutMime);
         string he = string.Empty;
         for (int i = 0; i < 4; i++)
         {
            he += imageDataWithoutMime[i].ToString("x");
         }
         switch (he)
         {
            case "ffd8ffe0":
            case "ffd8ffe1":
            case "ffd8ffe2":
            case "ffd8ffe3":
            case "ffd8ffe8":
               return "image/jpeg";
            case "89504e47":
               return "image/png";
            default: return null;
         }
      }
   }
}
