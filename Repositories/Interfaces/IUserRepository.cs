
using GarageBet.Api.Database.Tables;
using GarageBet.Api.Models;
using System.Collections.Generic;

namespace GarageBet.Api.Repository.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User FindByEmail(string email);

        List<UserModel> GetUsers();
    }
}
