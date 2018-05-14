﻿using System;
using System.Collections.Generic;
using GarageBet.Data.Interfaces;
using GarageBet.Data.Models;
using GarageBet.Domain.Tables;
using Microsoft.AspNetCore.Mvc;

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