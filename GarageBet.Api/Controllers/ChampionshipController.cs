using System;
using System.Collections.Generic;
using System.Net;
using GarageBet.Data.Interfaces;
using GarageBet.Domain.MM;
using GarageBet.Domain.Tables;
using Microsoft.AspNetCore.Mvc;

namespace GarageBet.Api.Controllers
{
    public class ChampionshipController : GbController
    {
        private IChampionshipRepository _repository;
        private ITeamRepository _teamRepository;

        public ChampionshipController(IChampionshipRepository repository, ITeamRepository teamRepository)
        {
            _repository = repository;
            _teamRepository = teamRepository;
        }

        [HttpGet]
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

        [HttpGet]
        public IActionResult Find([FromQuery] long id)
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

        [HttpPost]
        public IActionResult Add([FromBody] Championship championship)
        {
            try
            {
                _repository.Add(championship);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Created(String.Empty, championship);
        }

        [HttpPut]
        public IActionResult Update([FromBody] Championship championship)
        {
            try
            {
                _repository.Update(championship);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Ok(championship);
        }

        [HttpPut]
        public IActionResult AddTeamToChampionship([FromQuery] long championshipId, [FromBody] long teamId)
        {
            Championship championship;
            Team team;
            try
            {
                championship = _repository.Find(championshipId);
                team = _teamRepository.Find(teamId);

                foreach (var champTeam in championship.Teams)
                {
                    if (champTeam.TeamId == teamId)
                    {
                        return StatusCode(HttpStatusCode.Found);
                    }
                }

                if (championship == null || team == null)
                {
                    return NotFound();
                }

                ChampionshipTeam championshipTeam = new ChampionshipTeam();
                championshipTeam.Championship = championship;
                championshipTeam.Team = team;
                championship.Teams.Add(championshipTeam);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Ok(championship);
        }

        [HttpPut]
        public IActionResult RemoveTeamFromChampionship([FromQuery] long championshipId, [FromBody] long teamId)
        {
            Championship championship;
            Team team;
            try
            {
                championship = _repository.Find(championshipId);
                team = _teamRepository.Find(teamId);
                if (championship == null || team == null)
                {
                    return NotFound();
                }

                foreach (var championshipTeam in championship.Teams)
                {
                    if (
                        championshipTeam.ChampionshipId == championshipId &&
                        championshipTeam.TeamId == teamId
                      )
                    {
                        championship.Teams.Remove(championshipTeam);
                        break;
                    }
                }
                _repository.Update(championship);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = ex.Message
                });
            }
            return Ok(championship);
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] long id)
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
    }
}