using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WhatWasRead_Angular.App_Data.DBModels
{
   public class Filter
   {
      public int FilterId { get; set; }

      [Required]
      public int FilterTargetId { get; set; }

      [Required]
      [StringLength(50)]
      public string FilterColumnName { get; set; }

      [Required]
      [StringLength(10)]
      public string QueryWord { get; set; }

      [Required]
      [StringLength(10)]
      public string Comparator { get; set; }

      [Required]
      [StringLength(20)]
      public string FilterName { get; set; }

      public FilterTarget FilterTarget { get; set; }
   }
}
