using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageBet.Api.Models
{
    public class LeaderboardSummaryModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public UserModel Owner { get; set; }

    }
}
