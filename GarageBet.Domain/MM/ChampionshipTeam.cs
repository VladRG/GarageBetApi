using GarageBet.Domain.Tables;

namespace GarageBet.Domain.MM
{
    public class ChampionshipTeam
    {
        public long ChampionshipId { get; set; }
        public long TeamId { get; set; }

        public Championship Championship { get; set; }
        public Team Team { get; set; }
    }
}
