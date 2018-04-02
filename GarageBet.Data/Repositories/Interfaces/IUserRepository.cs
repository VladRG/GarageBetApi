
using GarageBet.Domain.Tables;

namespace GarageBet.Data.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User FindByEmail(string email);
    }
}
