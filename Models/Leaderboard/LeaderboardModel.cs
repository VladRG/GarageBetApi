
using System.Collections.Generic;

namespace GarageBet.Api.Models
{
    public class LeaderboardModel
    {
        public long Id { get; set; }

        public long Name { get; set; }

        public List<UserStats> Users { get; set; }

        public UserModel Admin { get; set; }
    }
}
