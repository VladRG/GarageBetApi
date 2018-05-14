
using GarageBet.Api.Database.Tables;

namespace GarageBet.Api.Database
{
    public class ChampionshipTeam
    {
        public long ChampionshipId { get; set; }

        public long TeamId { get; set; }

        public Championship Championship { get; set; }

        public Team Team { get; set; }

        public int Win { get; set; }

        public int Draw { get; set; }

        public int Lost { get; set; }
    }
}
