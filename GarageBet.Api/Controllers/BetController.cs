using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GarageBet.Data;
using GarageBet.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarageBet.Api.Controllers
{
    public class BetController : Controller
    {
        private IMatchBetRepository _repository;

        public BetController(IMatchBetRepository repository)
        {
            _repository = repository;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return Ok(_repository.List().FirstOrDefault().User);
        }
    }
}