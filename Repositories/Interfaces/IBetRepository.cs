using GarageBet.Api.Models;
using GarageBet.Api.Database.Tables;
using System.Collections.Generic;

namespace GarageBet.Api.Repository.Interfaces
{
    public interface IBetRepository : IRepository<Bet>
    {
        BetFormModel GetModelForMatch(long matchId);

        BetFormModel GetModelForEdit(long betId);

        IEnumerable<MatchBetModel> GetAvailable(long userId);

        UserStats GetUserStat(long userId);

        IEnumerable<UserStats> GetUserStats(int championshipId);
    }
}
