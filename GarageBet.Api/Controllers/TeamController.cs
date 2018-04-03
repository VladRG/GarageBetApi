using GarageBet.Data.Interfaces;
using GarageBet.Domain.Tables;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace GarageBet.Api.Controllers
{
    [Route("team")]
    public class TeamController : GbController
    {
        private ITeamRepository _repository;

        public TeamController(ITeamRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("/team", Name = "List Teams")]
        public IActionResult Index()
        {
            IEnumerable<Team> teams;
            try
            {
                teams = _repository.List();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Ok(teams);
        }

        [HttpGet("/team/{id}", Name = "Team details")]
        public IActionResult Find(long id)
        {
            Team team;
            try
            {
                team = _repository.Find(id);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }

            if (team == null)
            {
                return NotFound();
            }
            return Ok(team);
        }

        [HttpPost("/team", Name = "Add Team")]
        public IActionResult Add(Team team)
        {
            try
            {
                _repository.Add(team);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Created(string.Format("/team/{0}", team.Id), null);
        }

        [HttpPut("/team/{id}", Name = "Update Team")]
        public IActionResult Update(long id, Team team)
        {
            try
            {
                _repository.Update(team);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Ok(team);
        }

        [HttpDelete("/team/{id}", Name = "Delete Team")]
        public IActionResult Delete(long id)
        {
            try
            {
                Team team = _repository.Find(id);
                _repository.Remove(team);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Ok();
        }
    }
}