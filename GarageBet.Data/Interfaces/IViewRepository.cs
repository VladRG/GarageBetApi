using Database.Views;
using System.Collections.Generic;

namespace GarageBet.Data.Interfaces
{
    public interface IViewRepository
    {
        ICollection<MatchBet> List();

        ICollection<MatchBet> ListAsync();
    }
}