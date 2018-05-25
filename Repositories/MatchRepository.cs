using GarageBet.Api.Repository.Interfaces;
using GarageBet.Api.Database.Tables;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using GarageBet.Api.Models;
using GarageBet.Api.Database;

namespace GarageBet.Api.Repository.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        DataContext _context;

        public MatchRepository(DataContext context)
        {
            _context = context;
        }

        #region IMatchRepository
        public IEnumerable<MatchBetModel> ListMatchBets(long userId)
        {
            var matches = _context.Matches
                .Include(row => row.HomeTeam)
                .Include(row => row.AwayTeam)
                .Include(row => row.Bets)
                .Include(row => row.Championship).ToList();

            var result = new List<MatchBetModel>();
            foreach (var match in matches)
            {
                var bet = match.Bets.FirstOrDefault(b => b.UserId == userId);
                result.Add(new MatchBetModel
                {
                    MatchId = match.Id,
                    BetId = bet.Id,
                    ChampionshipId = match.Championship.Id,
                    HomeTeamName = match.HomeTeam.Name,
                    AwayTeamName = match.AwayTeam.Name,
                    DateTime = match.DateTime,
                    HomeBet = bet.HomeScore,
                    AwayBet = bet.AwayScore,
                    HomeScore = match.HomeScore,
                    AwayScore = match.AwayScore,
                    ChampionshipName = match.Championship.Name,
                    CompetitiveYear = match.Championship.CompetitiveYear,
                    BetState = GetBetState(match, userId, bet)
                });
            }
            return result.OrderBy(t => t.DateTime).ToList();
        }

        public IEnumerable<Match> ListByChampionshipId(long id)
        {
            return _context.Matches
                .Where(row => row.Championship.Id == id)
                .ToList();
        }

        public MatchModel FindForBet(long betId)
        {
            return _context.Matches.Select(match => new MatchModel
            {
                AwayScore = match.AwayScore,
                HomeScore = match.HomeScore,
                AwayTeamName = match.AwayTeam.Name,
                HomeTeamName = match.HomeTeam.Name,
                ChampionshipName = match.Championship.Name,
                DateTime = match.DateTime,
                CompetitiveYear = match.Championship.CompetitiveYear,
                Id = match.Id
            }).FirstOrDefault();
        }

        public IEnumerable<Match> ListAvailable()
        {
            return _context.Matches
                .Where(match => match.DateTime < DateTime.Now)
                .ToList();
        }

        public IEnumerable<MatchStats> GetMatchStats(long matchId)
        {
            List<MatchStats> stats = new List<MatchStats>();
            var bets = _context.Bets
                .Include(row => row.Match)
                .Include(row => row.User)
                .Where(row => row.MatchId == matchId).ToList();

            foreach (var item in bets)
            {
                stats.Add(new MatchStats
                {
                    HomeScore = item.HomeScore,
                    AwayScore = item.AwayScore,
                    User = new UserModel
                    {
                        Email = item.User.Email,
                        FirstName = item.User.FirstName,
                        LastName = item.User.LastName
                    },
                    BetState = GetBetState(item.Match, item.User.Id, item)
                });
            };

            return stats;
        }

        public MatchEditBetForm GetMatchModelForEditBet(long betId)
        {
            return _context.Bets.Where(bet => bet.Id == betId)
                   .Select(row => new MatchEditBetForm
                   {
                       AwayTeamName = row.Match.AwayTeam.Name,
                       HomeTeamName = row.Match.HomeTeam.Name,
                       MatchId = row.Match.Id
                   }).Single();
        }

        public MatchNewBetForm GetMatchModelForNewBet(long matchId)
        {
            return _context.Matches.Where(match => match.Id == matchId)
               .Select(row => new MatchNewBetForm
               {
                   AwayTeamName = row.AwayTeam.Name,
                   HomeTeamName = row.HomeTeam.Name,
                   MatchId = row.Id
               }).Single();
        }
        #endregion

        #region IRepository
        public Match Find(long id)
        {
            return _context.Matches
               .Include(row => row.Championship)
               .Include(row => row.HomeTeam)
               .Include(row => row.AwayTeam)
               .Single(row => row.Id == id);
        }

        public async Task<Match> FindAsync(long id)
        {
            return await _context.Matches.FindAsync(id);
        }

        public Match Add(Match entity)
        {
            _context.Matches.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task<Match> AddAsync(Match entity)
        {
            await _context.Matches.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public IEnumerable<Match> List()
        {
            return _context.Matches
                 .Include(row => row.Championship)
                 .Include(row => row.HomeTeam)
                 .Include(row => row.AwayTeam)
                 .ToList();
        }

        public async Task<List<Match>> ListAsync()
        {
            return await _context.Matches.ToListAsync();
        }

        public void Remove(Match entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public async void RemoveAsync(Match entity)
        {
            _context.Matches.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public Match Update(Match entity)
        {
            Match match = _context.Matches.Find(entity.Id);
            match.HomeScore = entity.HomeScore;
            match.AwayScore = entity.AwayScore;
            _context.Matches.Update(match);
            _context.SaveChanges();
            return entity;
        }

        public async Task<Match> UpdateAsync(Match entity)
        {
            _context.Matches.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        #endregion

        private BetModel GetBetModel(Match match, long userId)
        {
            Bet bet = match.Bets.FirstOrDefault(b => b.UserId == userId);
            if (bet == null)
            {
                return null;
            }
            return new BetModel
            {
                HomeScore = bet.HomeScore,
                AwayScore = bet.AwayScore
            };
        }

        private BetState GetBetState(Match match, long userId, Bet bet)
        {

            if (match.DateTime > DateTime.Now)
            {
                return BetState.CanBet;
            }

            if (match.DateTime < DateTime.Now && bet == null)
            {
                return BetState.NotAvailable;
            }

            if (DateTime.Now < match.DateTime.AddHours(2) && DateTime.Now > match.DateTime)
            {
                return BetState.NotAvailable;
            }

            if (match.HomeScore == bet.HomeScore && match.AwayScore == bet.AwayScore)
            {
                return BetState.Won;
            }

            if (
              (match.HomeScore < match.AwayScore && bet.HomeScore < bet.AwayScore) ||
              (match.HomeScore > match.AwayScore && bet.HomeScore > bet.AwayScore) ||
              (match.HomeScore == match.AwayScore && bet.HomeScore == bet.AwayScore))
            {
                return BetState.Result;
            }
            else
            {
                return BetState.Lost;
            }
        }
    }
}
