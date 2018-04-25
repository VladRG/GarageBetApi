using GarageBet.Domain;

namespace GarageBet.Domain.Tables
{
    public class UserClaim : EntityBase
    {
        public long UserId { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }

        public User User { get; set; }
    }
}
