﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeliseExam.Model
{
    public class AppUser: IdentityUser
    {
        public string Name { get; set; }
    }
}