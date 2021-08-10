using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatWasRead_Angular.Models
{
   public class CreateEditAuthorViewModel
   {
      public int AuthorId { get; set; }
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public string Validate(bool isCreate)
      {
         StringBuilder errors = new StringBuilder();
         if (!isCreate)
         {
            if (AuthorId < 1)
            {
               errors.AppendLine("Неверный идентификатор.");
            }
         }
         if (string.IsNullOrWhiteSpace(FirstName))
         {
            errors.AppendLine("Не указано имя автора.");
         }
         else if (FirstName.Length < 2 || FirstName.Length > 30)
         {
            errors.AppendLine("Имя автора должно состоять от 2 до 30 символов.");
         }

         if (string.IsNullOrWhiteSpace(LastName))
         {
            errors.AppendLine("Не указана фамилия автора.");
         }
         else if (LastName.Length < 2 || LastName.Length > 30)
         {
            errors.AppendLine("Фамилия автора должно состоять от 2 до 30 символов.");
         }
         return errors.ToString();
      }
   }
}
