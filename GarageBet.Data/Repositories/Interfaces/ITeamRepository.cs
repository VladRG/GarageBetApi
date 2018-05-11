using GarageBet.Data.Models;
using GarageBet.Domain.Models;
using GarageBet.Domain.Tables;
using System.Collections.Generic;

namespace GarageBet.Data.Interfaces
{

    public interface ITeamRepository : IRepository<Team>
    {
        IEnumerable<TeamModel> ListModels();

        IEnumerable<Team> ListForChampionship(long id);
    }
}
