using System;
using System.Collections.Generic;

namespace GarageBet.Domain.Tables
{
    public class User : EntityBase
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
        public DateTime LastLogin { get; set; }

        public Role Role { get; set; }

        public ICollection<Bet> Bets { get; set; }
    }
}
