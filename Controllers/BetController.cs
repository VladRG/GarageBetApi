using System;
using System.Collections.Generic;
using GarageBet.Api.Repository.Interfaces;
using GarageBet.Api.Models;
using GarageBet.Api.Database.Tables;
using Microsoft.AspNetCore.Mvc;

namespace GarageBet.Api.Controllers
{
    public class BetController : GbController
    {
        private IBetRepository _repository;

        public BetController(IBetRepository repository, IUserRepository userRepo) : base(userRepo)
        {
            _repository = repository;
        }

        [HttpGet("/bet", Name = "List Bets")]
        public IActionResult Index()
        {
            IEnumerable<MatchBetModel> bets;
            User user = GetUserFromAuthorizationHeader();
            try
            {
                bets = _repository.GetAvailable(user.Id);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Ok(bets);
        }

        [HttpGet("/bet/history", Name = "List Bet History")]
        public IActionResult GetBetHistory()
        {
            return Ok();
        }

        [HttpPost("/bet", Name = "Add Bet")]
        public IActionResult Add([FromBody]Bet bet)
        {
            User user = GetUserFromAuthorizationHeader();
            bet.UserId = user.Id;
            try
            {
                _repository.Add(bet);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Created(String.Format("/bet/{0}", bet.Id), null);
        }

        [HttpPut("/bet/{id}")]
        public IActionResult Update(long id, [FromBody]Bet bet)
        {
            User user = GetUserFromAuthorizationHeader();
            bet.UserId = user.Id;
            try
            {
                _repository.Update(bet);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Ok();
        }

        [HttpDelete("/bet/{id}")]
        public IActionResult Remove(long id)
        {
            Bet bet;
            try
            {
                bet = _repository.Find(id);
                _repository.Remove(bet);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Ok();
        }

        [HttpGet("/stats")]
        public IActionResult Stats()
        {
            User user = GetUserFromAuthorizationHeader();
            UserStats stat;
            try
            {
                stat = _repository.GetUserStat(user.Id);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Ok(stat);
        }

        [HttpGet("/leaderboard")]
        public IActionResult Leaderboard([FromQuery] int championshipId, [FromQuery] int page, [FromQuery] int pageSize)
        {
            IEnumerable<UserStats> stats;
            int count = 0;
            int position = 0;
            User user = GetUserFromAuthorizationHeader();

            try
            {
                stats = _repository.GetUserStats(championshipId, page, pageSize);
                if (page == 0)
                {
                    count = _repository.GetUserCount();
                    position = _repository.GetUserLeaderboardPosition(user.Email);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }

            return Ok(new
            {
                stats = stats,
                count = count,
                position = position
            });
        }
    }
}