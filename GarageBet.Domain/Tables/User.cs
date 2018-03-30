using Database.MM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GarageBet.Domain.Tables
{
    [Table("Users")]
    public class User : EntityBase
    {
        [MaxLength(50)]
        [Required(ErrorMessage = "Email address is required.")]
        public string Email { get; set; }

        [MaxLength(40)]
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [MaxLength(40)]
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [MaxLength(250)]
        public string Token { get; set; }

        public DateTime LastLogin { get; set; }

        public Role Role { get; set; }

        public ICollection<Bet> Bets { get; set; }

        public ICollection<UserRole> Roles { get; set; }
    }
}
