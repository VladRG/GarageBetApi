using System;
using System.Collections.Generic;
using GarageBet.Data.Interfaces;
using GarageBet.Domain.Tables;
using Microsoft.AspNetCore.Mvc;

namespace GarageBet.Api.Controllers
{
    [Route("championship")]
    public class ChampionshipController : GbController
    {
        private IChampionshipRepository _repository;
        private ITeamRepository _teamRepository;

        public ChampionshipController(IChampionshipRepository repository, ITeamRepository teamRepository)
        {
            _repository = repository;
            _teamRepository = teamRepository;
        }

        [HttpGet("/championship")]
        public IActionResult Index()
        {
            IEnumerable<Championship> championships;
            try
            {
                championships = _repository.List();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Ok(championships);
        }

        [HttpGet("/championship/{id}", Name = "Championship Details")]
        public IActionResult Find(long id)
        {
            Championship championship;
            try
            {
                championship = _repository.Find(id);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }

            if (championship == null)
            {
                return NotFound();
            }
            return Ok(championship);
        }

        [HttpPost("/championship", Name = "Add Championship")]
        public IActionResult Add(Championship championship)
        {
            try
            {
                _repository.Add(championship);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Created(string.Format("/championship/{0}", championship.Id), null);
        }

        [HttpPut("/championship/{id}", Name = "Update Championship")]
        public IActionResult Update(long id, Championship championship)
        {
            try
            {
                _repository.Update(championship);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Ok();
        }

        [HttpDelete("/championship/{id}", Name = "Delete Championship")]
        public IActionResult Delete(long id)
        {
            try
            {
                Championship championship = _repository.Find(id);
                _repository.Remove(championship);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Ok();
        }

        [HttpPut("/championship/team/{id}", Name = "Add Team to Championship")]
        public IActionResult AddTeamToChampionship(long id, long teamId)
        {
            Championship championship;
            Team team;
            try
            {
                championship = _repository.Find(id);
                team = _teamRepository.Find(teamId);

                if (championship == null || team == null)
                {
                    return NotFound();
                }
                _repository.AddTeam(championship, team);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Ok();
        }

        [HttpDelete("/championship/team/{id}", Name = "Remove Team from Championship")]
        public IActionResult RemoveTeamFromChampionship(long id, long teamId)
        {
            Championship championship;
            Team team;
            try
            {
                championship = _repository.Find(id);
                team = _teamRepository.Find(teamId);
                if (championship == null || team == null)
                {
                    return NotFound();
                }

                _repository.RemoveTeam(championship, team);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = ex.Message
                });
            }
            return Ok();
        }
    }
}