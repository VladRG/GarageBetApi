
namespace GarageBet.Domain.Tables
{
    public class Bet : EntityBase
    {
        public User User { get; set; }
        public Match Match { get; set; }
    }
}
