using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GarageBet.Api.Database.Tables
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

        public ICollection<UserClaim> Claims { get; set; }

        public ICollection<LeaderboardUser> Leaderboards { get; set; }

        public ICollection<Leaderboard> ManagedLeaderboards { get; set; }

    }

}
