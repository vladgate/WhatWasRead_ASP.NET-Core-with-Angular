using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhatWasRead_Angular.App_Data.DBModels
{
   public class BookTags
   {
      public int BookId { get; set; }
      public Book Book { get; set; }
      public int TagId { get; set; }
      public Tag Tag { get; set; }
   }
}
