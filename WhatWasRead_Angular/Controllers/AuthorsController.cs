using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using WhatWasRead_Angular.App_Data;
using WhatWasRead_Angular.Models;
using WhatWasRead_Angular.App_Data.DBModels;
using Microsoft.AspNetCore.Authorization;

namespace WhatWasRead_Angular.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   [Authorize]

   public class AuthorsController : ControllerBase
   {
      private readonly IRepository _repository;

      public AuthorsController(IRepository repository)
      {
         _repository = repository;
      }

      // GET: api/Authors
      [HttpGet]
      [AllowAnonymous]
      public IActionResult GetAuthors()
      {
         return new JsonResult(_repository.Authors.Select(a => new { AuthorId = a.AuthorId, FirstName = a.FirstName, LastName = a.LastName }).OrderBy(a=>a.LastName).ToList());
      }

      // GET: api/Authors/5
      [HttpGet("{id}")]
      [AllowAnonymous]
      public IActionResult GetAuthor(int id)
      {
         var author = _repository.Authors.Where(a => a.AuthorId == id).FirstOrDefault();

         if (author == null)
         {
            return NotFound();
         }

         return new JsonResult(new { AuthorId = author.AuthorId, FirstName = author.FirstName, LastName = author.LastName });
      }

      [HttpPost]
      public async Task<IActionResult> PostAuthor([FromBody] CreateEditAuthorViewModel model)
      {
         string errors = model.Validate(isCreate: true);
         if (errors != "")
         {
            return new JsonResult(new { errors = errors });
         }
         Author newAuthor = new Author { FirstName = model.FirstName, LastName = model.LastName };
         _repository.AddAuthor(newAuthor);
         try
         {
            await _repository.SaveChangesAsync();
         }
         catch (Exception)
         {
            return new JsonResult(new { errors = "Возникла ошибка." });
         }
         return Ok(new { success = true, statuscode = "200", authodId = newAuthor.AuthorId });
      }

      [HttpPut("{id}")]
      public async Task<IActionResult> PutAuthor([FromBody] CreateEditAuthorViewModel model)
      {
         string errors = model.Validate(isCreate: false);
         if (errors != "")
         {
            return new JsonResult(new { errors = errors });
         }
         Author fromDb = _repository.Authors.Where(a => a.AuthorId == model.AuthorId).FirstOrDefault();
         if (fromDb == null)
         {
            return BadRequest();
         }
         fromDb.FirstName = model.FirstName;
         fromDb.LastName = model.LastName;
         try
         {
            await _repository.SaveChangesAsync();
         }
         catch (DbUpdateConcurrencyException)
         {
            if (!AuthorExists(model.AuthorId))
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


      // DELETE: api/Authors/5
      [HttpDelete("{id}")]
      public async Task<IActionResult> DeleteAuthor(int id)
      {
         var author = _repository.Authors.Where(a => a.AuthorId == id).FirstOrDefault();
         if (author == null)
         {
            return NotFound();
         }
         try
         {
            _repository.RemoveAuthor(author);
            await _repository.SaveChangesAsync();
         }
         catch (DbUpdateException ex) when (ex.InnerException!=null && ex.InnerException.Message.Contains("DELETE statement conflicted with the REFERENCE constraint"))
         {
            return new JsonResult(new { errors = "У даного автора имеются книги, поэтому сейчас удалить его нельзя." });
         }
         catch (Exception)
         {
            return new JsonResult(new { errors = "Возникла ошибка." });
         }

         return Ok(new { success = true, statuscode = "200" });
      }

      private bool AuthorExists(int id)
      {
         return _repository.Authors.Any(e => e.AuthorId == id);
      }
   }
}
