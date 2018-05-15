
using GarageBet.Api.Database.Tables;

namespace GarageBet.Api.Repository.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User FindByEmail(string email);
    }
}
