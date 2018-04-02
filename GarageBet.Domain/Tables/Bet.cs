
namespace GarageBet.Domain.Tables
{
    public class Bet : EntityBase
    {
        public User User { get; set; }

        public Match Match { get; set; }

        public short HomeScore { get; set; }

        public short AwayScore { get; set; }
    }
}
