using GarageBet.Data.Models;
using GarageBet.Domain.Tables;
using System.Collections.Generic;

namespace GarageBet.Data.Interfaces
{
    public interface IChampionshipRepository : IRepository<Championship>
    {
        Championship AddTeam(Championship championship, Team team);

        Championship RemoveTeam(Championship championship, Team team);

        ChampionshipModel FindForEdit(long id);

        List<ChampionshipModel> ListModels();
    }
}
