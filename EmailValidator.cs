using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebLab
{
    /* public class EmailValidator : ValidationAttribute
     {
         LabOOPContext _context;

         public EmailValidator(LabOOPContext context)
         {
             _context = context;
         }

         public override bool IsValid(object value)
         {
             if(value != null)
             {
                 string stValue = value.ToString();

                 if(_context.Преподаватели.Any(e => e.Mail == stValue) || 
                    _context.Студенты.Any(f => f.Mail == stValue) ||
                    _context.Пользователь.Any(g => g.Mail == stValue))
                 {
                     return false;
                 }

                 return true;
             }

             return false;
         }
     }*/
}
