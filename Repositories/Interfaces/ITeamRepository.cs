using GarageBet.Api.Models;
using GarageBet.Api.Database.Tables;
using System.Collections.Generic;

namespace GarageBet.Api.Repository.Interfaces
{

    public interface ITeamRepository : IRepository<Team>
    {
        IEnumerable<TeamModel> ListModels();

        IEnumerable<Team> ListForChampionship(long id);
    }
}
