using GarageBet.Data.Interfaces;
using GarageBet.Domain.Tables;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace GarageBet.Api.Controllers
{
    public class TeamController : GbController
    {
        private ITeamRepository _repository;

        public TeamController(ITeamRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
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

        [HttpGet]
        public IActionResult Find([FromQuery] long id)
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

        [HttpPost]
        public IActionResult Add([FromBody] Team team)
        {
            try
            {
                _repository.Add(team);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Ok(team);
        }

        [HttpPut]
        public IActionResult Update([FromBody] Team team)
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

        [HttpDelete]
        public IActionResult Delete([FromQuery] long id)
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