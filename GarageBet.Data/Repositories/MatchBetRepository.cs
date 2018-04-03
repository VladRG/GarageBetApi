using System;
using System.Collections.Generic;
using System.Linq;
using Database.Views;
using GarageBet.Data.Interfaces;
using GarageBet.Domain.Tables;

namespace GarageBet.Data.Repositories
{
    public class MatchBetRepository : IMatchBetRepository
    {
        DataContext _context;

        public MatchBetRepository(DataContext context)
        {
            _context = context;
        }

        #region IMatchBetRepository
        public IEnumerable<MatchBet> FindByUserId(long id)
        {
            IQueryable<MatchBet> source = _context.MatchBets;
            return source.GroupBy(row => row.UserId)
                .SelectMany(row => row)
                .Where(row => row.UserId == id)
                .ToList();
        }

        public IEnumerable<MatchBet> ListHistory(User user)
        {
            return _context.MatchBets.AsQueryable()
                 .Where(row => row.MatchDateTime > DateTime.Now)
                 .ToList();
        }
        #endregion

        #region IViewRepository
        public ICollection<MatchBet> List()
        {
            IQueryable<MatchBet> source = _context.MatchBets;
            return source.ToList();
        }

        public ICollection<MatchBet> ListAsync()
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
