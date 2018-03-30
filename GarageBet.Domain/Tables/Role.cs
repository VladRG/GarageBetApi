
using Database.MM;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GarageBet.Domain.Tables
{
    [Table("Roles")]
    public class Role : EntityBase
    {
        [MaxLength(15)]
        [Required(ErrorMessage = "Role name is required.")]
        public string Name { get; set; }

        public ICollection<UserRole> Users { get; set; }
    }
}
