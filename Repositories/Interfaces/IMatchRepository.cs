using GarageBet.Api.Models;
using GarageBet.Api.Database.Tables;
using System.Collections.Generic;

namespace GarageBet.Api.Repository.Interfaces
{
    public interface IMatchRepository : IRepository<Match>
    {
        IEnumerable<Match> ListByChampionshipId(long id);

        IEnumerable<Match> ListAvailable();

        IEnumerable<MatchBetModel> ListMatchBets(long userId);

        IEnumerable<MatchBetModel> GetForChampionship(long championshipId, long userId);

        IEnumerable<MatchBetModel> GetForToday(long userId);

        IEnumerable<MatchStats> GetMatchStats(long matchId);

        MatchModel FindForBet(long betId);

        MatchEditBetForm GetMatchModelForEditBet(long betId);

        MatchNewBetForm GetMatchModelForNewBet(long matchId);
    }
}
