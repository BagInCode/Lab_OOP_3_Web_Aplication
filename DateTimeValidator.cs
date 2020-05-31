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
    public class DateTimeValidator : ValidationAttribute
    {
        public DateTimeValidator()
        {

        }

        public override bool IsValid(object value)
        {
            DateTime val = DateTime.Parse(value.ToString());

            if(val <= DateTime.Now || val > DateTime.Now.AddDays(30))
            {
                return false;
            }

            return true;
        }
    }
}
