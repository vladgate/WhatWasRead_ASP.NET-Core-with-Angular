using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatWasRead_Angular.Models
{
   public class CreateEditTagViewModel
   {
      public int TagId { get; set; }
      public string NameForLabels { get; set; }
      public string NameForLinks { get; set; }
      public string Validate(bool isCreate)
      {
         StringBuilder errors = new StringBuilder();
         if (!isCreate)
         {
            if (TagId < 1)
            {
               errors.AppendLine("Неверный идентификатор.");
            }
         }
         if (string.IsNullOrWhiteSpace(NameForLabels))
         {
            errors.AppendLine("Не указан текст представления тега.");
         }
         else if (NameForLabels.Length < 1 || NameForLabels.Length > 50)
         {
            errors.AppendLine("Текст представление тега должно состоять от 1 до 50 символов.");
         }

         if (string.IsNullOrWhiteSpace(NameForLinks))
         {
            errors.AppendLine("Не указан текст ссылки тега.");
         }
         else if (NameForLinks.Length < 1 || NameForLinks.Length > 50)
         {
            errors.AppendLine("Текст ссылки тега должен состоять от 1 до 50 символов.");
         }
         return errors.ToString();
      }
   }
}
