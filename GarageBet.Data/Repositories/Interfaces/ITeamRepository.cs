using GarageBet.Domain.Tables;
using System.Collections.Generic;

namespace GarageBet.Data.Interfaces
{
    public interface ITeamRepository : IRepository<Team>
    {
        IEnumerable<Team> ListForChampionship(long id);
    }
}
