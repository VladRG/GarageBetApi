
using Database.MM;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GarageBet.Domain.Tables
{
    public class Role : EntityBase
    {
        [Required]
        public string Name { get; set; }

        public ICollection<UserRole> Users { get; set; }
    }
}
