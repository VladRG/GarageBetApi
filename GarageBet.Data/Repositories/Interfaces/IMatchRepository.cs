using GarageBet.Domain.Tables;
using System.Collections.Generic;

namespace GarageBet.Data.Interfaces
{
    public interface IMatchRepository : IRepository<Match>
    {
        IEnumerable<Match> ListByChampionshipId(long id);

        IEnumerable<Match> ListAvailable();
    }
}
