using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhatWasRead_Angular.App_Data.DBModels
{
   public class BooksWithAuthor
   {
      public int BookId { get; set; }
      public string Name { get; set; }
      public int LanguageId { get; set; }
      public int Pages { get; set; }
      public string Description { get; set; }
      public int CategoryId { get; set; }
      public int Year { get; set; }
      public byte[] ImageData { get; set; }
      public string ImageMimeType { get; set; }
      public int AuthorId { get; set; }
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public string NameForLinks { get; set; }
      public Nullable<int> TagId { get; set; }
      public string TagNameForLabels { get; set; }
      public string TagNameForLinks { get; set; }
   }
}
