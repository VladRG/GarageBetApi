using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GarageBet.Api.Configuration;
using GarageBet.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GarageBet.Api.Controllers
{
    public class AuthenticationController : Controller
    {
        public AuthenticationController(
            IOptions<JwtConfiguration> jwtConfiguration,
            IUserRepository userRepository
            )
        {
            JwtConfig = jwtConfiguration.Value;
            _userRepository = userRepository;
        }

        public JwtConfiguration JwtConfig { get; set; }

        private IUserRepository _userRepository;

        [AllowAnonymous]
        public IActionResult Index()
        {
            return Ok(_userRepository.List());
        }

        [AllowAnonymous]
        public IActionResult Login([FromForm] string email, [FromForm] string password)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenObject = new JwtSecurityToken(
                    issuer: JwtConfig.Issuer,
                    audience: JwtConfig.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(JwtConfig.Validity),
                    signingCredentials: credentials
                );
            StringBuilder token = new StringBuilder("Bearer ");
            token.Append(new JwtSecurityTokenHandler().WriteToken(tokenObject));

            Request.HttpContext.Response.Headers.Add("Authorization", token.ToString());
            return Ok();
        }
    }
}