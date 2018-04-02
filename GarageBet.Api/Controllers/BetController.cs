using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Views;
using GarageBet.Data;
using GarageBet.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarageBet.Api.Controllers
{
    public class BetController : GbController
    {
        private IMatchBetRepository _matchBetRepository;
        private IBetRepository _repository;

        public BetController(
                IBetRepository repository,
                IMatchBetRepository matchBetRepository
            )
        {
            _repository = repository;
            _matchBetRepository = matchBetRepository;
        }

        [HttpGet]
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

        [HttpGet]
        public IActionResult FindByUser([FromQuery] long userId)
        {
            IEnumerable<MatchBet> bets;
            try
            {
                bets = _matchBetRepository.FindByUserId(userId);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Ok(bets);
        }
    }
}