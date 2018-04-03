using Database.MM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GarageBet.Domain.Tables
{
    public class User : EntityBase
    {
        [MaxLength(50)]
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Token { get; set; }

        public DateTime LastLogin { get; set; }

        [Required]
        public string Password { get; set; }

        public ICollection<Bet> Bets { get; set; }

        public ICollection<UserRole> Roles { get; set; }

    }

}
