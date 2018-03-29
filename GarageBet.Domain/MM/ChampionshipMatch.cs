using GarageBet.Domain.Tables;

namespace GarageBet.Domain.MM
{
    public class ChampionshipMatch
    {
        public long ChampionshipId { get; set; }
        public long MatchId { get; set; }

        public Championship Championship { get; set; }
        public Match Match { get; set; }
    }
}
