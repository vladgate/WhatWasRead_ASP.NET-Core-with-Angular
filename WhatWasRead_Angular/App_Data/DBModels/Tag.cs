using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WhatWasRead_Angular.App_Data.DBModels
{
    public class Tag
    {
      public Tag()
      {
         this.BookTags = new HashSet<BookTags>();
      }
      public int TagId { get; set; }

      [Required]
      [MaxLength(50)]
      public string NameForLabels { get; set; }

      [Required]
      [MaxLength(50)]
      public string NameForLinks { get; set; }

      [JsonIgnore]
      public virtual ICollection<BookTags> BookTags { get; set; }

   }
}