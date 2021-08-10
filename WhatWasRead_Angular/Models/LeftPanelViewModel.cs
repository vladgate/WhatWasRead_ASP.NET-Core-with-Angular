using WhatWasRead_Angular.App_Data.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WhatWasRead_Angular.Models
{
   public class LeftPanelViewModel
   {
      public const string AuthorQueryWord = "author";
      public const string LanguageQueryWord = "lang";
      public const string PagesQueryWord = "pages";
      public IEnumerable<Category> Categories { get; set; }
      public IEnumerable<LanguageViewModel> Languages { get; set; }
      public IEnumerable<AuthorViewModel> Authors { get; set; }
      public IEnumerable<Tag> Tags { get; set; }
      public int MinPagesExpected { get; set; } // min amount of pages in book
      public int MaxPagesExpected { get; set; } // max amount of pages in book
      public int? MinPagesActual { get; set; } //save filter value
      public int? MaxPagesActual { get; set; } //save filter value
   }
}