
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GarageBet.Domain.Tables
{
    [Table("Bets")]
    public class Bet : EntityBase
    {
        public User User { get; set; }

        public Match Match { get; set; }

        [Required(ErrorMessage = "Home Score is required.")]
        public short HomeScore { get; set; }

        [Required(ErrorMessage = "Away Score is required.")]
        public short AwayScore { get; set; }
    }
}
