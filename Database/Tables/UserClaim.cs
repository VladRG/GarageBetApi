using GarageBet.Api;

namespace GarageBet.Api.Database.Tables
{
    public class UserClaim : EntityBase
    {
        public long UserId { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }

        public User User { get; set; }
    }
}
