using WhatWasRead_Angular.App_Data.DBModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace WhatWasRead_Angular.App_Data
{
   public class FakeRepo : IRepository
   {
      #region InitVariables
      private List<Book> _books;
      private List<Language> _languages = new List<Language>
      {
         new Language {LanguageId = 1, NameForLabels="English", NameForLinks = "en"},
         new Language {LanguageId = 2, NameForLabels="Russian", NameForLinks = "ru"},
         new Language {LanguageId = 3, NameForLabels="Ukrainian", NameForLinks = "ua"},
      };

      private List<Author> _authors = new List<Author>
{
         new Author {AuthorId = 1, FirstName = "F1", LastName="L1"},
         new Author {AuthorId = 2, FirstName = "F2", LastName="L2"},
         new Author {AuthorId = 3, FirstName = "F3", LastName="L3"},
         new Author {AuthorId = 4, FirstName = "F4", LastName="L4"},
         new Author {AuthorId = 5, FirstName = "F5", LastName="L5"},
};

      private List<Tag> _tags = new List<Tag>
      {
         new Tag {TagId = 1, NameForLabels="Tag1", NameForLinks = "tag1"},
         new Tag {TagId = 2, NameForLabels="Tag2", NameForLinks = "tag2"},
         new Tag {TagId = 3, NameForLabels="Tag3", NameForLinks = "tag3"},
      };

      private List<Category> _categories = new List<Category>
      {
        new Category() { CategoryId = 1, NameForLinks = "cat1", NameForLabels = "Category 1" },
        new Category() { CategoryId = 2, NameForLinks = "cat2", NameForLabels = "Category 2" },
        new Category() { CategoryId = 3, NameForLinks = "cat3", NameForLabels = "Category 3" }
      };

      private string _imageMimeType = "image/jpeg";

      private List<Book> CreateBooks()
      {
         Book book1 = new Book
         {
            BookId = 1,
            Category = _categories[0],
            CategoryId = _categories[0].CategoryId,
            Pages = 10,
            Description = "Desc1",
            Name = "Book1",
            Year = 2001,
            Language = _languages[0],
            LanguageId = _languages[0].LanguageId,
            ImageMimeType = _imageMimeType,
            ImageData = new byte[] { 1 },
         };
         book1.AuthorsOfBooks = new List<AuthorsOfBooks>() { new AuthorsOfBooks { AuthorId = _authors[0].AuthorId, Author = _authors[0], BookId = book1.BookId, Book = book1 } };
         book1.BookTags = new List<BookTags>() { new BookTags { TagId = _tags[0].TagId, Tag = _tags[0], BookId = book1.BookId, Book = book1 } };

         Book book2 = new Book
         {
            BookId = 2,
            Category = _categories[0],
            CategoryId = _categories[0].CategoryId,
            Pages = 20,
            Description = "Desc2",
            Name = "Book2",
            Year = 2002,
            Language = _languages[0],
            LanguageId = _languages[0].LanguageId,
            ImageMimeType = _imageMimeType,
            ImageData = new byte[] { 2 },
         };
         book2.AuthorsOfBooks = new List<AuthorsOfBooks>() { new AuthorsOfBooks { AuthorId = _authors[0].AuthorId, Author = _authors[0], Book = book2, BookId = book2.BookId }, new AuthorsOfBooks { AuthorId = _authors[1].AuthorId, Author = _authors[1], Book = book2, BookId = book2.BookId } };
         book2.BookTags = new List<BookTags>() { new BookTags { TagId = _tags[0].TagId, Tag = _tags[0], BookId = book2.BookId, Book = book2 }, new BookTags { TagId = _tags[1].TagId, Tag = _tags[1], BookId = book2.BookId, Book = book2 } };


         Book book3 = new Book
         {
            BookId = 3,
            Category = _categories[0],
            CategoryId = _categories[0].CategoryId,
            Pages = 30,
            Description = "Desc3",
            Name = "Book3",
            Year = 2003,
            Language = _languages[1],
            LanguageId = _languages[1].LanguageId,
            ImageMimeType = _imageMimeType,
            ImageData = new byte[] { 3 },
         };
         book3.AuthorsOfBooks = new List<AuthorsOfBooks>() { new AuthorsOfBooks { AuthorId = _authors[1].AuthorId, Author = _authors[1], BookId = book3.BookId, Book = book3 }, new AuthorsOfBooks { AuthorId = _authors[2].AuthorId, Author = _authors[2], Book = book3, BookId = book3.BookId }, new AuthorsOfBooks { AuthorId = _authors[3].AuthorId, Author = _authors[3], Book = book3, BookId = book3.BookId } };
         book3.BookTags = new List<BookTags>() { new BookTags { TagId = _tags[1].TagId, Tag = _tags[1], BookId = book3.BookId, Book = book3 }, new BookTags { TagId = _tags[2].TagId, Tag = _tags[2], Book = book3, BookId = book3.BookId, } };

         Book book4 = new Book
         {
            BookId = 4,
            Category = _categories[0],
            CategoryId = _categories[0].CategoryId,
            Pages = 40,
            Description = "Desc4",
            Name = "Book4",
            Year = 2004,
            Language = _languages[1],
            LanguageId = _languages[1].LanguageId,
            ImageMimeType = _imageMimeType,
            ImageData = new byte[] { 4 }
         };
         book4.AuthorsOfBooks = new List<AuthorsOfBooks>() { new AuthorsOfBooks { AuthorId = _authors[3].AuthorId, Author = _authors[3], BookId = book3.BookId, Book = book3 } };
         book4.BookTags = new List<BookTags>() { new BookTags { TagId = _tags[0].TagId, Tag = _tags[0], BookId = book3.BookId, Book = book3 }, new BookTags { TagId = _tags[1].TagId, Tag = _tags[1], Book = book3, BookId = book3.BookId } };

         Book book5 = new Book
         {
            BookId = 5,
            Category = _categories[1],
            CategoryId = _categories[1].CategoryId,
            Pages = 50,
            Description = "Desc5",
            Name = "Book5",
            Year = 2005,
            Language = _languages[2],
            LanguageId = _languages[2].LanguageId,
            ImageMimeType = _imageMimeType,
            ImageData = new byte[] { 5 },
         };
         book5.AuthorsOfBooks = new List<AuthorsOfBooks>() { new AuthorsOfBooks { AuthorId = _authors[3].AuthorId, Author = _authors[3], BookId = book3.BookId, Book = book3 }, new AuthorsOfBooks { AuthorId = _authors[4].AuthorId, Author = _authors[4], BookId = book3.BookId, Book = book3 } };
         book5.BookTags = new List<BookTags>() { new BookTags { TagId = _tags[2].TagId, Tag = _tags[2], BookId = book3.BookId, Book = book3 } };

         return new List<Book> { book1, book2, book3, book4, book5 };
      }
      #endregion
      public FakeRepo()
      {
         _books = CreateBooks();
      }

      public DbContext Context => throw new NotImplementedException();

      public IEnumerable<Book> Books => _books;

      public IEnumerable<Author> Authors => _authors;

      public IEnumerable<Category> Categories => _categories;

      public IEnumerable<Language> Languages => _languages;

      public IEnumerable<Tag> Tags => _tags;

      public void AddAuthor(Author newAuthor)
      {
         _authors.ToList().Add(newAuthor);
      }

      public void AddBook(Book book)
      {
         _books.ToList().Add(book);
      }

      public void AddTag(Tag newTag)
      {
         _tags.ToList().Add(newTag);
      }

      public void Dispose()
      {
         throw new NotImplementedException();
      }

      public Book FindBook(int id)
      {
         return _books.Where(b => b.BookId == id).FirstOrDefault();
      }

      public void RemoveAuthor(Author author)
      {
         Author fromRepo = _authors.Where(a => a.AuthorId == author.AuthorId).FirstOrDefault();
         if (fromRepo != null)
         {
            _authors.Remove(fromRepo);
         }
      }

      public void RemoveBook(Book book)
      {
         Book fromRepo = _books.Where(a => a.BookId == book.BookId).FirstOrDefault();
         if (fromRepo != null)
         {
            _books.Remove(fromRepo);
         }
      }

      public void RemoveTag(Tag tag)
      {
         Tag fromRepo = _tags.Where(a => a.TagId == tag.TagId).FirstOrDefault();
         if (fromRepo != null)
         {
            _tags.Remove(fromRepo);
         }
      }

      async public Task SaveChangesAsync()
      {
         throw new NotImplementedException();
      }

      async public Task<IEnumerable<Book>> UpdateBooksFromFilterUsingRawSql(NameValueCollection queryString, string controller, string action)
      {
         return _books;
      }
   }
}
