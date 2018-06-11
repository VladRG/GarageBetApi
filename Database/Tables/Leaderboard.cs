using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GarageBet.Api.Database.Tables
{
    public class Leaderboard : EntityBase
    {
        [Required]
        public string Name { get; set; }

        public long AdminId { get; set; }

        public ICollection<LeaderboardUser> Users { get; set; }

        public User Admin { get; set; }
    }
}