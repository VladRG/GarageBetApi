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

        IEnumerable<MatchStats> GetMatchStats(long matchId);
    }
}
