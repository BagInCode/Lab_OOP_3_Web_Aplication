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
    public class GreatherThanZeroValidator : ValidationAttribute
    {
        public GreatherThanZeroValidator()
        {

        }

        public override bool IsValid(object value)
        {
            string str = value.ToString();

            int val = 0;

            for(int i = 0; i < str.Length; i++)
            {
                val = val * 10 + str[i] - '0';
            }

            if (val <= 0)
            {
                return false;
            }

            return true;
        }
    }
}
