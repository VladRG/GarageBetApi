using System.Linq;
using GarageBet.Api.Repository.Interfaces;
using GarageBet.Api.Database.Tables;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GarageBet.Api.Controllers
{
    public class GbController : Controller
    {
        private IUserRepository _userRepository;

        public GbController(IUserRepository repo)
        {
            _userRepository = repo;
        }

        public IActionResult InternalServerError(string error)
        {
            return StatusCode(500, new
            {
                Error = error
            });
        }

        protected User GetUserFromAuthorizationHeader()
        {
            string tokenString = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = tokenHandler.ReadJwtToken(tokenString);
            User user;
            try
            {
                string email = token.Claims.ToList()
                    .Where(claim => claim.Type == ClaimTypes.Email)
                    .FirstOrDefault()
                    .Value;
                user = _userRepository.FindByEmail(email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return user;
        }
    }
}
