using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SeliseExam.CustomValidation
{
    public class EmailValidation : ValidationAttribute
    {
        private readonly string allowedDomain;

        public EmailValidation(string allowedDomain)
        {
            this.allowedDomain = allowedDomain;
        }
        public override bool IsValid(object value)
        {
            string[] words = value.ToString().Split('@');
            return words[1].ToUpper() == allowedDomain.ToUpper();
        }
    }
}
