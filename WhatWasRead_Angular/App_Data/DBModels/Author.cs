using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WhatWasRead_Angular.App_Data.DBModels
{
    public class Author
   {
      public Author()
      {
         AuthorsOfBooks = new HashSet<AuthorsOfBooks>();
      }
      public int AuthorId { get; set; }

      [Required]
      [StringLength(30)]
      [JsonIgnore]
      public string FirstName { get; set; }

      [Required]
      [StringLength(30)]
      [JsonIgnore]
      public string LastName { get; set; }

      [JsonIgnore]
      public virtual ICollection<AuthorsOfBooks> AuthorsOfBooks { get; set; }

      public string DisplayText
        {
            get
            {
                return this.LastName + " " + this.FirstName;
            }
        }
    }
}
