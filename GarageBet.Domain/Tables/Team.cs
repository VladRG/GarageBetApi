
using GarageBet.Domain.MM;
using System.Collections.Generic;

namespace GarageBet.Domain.Tables
{
    public class Team : EntityBase
    {
        public string Name { get; set; }

        public ICollection<ChampionshipTeam> Championships { get; set; }
    }
}
