
using System.Collections.Generic;

namespace GarageBet.Data.Models
{
    public class ChampionshipModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string CompetitiveYear { get; set; }

        public List<TeamModel> Teams { get; set; }
    }
}
