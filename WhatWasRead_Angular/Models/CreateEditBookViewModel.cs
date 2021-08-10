using WhatWasRead_Angular.App_Data.DBModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace WhatWasRead_Angular.Models
{
   public class CreateEditBookViewModel
   {
      public int BookId { get; set; }
      public string Name { get; set; }
      public int Pages { get; set; }
      public string Description { get; set; }
      public int Year { get; set; }
      public string Base64ImageSrc { get; set; }
      public int SelectedLanguageId { get; set; }
      public int SelectedCategoryId { get; set; }
      public IEnumerable<int> SelectedAuthorsId { get; set; }
      public IEnumerable<int> SelectedTagsId { get; set; }

      public IEnumerable<Category> Categories { get; set; }
      public IEnumerable<Language> Languages { get; set; }
      public IEnumerable<Tag> Tags { get; set; }
      public IEnumerable<Author> Authors { get; set; }

      public string Validate(bool isCreate)
      {
         StringBuilder errors = new StringBuilder();
         if (!isCreate)
         {
            if (BookId < 1)
            {
               errors.AppendLine("Неверный идентификатор.");
            }
         }
         if (string.IsNullOrWhiteSpace(Name))
         {
            errors.AppendLine("Не указано название книги.");
         }
         else if (Name.Length < 4 || Name.Length > 100)
         {
            errors.AppendLine("Название книни должно состоять от 4 до 100 символов.");
         }

         if (Pages < 1 || Pages > 5000)
         {
            errors.AppendLine("Количество страниц должно быть в диапазоне от 1 до 5000.");
         }

         if (string.IsNullOrWhiteSpace(Description))
         {
            errors.AppendLine("Не указано описание книги.");
         }
         else if (Description.Length < 20 || Description.Length > 1000)
         {
            errors.AppendLine("Описание книни должно состоять от 20 до 1000 символов.");
         }

         if (Year < 1980 || Year > 2050)
         {
            errors.AppendLine("Год должен быть в диапазоне от 1980 до 2050.");
         }

         if (string.IsNullOrWhiteSpace(Base64ImageSrc))
         {
            errors.AppendLine("Не выбрана обложка книги.");
         }

         if (SelectedLanguageId < 1)
         {
            errors.AppendLine("Не указан язык.");
         }

         if (SelectedCategoryId < 1)
         {
            errors.AppendLine("Не указана категория.");
         }

         if (SelectedAuthorsId == null || SelectedAuthorsId.Count() < 1)
         {
            errors.AppendLine("Не указано авторство книги.");
         }
         return errors.ToString();
      }
   }

}