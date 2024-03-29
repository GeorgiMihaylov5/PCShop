﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PCShop.Entities
{
    public class User : IdentityUser
    {
        [MaxLength(30)]
        public string FirstName { get; set; }

        [MaxLength(30)]
        public string LastName { get; set; }


        [Range(14,120)]
        public int? Age { get; set; }
        public DateTime RegisterDate { get; set; }


        public ICollection<Order> Orders { get; set; }
        public bool IsAdministrator { get; set; }
    }
}
