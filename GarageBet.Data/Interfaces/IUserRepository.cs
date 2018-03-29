
using GarageBet.Domain.Tables;
using System.Collections.Generic;

namespace GarageBet.Data.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
    }
}
