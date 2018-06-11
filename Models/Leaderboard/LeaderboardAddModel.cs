using System.Collections.Generic;

namespace GarageBet.Api.Models
{
    public class LeaderboardAddModel
    {
        public string Name { get; set; }

        public long AdminId { get; set; }

        public IList<UserModel> Users { get; set; }
    }
}
