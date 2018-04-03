using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Views;
using GarageBet.Data;
using GarageBet.Data.Interfaces;
using GarageBet.Domain.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarageBet.Api.Controllers
{
    public class BetController : GbController
    {
        private IMatchBetRepository _matchBetRepository;
        private IBetRepository _repository;

        public BetController(IBetRepository repository, IMatchBetRepository matchBetRepository)
        {
            _repository = repository;
            _matchBetRepository = matchBetRepository;
        }

        [HttpGet("/bet", Name = "List Bets")]
        public IActionResult Index()
        {
            IEnumerable<MatchBet> bets;
            try
            {
                bets = _matchBetRepository.List();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Ok(bets);
        }

        [HttpGet("/bet/{id}")]
        public IActionResult FindByUser(long id)
        {
            IEnumerable<MatchBet> bets;
            try
            {
                bets = _matchBetRepository.FindByUserId(id);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Ok(bets);
        }

        [HttpPost("/bet/{matchId}", Name = "Add Bet")]
        public IActionResult Add(long matchId, Bet bet)
        {
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
        public IActionResult Update(long id, Bet bet)
        {
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

    }
}