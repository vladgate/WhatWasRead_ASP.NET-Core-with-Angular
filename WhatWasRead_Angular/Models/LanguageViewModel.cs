using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhatWasRead_Angular.Models
{
   public class LanguageViewModel
   {
      public int LanguageId { get; set; }
      public string NameForLinks { get; set; }
      public string NameForLabels { get; set; }
      public string Link{ get; set; }
      public bool Checked { get; set; }
   }
}
