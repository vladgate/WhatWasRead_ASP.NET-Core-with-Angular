using WhatWasRead_Angular.App_Data.DBModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace WhatWasRead_Angular.App_Data
{
   public interface IRepository
   {
      DbContext Context { get; }

      IEnumerable<Book> Books { get; }
      IEnumerable<Author> Authors { get; }
      IEnumerable<Category> Categories { get; }
      IEnumerable<Language> Languages { get; }
      IEnumerable<Tag> Tags { get; }

      Task<IEnumerable<Book>> UpdateBooksFromFilterUsingRawSql(NameValueCollection queryString, string controller, string action);
      void Dispose();
      Task SaveChangesAsync();

      void AddBook(Book book);
      void RemoveBook(Book book);
      Book FindBook(int id);
      
      void AddAuthor(Author newAuthor);
      void RemoveAuthor(Author author);

      void AddTag(Tag newTag);
      void RemoveTag(Tag tag);
   }
}
