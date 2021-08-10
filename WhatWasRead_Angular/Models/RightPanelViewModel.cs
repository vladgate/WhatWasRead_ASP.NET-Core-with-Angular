using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhatWasRead_Angular.Models
{
   public class RightPanelViewModel
   {
      public IEnumerable<BookShortInfo> BookInfo { get; set; }
      public int TotalPages { get; set; }
   }
}
