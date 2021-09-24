﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WSVentas.Models.Request
{
    public class AuthRequest
    {
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}