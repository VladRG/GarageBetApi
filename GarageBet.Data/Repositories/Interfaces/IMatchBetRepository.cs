using Database.Views;
using GarageBet.Domain.Tables;
using System.Collections.Generic;

namespace GarageBet.Data.Interfaces
{
    public interface IMatchBetRepository : IViewRepository
    {
        IEnumerable<MatchBet> FindByUserId(long id);

        IEnumerable<MatchBet> ListHistory(User user);
    }
}
