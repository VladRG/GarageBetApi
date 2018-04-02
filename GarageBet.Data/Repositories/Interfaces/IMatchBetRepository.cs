using Database.Views;
using System.Collections.Generic;

namespace GarageBet.Data.Interfaces
{
    public interface IMatchBetRepository : IViewRepository
    {
        IEnumerable<MatchBet> FindByUserId(long id);
    }
}
