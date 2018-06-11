using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GarageBet.Api.Database.Tables;
using GarageBet.Api.Models;
using GarageBet.Api.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GarageBet.Api.Controllers
{
    public class LeaderboardController : GbController
    {
        private ILeaderboardRepository _repository;

        public LeaderboardController(ILeaderboardRepository repository, IUserRepository userRepo) : base(userRepo)
        {
            _repository = repository;
        }

        [HttpGet("/leaderboard")]
        public IActionResult Leaderboard([FromQuery] int page, [FromQuery] int pageSize)
        {
            IEnumerable<UserStats> stats;
            int count = 0;
            int position = 0;
            User user = GetUserFromAuthorizationHeader();

            try
            {
                stats = _repository.GetUserStats(page, pageSize);
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

        public IActionResult AcceptInvite(long group, [FromBody] Boolean accept)
        {
            User user = GetUserFromAuthorizationHeader();
            try
            {
                if (accept)
                {
                    this._repository.AcceptInvite(user, group);
                }
                else
                {
                    this._repository.DeclineInvite(user, group);
                }
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
    }
}