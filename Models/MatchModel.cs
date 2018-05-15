using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageBet.Api.Models
{
    public class MatchModel
    {
        public long Id { get; set; }

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }

        public TeamModel HomeTeam { get; set; }

        public TeamModel AwayTeam { get; set; }

        public ChampionshipModel Championship { get; set; }

        public DateTime DateTime { get; set; }
    }
}
