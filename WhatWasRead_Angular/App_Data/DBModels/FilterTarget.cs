using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WhatWasRead_Angular.App_Data.DBModels
{
   public class FilterTarget
   {
      public FilterTarget()
      {
         Filters = new HashSet<Filter>();
      }

      public int FilterTargetId { get; set; }

      [Required]
      [StringLength(50)]
      public string FilterTargetName { get; set; }

      [Required]
      [StringLength(50)]
      public string FilterTargetSchema { get; set; }

      [Required]
      [StringLength(50)]
      public string ControllerName { get; set; }

      [Required]
      [StringLength(50)]
      public string ActionName { get; set; }

      public virtual ICollection<Filter> Filters { get; set; }

   }
}
