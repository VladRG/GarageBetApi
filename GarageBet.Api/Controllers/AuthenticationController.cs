using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using GarageBet.Api.Configuration;
using GarageBet.Data.Interfaces;
using GarageBet.Domain.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GarageBet.Api.Controllers
{
    public class AuthenticationController : GbController
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
            User user = null;
            try
            {
                user = _userRepository.FindByEmail(email);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }

            if (user == null)
            {
                return NotFound();
            }

            if (String.Compare(password, user.Password, false) != 0)
            {
                return Unauthorized();
            }

            StringBuilder roles = new StringBuilder();
            foreach (var role in user.Roles)
            {
                roles.Append(role.RoleId + "#");
            }
            roles.Remove(roles.Length - 1, 1);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, roles.ToString())
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig.Key));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken tokenObject = new JwtSecurityToken(
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

        [AllowAnonymous]
        public IActionResult Register([FromBody] User user)
        {
            User existingUser = _userRepository.FindByEmail(user.Email);
            if (existingUser != null)
            {
                return StatusCode(HttpStatusCode.Conflict);
            }

            try
            {
                _userRepository.Add(user);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            return Created(String.Empty, user);
        }
    }
}