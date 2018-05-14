using GarageBet.Api.Models;
using GarageBet.Api.Database.Tables;
using System.Collections.Generic;

namespace GarageBet.Api.Repository.Interfaces
{
    public interface IChampionshipRepository : IRepository<Championship>
    {
        Championship AddTeam(Championship championship, Team team);

        Championship RemoveTeam(Championship championship, Team team);

        ChampionshipModel FindForEdit(long id);

        List<ChampionshipModel> ListModels();
    }
}
