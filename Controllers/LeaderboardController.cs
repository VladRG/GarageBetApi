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
        public IActionResult Leaderboard([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] int group)
        {
            IEnumerable<UserStats> stats;
            int count = 0;
            int position = 0;
            User user = GetUserFromAuthorizationHeader();

            try
            {
                stats = _repository.GetUserStats(page, pageSize, group);
                if (page == 0)
                {
                    count = _repository.GetUserCount(group);
                    position = _repository.GetUserLeaderboardPosition(user.Email, group);
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

        [HttpGet("leaderboard/group")]
        public IActionResult GetUserLeaderboards([FromQuery] int page, [FromQuery] int pageSize)
        {
            User user = GetUserFromAuthorizationHeader();
            List<LeaderboardSummaryModel> leaderboards = new List<LeaderboardSummaryModel>();
            try
            {
                leaderboards = _repository.GetLeaderboarSummaries(user.Id).ToList();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Ok(leaderboards);
        }

        [HttpGet("leaderboard/edit/{group}")]
        public IActionResult GetLeaderboardForEdit(long group)
        {
            LeaderboardAddModel leaderboard;
            try
            {
                leaderboard = _repository.GetLeaderboardForEdit(group);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Ok(leaderboard);
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

        [HttpPost("/leaderboard")]
        public IActionResult AddLeaderboard([FromBody] LeaderboardAddModel leaderboard)
        {
            User user = GetUserFromAuthorizationHeader();
            leaderboard.AdminId = user.Id;
            leaderboard.Users.Add(new UserModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            });
            try
            {
                _repository.Add(leaderboard);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Created("", "");
        }

        [HttpPut("/leaderboard/{group}")]
        public IActionResult UpdateLeaderboard(long group, [FromBody] LeaderboardAddModel leaderboard)
        {
            User user = GetUserFromAuthorizationHeader();
            try
            {
                _repository.Update(leaderboard);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Created("", "");
        }

        [HttpPut("leaderboard/leave/{id}")]
        public IActionResult LeaveLeaderboard(long id)
        {
            User user = GetUserFromAuthorizationHeader();
            try
            {
                _repository.LeaveLeaderboard(user.Id, id);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }

            return Ok();
        }

        [HttpDelete("leaderboard/{id}")]
        public IActionResult DeleteLeaderboard(long id)
        {
            User user = GetUserFromAuthorizationHeader();
            try
            {
                var leaderboard = _repository.Find(id);
                _repository.Remove(leaderboard);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }

            return Ok();
        }
    }
}