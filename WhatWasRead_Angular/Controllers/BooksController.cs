using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using WhatWasRead_Angular.Infrastructure;
using WhatWasRead_Angular.Models;
using WhatWasRead_Angular.App_Data.DBModels;
using WhatWasRead_Angular.App_Data;
using Microsoft.EntityFrameworkCore;

namespace WhatWasRead_Angular.Controllers
{
   public interface IBooksRequestManager
   {
      NameValueCollection GetQueryString(ControllerBase controller);
   }
   internal sealed class BooksRequestManager : IBooksRequestManager
   {
      public NameValueCollection GetQueryString(ControllerBase controller)
      {
         var dict = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(controller.Request.QueryString.Value);
         if (dict.Count > 0)
         {
            NameValueCollection queryString = new NameValueCollection(dict.Count);
            foreach (var item in dict)
            {
               queryString.Add(item.Key, item.Value);
            }
            return queryString;
         }
         else
         {
            return null;
         }
      }
   }

   [Route("api/[controller]")]
   [ApiController]
   public class BooksController : ControllerBase
   {
      private readonly IRepository _repository;
      private IBooksRequestManager _booksRequestManager;

      public BooksController(IRepository repo)
      {
         _repository = repo;
      }
      public IBooksRequestManager BooksRequestManager
      {
         get
         {
            if (_booksRequestManager == null)
            {
               _booksRequestManager = new BooksRequestManager();
            }
            return _booksRequestManager;
         }
         set
         {
            _booksRequestManager = value;
         }
      }

      [HttpGet("getImage/{id}")]
      public FileContentResult GetImage(int id)
      {
         Book book = _repository.Books.FirstOrDefault(p => p.BookId == id);
         if (book != null)
         {
            return File(book.ImageData, book.ImageMimeType);
         }
         else
         {
            return null;
         }
      }

      [HttpGet("list/{category}/page{page}")]
      async public Task<IActionResult> List(int page = 1, string category = "all", string tag = null)
      {
         string defaultCategory = "all";
         if (category == null)
         {
            category = defaultCategory;
         }
         Category currentCategory = null;
         Tag currentTag = null;
         if (category != defaultCategory)
         {
            currentCategory = _repository.Categories.Where(cat => cat.NameForLinks == category).FirstOrDefault();
            if (currentCategory == null) //category does not exist
            {
               return NotFound();
            }
         }
         if (tag != null)
         {
            currentTag = _repository.Tags.Where(x => x.NameForLinks == tag).FirstOrDefault();
            if (currentTag == null) //tag does not exist
            {
               return NotFound();
            }
         }

         RightPanelViewModel rightPanelModel = new RightPanelViewModel();
         LeftPanelViewModel leftPanelModel = new LeftPanelViewModel();
         BookListViewModel model = new BookListViewModel { LeftPanelData = leftPanelModel, RightPanelData = rightPanelModel };
         leftPanelModel.MinPagesExpected = _repository.Books.Count() > 0 ? _repository.Books.Select(b => b.Pages).Min() : 0;
         leftPanelModel.MaxPagesExpected = _repository.Books.Count() > 0 ? _repository.Books.Select(b => b.Pages).Max() : 0;
         leftPanelModel.MinPagesActual = leftPanelModel.MinPagesExpected;
         leftPanelModel.MaxPagesActual = leftPanelModel.MaxPagesExpected;

         IBooksRequestManager requestManager = this.BooksRequestManager;
         NameValueCollection query = requestManager.GetQueryString(this);
         if (query != null)
         {
            string[] queryKeys = query.AllKeys;
            if (queryKeys.Length > 0 && !(queryKeys.Length == 1 && (queryKeys[0] == "tag" || queryKeys[0] == "page" || queryKeys[0] == "category")))
            {
               await _repository.UpdateBooksFromFilterUsingRawSql(query, "books", "list");
            }
         }

         IEnumerable<Book> books = null;
         if (currentCategory == null) //all categories
         {
            if (currentTag == null) //all categories & all tags
            {
               books = _repository.Books
                   .OrderByDescending(b => b.BookId);
            }
            else //all categories & specific tag
            {
               books = _repository.Books.Where(b => b.BookTags.Where(bt => bt.TagId == currentTag.TagId).Count() > 0)
                   .OrderByDescending(b => b.BookId);
            }
         }
         else //specific category
         {
            books = _repository.Books
                .Where(b => b.CategoryId == currentCategory.CategoryId)
                .OrderByDescending(b => b.BookId);
         }

         if (query != null)
         {
            string pages = query["pages"];
            if (pages != null)
            {
               string[] ar = pages.Split('-');
               if (Int32.TryParse(ar[0], out int min))
               {
                  leftPanelModel.MinPagesActual = min;
               }
               if (Int32.TryParse(ar[1], out int max))
               {
                  leftPanelModel.MaxPagesActual = max;
               }
            }
         }

         leftPanelModel.Categories = _repository.Categories.OrderBy(c => c.NameForLabels).ToList();
         leftPanelModel.Tags = _repository.Tags.OrderBy(t => t.NameForLabels).ToList();
         leftPanelModel.Authors = _repository.Authors.Select(a => new AuthorViewModel
         {
            AuthorId = a.AuthorId,
            DisplayText = a.DisplayText,
            Link = Helper.CreateFilterPartOfLink(this.Request?.Path ?? "", query, LeftPanelViewModel.AuthorQueryWord, a.AuthorId.ToString(), out bool check),
            Checked = check
         }).OrderBy(a => a.DisplayText).ToList();
         leftPanelModel.Languages = _repository.Languages.Select(l => new LanguageViewModel
         {
            LanguageId = l.LanguageId,
            NameForLabels = l.NameForLabels,
            NameForLinks = l.NameForLinks,
            Link = Helper.CreateFilterPartOfLink(this.Request?.Path ?? "", query, LeftPanelViewModel.LanguageQueryWord, l.NameForLinks, out bool check),
            Checked = check
         }).OrderBy(l => l.NameForLabels).ToList();

         rightPanelModel.TotalPages = (int)Math.Ceiling((decimal)books.Count() / Globals.ITEMS_PER_PAGE);
         books = books.Skip((page - 1) * Globals.ITEMS_PER_PAGE).Take(Globals.ITEMS_PER_PAGE);

         if (books != null)
         {
            rightPanelModel.BookInfo = books.Select(b => new BookShortInfo { BookId = b.BookId, Name = b.Name, Authors = b.DisplayAuthors() }).ToList();
         }
         return new JsonResult(model);
      }

      [HttpGet("listAppend")]
      async public Task<IActionResult> ListAppend(int page = 1, string category = "all", string tag = null)
      {
         string defaultCategory = "all";
         if (category == null)
         {
            category = defaultCategory;
         }
         Category currentCategory = null;
         Tag currentTag = null;
         if (category != defaultCategory)
         {
            currentCategory = _repository.Categories.Where(cat => cat.NameForLinks == category).FirstOrDefault();
            if (currentCategory == null) //category does not exist
            {
               return new EmptyResult();
            }
         }
         if (tag != null)
         {
            currentTag = _repository.Tags.Where(x => x.NameForLinks == tag).FirstOrDefault();
            if (currentTag == null) //tag does not exist
            {
               return new EmptyResult();
            }
         }

         IBooksRequestManager requestManager = this.BooksRequestManager;
         NameValueCollection query = requestManager.GetQueryString(this);
         if (query != null)
         {
            string[] queryKeys = query.AllKeys;
            if (queryKeys.Length > 0 && !(queryKeys.Length == 1 && (queryKeys[0] == "tag" || queryKeys[0] == "page" || queryKeys[0] == "category")))
            {
               await _repository.UpdateBooksFromFilterUsingRawSql(query, "books", "list");
            }
         }
         IEnumerable<Book> books = null;
         if (currentCategory == null) //all categories
         {
            if (currentTag == null) //all categories & all tags
            {
               books = _repository.Books
                   .OrderByDescending(b => b.BookId);
            }
            else //all categories & specific tag
            {
               books = _repository.Books.Where(b => b.BookTags.Select(x => x.TagId).Contains(currentTag.TagId))
                   .OrderByDescending(b => b.BookId);
            }
         }
         else //specific category
         {
            books = _repository.Books
                .Where(b => b.CategoryId == currentCategory.CategoryId)
                .OrderByDescending(b => b.BookId);
         }
         books = books.Skip((page - 1) * Globals.ITEMS_PER_PAGE).Take(Globals.ITEMS_PER_PAGE);

         if (books == null || books.Count() == 0)
         {
            return new EmptyResult();
         }
         IEnumerable<BookShortInfo> rightPanelModel = books.Select(b => new BookShortInfo { BookId = b.BookId, Name = b.Name, Authors = b.DisplayAuthors() }).ToList();
         return  new JsonResult(rightPanelModel);
      }

      [HttpGet("details/{id}")]
      public IActionResult Details(int id)
      {
         Book book = _repository.FindBook(id);
         if (book == null)
         {
            return NotFound();
         }
         var model = new
         {
            BookId = book.BookId,
            Name = book.Name,
            Pages = book.Pages,
            Year = book.Year,
            Description = book.Description,
            Base64ImageSrc = String.Format("data:{0};base64,{1}", book.ImageMimeType, Convert.ToBase64String(book.ImageData)),
            AuthorsOfBooks = book.DisplayAuthors(),
            BookTags = book.BookTags.Select(bt => new { bt.Tag.NameForLabels, bt.Tag.NameForLinks }),
            Category = book.Category.NameForLabels,
            Language = book.Language.NameForLabels
         };
         return new JsonResult(model);
      }

      [HttpGet("create")]
      public IActionResult Create()
      {
         var model = new
         {
            Authors = _repository.Authors.OrderBy(a => a.DisplayText).ToList(),
            Tags = _repository.Tags.Select(t => new { t.TagId, t.NameForLabels }).OrderBy(t => t.NameForLabels).ToList(),
            Categories = _repository.Categories.Select(c => new { c.CategoryId, c.NameForLabels }).OrderBy(c => c.NameForLabels).ToList(),
            Languages = _repository.Languages.Select(l => new { l.LanguageId, l.NameForLabels }).OrderBy(l => l.NameForLabels).ToList()
         };
         return new JsonResult(model);
      }

      [HttpPost]
      public async Task<IActionResult> Create(CreateEditBookViewModel model)
      {
         string errors = model.Validate(isCreate: true);
         if (errors != "")
         {
            return new JsonResult(new { errors = errors });
         }

         Book book = new Book();
         string mimeType = Helper.GetMimeType(model.Base64ImageSrc, out byte[] imageData);
         if (mimeType != null)
         {
            book.ImageMimeType = mimeType;
            book.ImageData = imageData;
         }
         else
         {
            return new JsonResult(new { errors = "Изображение имеет неверный формат." });
         }

         ICollection<Author> authors = _repository.Authors.Where(x => model.SelectedAuthorsId.Contains(x.AuthorId)).ToList();
         if (authors.Count > 0)
         {
            foreach (Author author in authors)
            {
               book.AuthorsOfBooks.Add(new AuthorsOfBooks { Author = author, Book = book });
            }
         }
         if (model.SelectedTagsId?.Count() > 0)
         {
            ICollection<Tag> tags = _repository.Tags.Where(x => model.SelectedTagsId.Contains(x.TagId)).ToList();
            if (tags.Count > 0)
            {
               foreach (Tag tag in tags)
               {
                  book.BookTags.Add(new BookTags { Tag = tag, Book = book });
               }
            }
         }

         book.CategoryId = model.SelectedCategoryId;
         book.Description = model.Description;
         book.LanguageId = model.SelectedLanguageId;
         book.Name = model.Name;
         book.Pages = model.Pages;
         book.Year = model.Year;
         _repository.AddBook(book);
         try
         {
            await _repository.SaveChangesAsync();
         }
         catch (Exception)
         {
            return new JsonResult(new { errors = "Возникла ошибка." });
         }
         return Ok(new { success = true, statuscode = "200", bookId = book.BookId });
      }

      [HttpGet("edit/{id}")]
      public IActionResult Edit(int id)
      {
         Book book = _repository.FindBook(id);
         if (book == null)
         {
            return NotFound();
         }
         var model = new
         {
            BookId = book.BookId,
            Name = book.Name,
            Pages = book.Pages,
            Description = book.Description,
            Year = book.Year,
            Base64ImageSrc = String.Format("data:{0};base64,{1}", book.ImageMimeType, Convert.ToBase64String(book.ImageData)),
            SelectedLanguageId = book.LanguageId,
            SelectedCategoryId = book.CategoryId,
            SelectedAuthorsId = book.AuthorsOfBooks.Select(ab => ab.Author).Select(a => a.AuthorId).ToList(),
            SelectedTagsId = book.BookTags.Select(a => a.TagId).ToList(),

            Authors = _repository.Authors.Select(a => new { AuthorId = a.AuthorId, DisplayText = a.DisplayText }).OrderBy(b => b.DisplayText).ToList(),
            Tags = _repository.Tags.Select(t => new { TagId = t.TagId, NameForLabels = t.NameForLabels }).OrderBy(t => t.NameForLabels).ToList(),
            Categories = _repository.Categories.Select(c => new { CategoryId = c.CategoryId, NameForLabels = c.NameForLabels }).OrderBy(c => c.NameForLabels).ToList(),
            Languages = _repository.Languages.Select(l => new { LanguageId = l.LanguageId, NameForLabels = l.NameForLabels }).OrderBy(l => l.NameForLabels).ToList()
         };
         return new JsonResult(model);
      }

      [HttpPut]
      public async Task<IActionResult> Edit(CreateEditBookViewModel model)
      {
         string errors = model.Validate(isCreate: false);
         if (errors != "")
         {
            return new JsonResult(new { errors = errors });
         }

         Book book = _repository.Books.FirstOrDefault(x => x.BookId == model.BookId);
         if (book == null)
         {
            return BadRequest();
         }

         string mimeType = Helper.GetMimeType(model.Base64ImageSrc, out byte[] imageData);
         if (mimeType != null)
         {
            book.ImageMimeType = mimeType;
            book.ImageData = imageData;
         }
         else
         {
            return new JsonResult(new { errors = "Изображение имеет неверный формат." });
         }

         ICollection<Author> authors = _repository.Authors.Where(x => model.SelectedAuthorsId.Contains(x.AuthorId)).ToList();
         if (authors.Count > 0)
         {
            book.AuthorsOfBooks.Clear();
            foreach (Author author in authors)
            {
               book.AuthorsOfBooks.Add(new AuthorsOfBooks { Author = author, Book = book });
            }
         }
         book.BookTags.Clear();
         if (model.SelectedTagsId?.Count() > 0)
         {
            ICollection<Tag> tags = _repository.Tags.Where(x => model.SelectedTagsId.Contains(x.TagId)).ToList();
            if (tags.Count > 0)
            {
               foreach (Tag tag in tags)
               {
                  book.BookTags.Add(new BookTags { Tag = tag, Book = book });
               }
            }
         }

         book.CategoryId = model.SelectedCategoryId;
         book.Description = model.Description;
         book.LanguageId = model.SelectedLanguageId;
         book.Name = model.Name;
         book.Pages = model.Pages;
         book.Year = model.Year;
         try
         {
            await _repository.SaveChangesAsync();
         }
         catch (DbUpdateConcurrencyException)
         {
            if (!_repository.Books.Any(e => e.BookId == book.BookId))
            {
               return NotFound();
            }
            else
            {
               return new JsonResult(new { errors = "Данные были оновлены." });
            }
         }
         catch (Exception)
         {
            return new JsonResult(new { errors = "Возникла ошибка." });
         }
         return Ok(new { success = true, statuscode = "200" });
      }

      [HttpDelete("delete/{id}")]
      public async Task<IActionResult> Delete(int id)
      {
         Book book = _repository.FindBook(id);
         if (book == null)
         {
            return NotFound();
         }
         _repository.RemoveBook(book);
         try
         {
            await _repository.SaveChangesAsync();
         }
         catch (Exception)
         {
            return new JsonResult(new { errors = "Возникла ошибка." });
         }
         return Ok(new { success = true, statuscode = "200" });
      }
   }
}
