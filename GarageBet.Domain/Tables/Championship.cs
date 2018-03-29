using GarageBet.Domain.MM;
using System.Collections.Generic;

namespace GarageBet.Domain.Tables
{
    public class Championship : EntityBase
    {
        public string Name { get; set; }

        public ICollection<ChampionshipMatch> Matches { get; set; }
        public ICollection<ChampionshipTeam> Teams { get; set; }
    }
}
