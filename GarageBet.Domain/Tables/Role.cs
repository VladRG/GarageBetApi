
using Database.MM;
using System.Collections.Generic;

namespace GarageBet.Domain.Tables
{
    public class Role : EntityBase
    {
        public string Name { get; set; }

        public ICollection<UserRole> Users { get; set; }
    }
}
