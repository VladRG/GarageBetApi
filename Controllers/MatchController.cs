using System;
using System.Collections.Generic;
using GarageBet.Api.Repository.Interfaces;
using GarageBet.Api.Models;
using GarageBet.Api.Database.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GarageBet.Api.Controllers
{
    [Route("match")]
    public class MatchController : GbController
    {
        private IMatchRepository _repository;

        public MatchController(IMatchRepository repository, IUserRepository userRepo) : base(userRepo)
        {
            _repository = repository;
        }

        [HttpGet("/match", Name = "List Matches")]
        public IActionResult Index()
        {
            IEnumerable<MatchBetModel> matches;
            User user = GetUserFromAuthorizationHeader();

            try
            {
                matches = _repository.ListMatchBets(user.Id);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Ok(matches);
        }

        [HttpGet("/match/championship/{championshipId}")]
        public IActionResult GetForChampionship(long championshipId)
        {
            IEnumerable<MatchBetModel> matches;
            User user = GetUserFromAuthorizationHeader();

            try
            {
                matches = _repository.GetForChampionship(championshipId, user.Id);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Ok(matches);
        }

        [HttpGet("/match/today")]
        public IActionResult GetForToday()
        {
            IEnumerable<MatchBetModel> matches;
            User user = GetUserFromAuthorizationHeader();

            try
            {
                matches = _repository.GetForToday(user.Id);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Ok(matches);
        }


        [HttpGet("/match/{id}", Name = "Match details")]
        public IActionResult ListByChampionship(long id)
        {
            Match match;
            try
            {
                match = _repository.Find(id);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Ok(match);
        }

        [HttpGet("/match/edit-bet/{id}")]
        public IActionResult GetMatchModelForEditBet(long id)
        {
            MatchEditBetForm response = null;
            try
            {
                response = _repository.GetMatchModelForEditBet(id);
            }
            catch (Exception ex)
            {
                InternalServerError(ex.Message);
            }
            return Ok(response);
        }

        [HttpGet("/match/new-bet/{id}")]
        public IActionResult GetMatchModelForNewBet(long id)
        {
            MatchNewBetForm response = null;
            try
            {
                response = _repository.GetMatchModelForNewBet(id);
            }
            catch (Exception ex)
            {
                InternalServerError(ex.Message);
            }
            return Ok(response);
        }


        [HttpGet("/match/bet/{betId}")]
        public IActionResult FindForBet(long betId)
        {
            MatchModel match;
            try
            {
                match = _repository.FindForBet(betId);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Ok(match);
        }

        [HttpGet("/match/available", Name = "Available Matches")]
        public IActionResult GetAvailableMatches()
        {
            IEnumerable<Match> matches;
            try
            {
                matches = _repository.ListAvailable();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Ok(matches);
        }

        [HttpPost("/match", Name = "Add Match")]
        [Authorize(Policy = "Admin")]
        public IActionResult Add([FromBody]Match match)
        {
            try
            {
                _repository.Add(match);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Created(String.Empty, null);
        }

        [HttpPut("/match/{id}", Name = "Update Match")]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(long id, [FromBody]Match match)
        {
            try
            {
                _repository.Update(match);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Ok(match);
        }

        [HttpDelete("/match/{id}", Name = "Delete Match")]
        [Authorize(Policy = "Admin")]
        public IActionResult Delete(long id)
        {
            Match match;
            try
            {
                match = _repository.Find(id);
                _repository.Remove(match);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Ok();
        }

        [HttpGet("/match/stats/{id}")]
        public IActionResult Stats(long id)
        {
            IEnumerable<MatchStats> stats;
            try
            {
                stats = _repository.GetMatchStats(id);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Ok(stats);
        }
    }
}