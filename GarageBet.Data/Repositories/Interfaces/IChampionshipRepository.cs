using GarageBet.Domain.Tables;

namespace GarageBet.Data.Interfaces
{
    public interface IChampionshipRepository : IRepository<Championship>
    {
        Championship AddTeam(Championship championship, Team team);

        Championship RemoveTeam(Championship championship, Team team);
    }
}
