using WhatWasRead_Angular.App_Data.DBModels;
using WhatWasRead_Angular.App_Data.EF;
using WhatWasRead_Angular.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatWasRead_Angular.App_Data
{
   public class EFRepository : IRepository
   {
      private readonly WhatWasReadContext _context;
      private IEnumerable<Book> _books;
      private bool _filterApplied;
      public EFRepository(WhatWasReadContext context)
      {
         _context = context;
      }

      public DbContext Context { get { return _context; } }

      public IEnumerable<Author> Authors
      {
         get
         {
            return _context.Authors.ToList();
         }
      }

      public IEnumerable<Book> Books
      {
         get
         {
            if (_filterApplied)
            {
               return _books;
            }
            else
            {
               _books = _context.Books.Include(b => b.Category).Include(b => b.Language).Include(b => b.AuthorsOfBooks).ThenInclude(ab => ab.Author).Include(b => b.BookTags).ThenInclude(bt => bt.Tag).ToList();
            }
            return _books;
         }
         private set
         {
            _books = value;
         }
      }

      public IEnumerable<Category> Categories
      {
         get
         {
            return _context.Categories.ToList();
         }
      }

      public IEnumerable<Language> Languages
      {
         get
         {
            return _context.Languages.ToList();
         }
      }
      public IEnumerable<Tag> Tags
      {
         get
         {
            return _context.Tags.ToList();
         }
      }
      public void AddAuthor(Author newAuthor)
      {
         _context.Authors.Add(newAuthor);
      }

      public void AddBook(Book book)
      {
         _context.Books.Add(book);
      }

      public void AddTag(Tag newTag)
      {
         _context.Tags.Add(newTag);
      }

      public void Dispose()
      {
         _context.Dispose();
      }

      public Book FindBook(int id)
      {
         return Books.Where(b => b.BookId == id).FirstOrDefault();
      }

      public void RemoveAuthor(Author author)
      {
         _context.Authors.Remove(author);
      }

      public void RemoveBook(Book book)
      {
         _context.Books.Remove(book);
      }

      public void RemoveTag(Tag tag)
      {
         _context.Tags.Remove(tag);
      }

      async public Task SaveChangesAsync()
      {
         await _context.SaveChangesAsync();
      }

      async public Task<IEnumerable<Book>> UpdateBooksFromFilterUsingRawSql(NameValueCollection queryString, string controller, string action)
      {
         List<Book> result = new List<Book>();
         try
         {
            string connectionstring = _context.Config[Globals.CONNECTION_NAME_CONFIG];
            SqlConnection connection = new SqlConnection(connectionstring);
            connection.Open();
            FilterTargetInfo filterTargetInfo = GetFilterTargetInfo(connection, controller, action);
            if (filterTargetInfo == null)
            {
               return result; // empty
            }

            Dictionary<string, QueryColumn> dict = GetFilterInfo(filterTargetInfo.FilterTargetId, connection, queryString.AllKeys);

            //get main part on query
            string sqlMainPart = $"Select * from {filterTargetInfo.FilterTargetSchema}.{filterTargetInfo.FilterTargetName} ";
            string sqlWherePart = BuildWherePartOfQuery(queryString, dict, filterTargetInfo);
            if (string.IsNullOrEmpty(sqlWherePart))
            {
               return result; // not such filters
            }
            string fullQueryText = sqlMainPart + sqlWherePart;

            SqlCommand cmdResult = new SqlCommand(fullQueryText, connection);
            using (SqlDataReader dataReader = await cmdResult.ExecuteReaderAsync())
            {
               while (await dataReader.ReadAsync())
               {
                  int id = (int)dataReader["BookId"];
                  Book book = result.Where(b => b.BookId == id).FirstOrDefault();
                  Author author;
                  Tag tag;
                  if (book != null) //book in list already exist
                  {
                     int authorId = (int)dataReader["AuthorId"];
                     if (book.AuthorsOfBooks.Select(ab => ab.Author).Where(a => a.AuthorId == authorId).FirstOrDefault() == null)
                     {
                        author = new Author { AuthorId = authorId, LastName = (string)dataReader["LastName"], FirstName = (string)dataReader["FirstName"] };
                        book.AuthorsOfBooks.Add(new AuthorsOfBooks { Author = author, Book = book });
                     }
                     int tagId = (int)dataReader["TagId"];
                     if (book.BookTags.Select(bt => bt.Tag).Where(a => a.TagId == tagId).FirstOrDefault() == null)
                     {
                        tag = new Tag { TagId = tagId, NameForLabels = (string)dataReader["TagNameForLabels"], NameForLinks = (string)dataReader["TagNameForLinks"] };
                        book.BookTags.Add(new BookTags { Tag = tag, TagId = tag.TagId, Book = book, BookId = book.BookId });
                     }
                     continue;
                  }
                  else
                  {
                     book = new Book();
                  }
                  book.BookId = id;
                  book.Name = (string)dataReader["Name"];
                  book.LanguageId = (int)dataReader["LanguageId"];
                  book.Pages = (int)dataReader["Pages"];
                  book.Description = (string)dataReader["Description"];
                  book.CategoryId = (int)dataReader["CategoryId"];
                  book.Year = (int)dataReader["Year"];
                  book.ImageData = (byte[])dataReader["ImageData"];
                  book.ImageMimeType = (string)dataReader["ImageMimeType"];
                  author = new Author { AuthorId = (int)dataReader["AuthorId"], LastName = (string)dataReader["LastName"], FirstName = (string)dataReader["FirstName"] };
                  book.AuthorsOfBooks.Add(new AuthorsOfBooks { Author = author, Book = book });
                  if (!(dataReader["TagId"] is DBNull))
                  {
                     tag = new Tag { TagId = (int)dataReader["TagId"], NameForLabels = (string)dataReader["TagNameForLabels"], NameForLinks = (string)dataReader["TagNameForLinks"] };
                     book.BookTags.Add(new BookTags { Tag = tag, Book = book, TagId = tag.TagId, BookId = book.BookId });
                  }
                  result.Add(book);
               }
            }
         }
         catch (Exception ex)
         {
#if DEBUG
            throw ex;
#endif
            return result; // empty
         }
         _filterApplied = true;
         this.Books = result;
         return result;
      }
      private Dictionary<string, QueryColumn> GetFilterInfo(int filterTargetId, SqlConnection connection, string[] queryWords)
      {
         Dictionary<string, QueryColumn> dict = new Dictionary<string, QueryColumn>(); // key - queryWord
         string qwords = FormatQueryWords(queryWords);
         string q = $"Select f.QueryWord, f.FilterColumnName, f.Comparator from [dbo].[Filters] as f where f.FilterTargetId = {filterTargetId} and f.QueryWord in ({qwords})";
         SqlCommand cmd2 = new SqlCommand(q, connection);
         using (SqlDataReader dr = cmd2.ExecuteReader())
         {
            while (dr.Read())
            {
               ComparatorType comparator = (ComparatorType)Enum.Parse(typeof(ComparatorType), (string)dr["Comparator"], true);
               dict.Add(dr["QueryWord"].ToString(), new QueryColumn(dr["FilterColumnName"].ToString(), comparator));
            }
         }
         return dict;
      }

      private string FormatQueryWords(string[] queryWords)
      {
         if (queryWords.Length == 1)
         {
            return '\'' + queryWords[0] + '\'';
         }
         StringBuilder result = new StringBuilder();
         for (int i = 0; i < queryWords.Length; i++)
         {
            result.Append('\''); // open '
            result.Append(queryWords[i]);
            result.Append('\''); // close '
            result.Append(',');
         }
         if (result.Length > 2)
         {
            result.Remove(result.Length - 1, 1); // remove last ','
         }
         return result.ToString();
      }

      private FilterTargetInfo GetFilterTargetInfo(SqlConnection connection, string controller, string action)
      {
         FilterTargetInfo filterTargetInfo = null;
         string q = $"Select top (1) f.FilterTargetId, f.FilterTargetSchema, f.FilterTargetName from [dbo].[FilterTargets] as f where f.ControllerName = @controller And f.ActionName = @action order by f.FilterTargetId";
         SqlParameter p1 = new SqlParameter("@controller", controller);
         SqlParameter p2 = new SqlParameter("@action", action);
         SqlCommand cmd = new SqlCommand(q, connection);
         cmd.Parameters.AddRange(new[] { p1, p2 });

         using (SqlDataReader dr = cmd.ExecuteReader())
         {
            if (dr.HasRows)
            {
               dr.Read();
               filterTargetInfo = new FilterTargetInfo
               {
                  FilterTargetSchema = (string)dr["FilterTargetSchema"],
                  FilterTargetName = (string)dr["FilterTargetName"],
                  FilterTargetId = (int)dr["FilterTargetId"]
               };
            }
         }
         return filterTargetInfo;
      }

      private string BuildWherePartOfQuery(NameValueCollection queryString, Dictionary<string, QueryColumn> columnData, FilterTargetInfo filterTargetInfo)
      {
         string keyField = "BookId";
         if (columnData.Count == 0)
         {
            return "";
         }
         StringBuilder sb = new StringBuilder();
         sb.Append($"WHERE {keyField} IN (SELECT {keyField} FROM {filterTargetInfo.FilterTargetSchema}.{filterTargetInfo.FilterTargetName} WHERE ");
         int index = 0;
         foreach (KeyValuePair<string, QueryColumn> colData in columnData)
         {
            string operatorPart = CreateQueryOperatorPart(queryString, colData);
            if (String.IsNullOrEmpty(operatorPart))
            {
               sb.Remove(sb.Length - 5, 5); //remove ' AND '
            }
            sb.Append(operatorPart);
            if (index != columnData.Count - 1) // not last
            {
               sb.Append(" AND ");
            }
            index++;
         }
         sb.Append(")");
         return sb.ToString();
      }

      private string CreateQueryOperatorPart(NameValueCollection queryString, KeyValuePair<string, QueryColumn> colData)
      {
         StringBuilder sb = new StringBuilder();
         string query = queryString[colData.Key];
         string[] q = query.Split(',');
         string comparator;
         switch (colData.Value.Comparator)
         {
            case ComparatorType.Between:
               comparator = " BETWEEN ";
               break;
            default:
               comparator = "=";
               break;
         }

         bool needParentheses = q.Length > 1 && colData.Value.Comparator == ComparatorType.Equal;
         bool startParentheses = false;
         for (int i = 0; i < q.Length; i++)
         {
            if (needParentheses)
            {
               if (!startParentheses)
               {
                  sb.Append('(');
                  startParentheses = true;
               }
            }

            sb.Append(colData.Value.ColumnName); // db column name
            sb.Append(comparator);
            if (colData.Value.Comparator == ComparatorType.Between)
            {
               int indexOfHyphen = q[i].IndexOf('-');
               bool success1 = int.TryParse(q[i].Substring(0, indexOfHyphen), out int operand1);
               bool success2 = int.TryParse(q[i].Substring(indexOfHyphen + 1), out int operand2);
               if (success1 && success2 && operand2 != 0) // not allowed when second operand equals to 0
               {
                  sb.Append(operand1);
                  sb.Append(" AND ");
                  sb.Append(operand2);
               }
               else
               {
                  return ""; // return empty if second operand equals to 0 - incorrect
               }
            }
            else if (colData.Value.Comparator == ComparatorType.Equal)
            {
               sb.Append('\''); // open '
               sb.Append(q[i]);
               sb.Append('\''); // close '
            }
            if (i != q.Length - 1) //last elem in array
            {
               sb.Append(" OR ");
            }
         }
         if (startParentheses)
         {
            sb.Append(')');
         }
         return sb.ToString();
      }
   }
}
internal class FilterTargetInfo
{
   public int FilterTargetId { get; set; }
   public string FilterTargetSchema { get; set; }
   public string FilterTargetName { get; set; }
}
internal class FilterInfo
{
   public string QueryWord { get; set; }
   public string FilterColumnName { get; set; }
   public string Comparator { get; set; }
}


