using GarageBet.Data.Models;
using GarageBet.Domain.Tables;
using System.Collections.Generic;

namespace GarageBet.Data.Interfaces
{
    public interface IBetRepository : IRepository<Bet>
    {
        BetModel GetModelForMatch(long matchId);

        BetModel GetModelForEdit(long betId);

        IEnumerable<BetModel> GetAvailable(long userId);
    }
}
