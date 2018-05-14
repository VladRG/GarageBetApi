using GarageBet.Data.Models;
using GarageBet.Domain.Tables;
using System.Collections.Generic;

namespace GarageBet.Data.Interfaces
{
    public interface IBetRepository : IRepository<Bet>
    {
        BetFormModel GetModelForMatch(long matchId);

        BetFormModel GetModelForEdit(long betId);

        IEnumerable<MatchBetModel> GetAvailable(long userId);

        UserStats GetUserStat(long userId);

        IEnumerable<UserStats> GetUserStats();
    }
}
