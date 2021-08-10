using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatWasRead_Angular.App_Data.DBModels
{
   public class Book
   {
      public Book()
      {
         this.AuthorsOfBooks = new HashSet<AuthorsOfBooks>();
         this.BookTags = new HashSet<BookTags>();
      }

      public int BookId { get; set; }

      [Required]
      [StringLength(300)]
      public string Name { get; set; }

      [Required]
      public int LanguageId { get; set; }

      [Required]
      public int Pages { get; set; }

      public string Description { get; set; }

      [Required]
      public int CategoryId { get; set; }

      [Required]
      public int Year { get; set; }

      public byte[] ImageData { get; set; }

      [MaxLength(30)]
      public string ImageMimeType { get; set; }

      public virtual Category Category { get; set; }
      public virtual Language Language { get; set; }
      public virtual ICollection<AuthorsOfBooks> AuthorsOfBooks { get; set; }
      public virtual ICollection<BookTags> BookTags { get; set; }

      public string DisplayAuthors()
      {
         List<Author> authorsList = this.AuthorsOfBooks.Select(ab => ab.Author).ToList();
         if (authorsList.Count == 0)
         {
            return "";
         }
         string authors = string.Empty;
         if (authorsList.Count == 1)
         {
            authors = authorsList.First().FirstName + " " + authorsList.First().LastName;
         }
         else
         {
            StringBuilder sb = new StringBuilder();
            foreach (Author author in authorsList)
            {
               sb.Append(author.FirstName + " " + author.LastName);
               sb.Append(", ");
            }
            sb.Remove(sb.Length - 2, 2); // remove in the last ', '
            authors = sb.ToString();
         }
         return authors;
      }
   }
}