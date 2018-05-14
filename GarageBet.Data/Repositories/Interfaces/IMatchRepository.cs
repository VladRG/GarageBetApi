using GarageBet.Data.Models;
using GarageBet.Domain.Tables;
using System.Collections.Generic;

namespace GarageBet.Data.Interfaces
{
    public interface IMatchRepository : IRepository<Match>
    {
        IEnumerable<Match> ListByChampionshipId(long id);

        IEnumerable<Match> ListAvailable();

        IEnumerable<MatchBetModel> ListMatchBets(long userId);

        IEnumerable<MatchStats> GetMatchStats(long matchId);
    }
}
