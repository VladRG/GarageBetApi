
using GarageBet.Domain.MM;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GarageBet.Domain.Tables
{
    [Table("Teams")]
    public class Team : EntityBase
    {
        [MaxLength(50)]
        [Required(ErrorMessage = "Team name is required.")]
        public string Name { get; set; }

        public ICollection<ChampionshipTeam> Championships { get; set; }
    }
}
