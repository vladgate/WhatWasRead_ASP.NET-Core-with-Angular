using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhatWasRead_Angular.App_Data.DBModels;
using WhatWasRead_Angular.App_Data.EF;
using WhatWasRead_Angular.App_Data;
using WhatWasRead_Angular.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace WhatWasRead_Angular.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   [Authorize]
   public class TagsController : ControllerBase
   {
      private readonly IRepository _repository;

      public TagsController(IRepository repository)
      {
         _repository = repository;
      }

      // GET: api/Authors
      [HttpGet]
      [AllowAnonymous]
      public IActionResult GetTags()
      {
         return new JsonResult(_repository.Tags.Select(t => new { TagId = t.TagId, NameForLabels = t.NameForLabels, NameForLinks = t.NameForLinks }).OrderBy(t => t.NameForLabels).ToList());
      }

      // GET: api/Tags/5
      [HttpGet("{id}")]
      [AllowAnonymous]
      public IActionResult GetTag(int id)
      {
         Tag t = _repository.Tags.Where(t => t.TagId == id).FirstOrDefault();

         if (t == null)
         {
            return NotFound();
         }

         return new JsonResult(new { TagId = t.TagId, NameForLabels = t.NameForLabels, NameForLinks = t.NameForLinks });
      }

      [HttpPut("{id}")]
      public async Task<IActionResult> PutTag([FromBody] CreateEditTagViewModel model)
      {
         string errors = model.Validate(isCreate: false);
         if (errors != "")
         {
            return new JsonResult(new { errors = errors });
         }
         Tag fromDb = _repository.Tags.Where(t => t.TagId == model.TagId).FirstOrDefault();
         if (fromDb == null)
         {
            return BadRequest();
         }
         fromDb.NameForLabels = model.NameForLabels;
         fromDb.NameForLinks = model.NameForLinks;
         try
         {
            await _repository.SaveChangesAsync();
         }
         catch (DbUpdateConcurrencyException)
         {
            if (!TagExists(model.TagId))
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

      [HttpPost]
      public async Task<IActionResult> PostTag([FromBody] CreateEditTagViewModel model)
      {
         string errors = model.Validate(isCreate: true);
         if (errors != "")
         {
            return new JsonResult(new { errors = errors });
         }
         Tag newTag = new Tag { NameForLabels = model.NameForLabels, NameForLinks = model.NameForLinks };
         _repository.AddTag(newTag);
         try
         {
            await _repository.SaveChangesAsync();
         }
         catch (Exception)
         {
            return new JsonResult(new { errors = "Возникла ошибка." });
         }
         return Ok(new { success = true, statuscode = "200", tagId = newTag.TagId });
      }

      // DELETE: api/Tags/5
      [HttpDelete("{id}")]
      public async Task<IActionResult> DeleteTag(int id)
      {
         Tag tag = _repository.Tags.Where(a => a.TagId == id).FirstOrDefault();
         if (tag == null)
         {
            return NotFound();
         }
         try
         {
            _repository.RemoveTag(tag);
            await _repository.SaveChangesAsync();
         }
         catch (DbUpdateException ex) when (ex.InnerException!=null && ex.InnerException.Message.Contains("DELETE statement conflicted with the REFERENCE constraint"))
         {
            return new JsonResult(new { errors = "С данным тегом имеются книги, поэтому сейчас удалить его нельзя." });
         }
         catch (Exception)
         {
            return new JsonResult(new { errors = "Возникла ошибка." });
         }

         return Ok(new { success = true, statuscode = "200" });
      }

      private bool TagExists(int id)
      {
         return _repository.Tags.Any(e => e.TagId == id);
      }
   }
}
