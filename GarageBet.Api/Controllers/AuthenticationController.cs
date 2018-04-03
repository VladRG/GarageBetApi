using System;
using System.Linq;
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
        public AuthenticationController(IOptions<JwtConfiguration> jwtConfiguration, IUserRepository userRepository)
        {
            JwtConfig = jwtConfiguration.Value;
            _userRepository = userRepository;
        }

        public JwtConfiguration JwtConfig { get; set; }

        private IUserRepository _userRepository;

        public IActionResult Index()
        {
            return Ok(_userRepository.List());
        }

        public IActionResult Login(User user)
        {
            if (String.IsNullOrWhiteSpace(user.Email) || String.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest();
            }

            try
            {
                user = _userRepository.FindByEmail(user.Email);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }

            if (user == null)
            {
                return NotFound();
            }

            if (String.Compare(user.Password, user.Password, false) != 0)
            {
                return Unauthorized();
            }

            AddAuthorizationHeader(user);
            return Ok();
        }

        [HttpGet("/logout", Name = "Logout")]
        public IActionResult Logout()
        {
            try
            {
                User user = GetUserFromAuthorizationHeader();
                _userRepository.Update(user);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }

            Request.Headers.Remove("Authorization");
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

        private void AddAuthorizationHeader(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email)
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
        }
    }
}