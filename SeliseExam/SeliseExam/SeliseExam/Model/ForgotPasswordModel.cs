using SeliseExam.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SeliseExam.Model
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        [EmailValidation(allowedDomain: "yopmail.com", ErrorMessage = "Email domain must be yopmail.com")]
        public string Email { get; set; }
    }
}
