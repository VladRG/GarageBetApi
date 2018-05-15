using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using GarageBet.Api.Configuration;
using GarageBet.Api.Repository.Interfaces;
using GarageBet.Api.Database.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using GarageBet.Api.Models;

namespace GarageBet.Api.Controllers
{
    [Route("auth")]
    public class AuthenticationController : GbController
    {
        public AuthenticationController(IOptions<JwtConfiguration> jwtConfiguration, IUserRepository userRepository)
            : base(userRepository)
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

        [AllowAnonymous]
        [HttpPost("/login")]
        public IActionResult Login([FromBody]User user)
        {

            User existingUser;

            if (String.IsNullOrWhiteSpace(user.Email) || String.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest();
            }

            try
            {
                existingUser = _userRepository.FindByEmail(user.Email.ToLower());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }

            if (user == null)
            {
                return NotFound();
            }
            else if (CheckPasswordHash(existingUser, user) == PasswordVerificationResult.Failed)
            {
                return Unauthorized();
            }

            AddAuthorizationHeader(existingUser);
            return Ok(GetUserModel(existingUser));
        }

        [HttpPost("/logout", Name = "Logout")]
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
        [HttpPost("/register")]
        public IActionResult Register([FromBody] User user)
        {
            try
            {
                User existingUser = _userRepository.FindByEmail(user.Email);
                if (existingUser != null)
                {
                    return StatusCode((int)HttpStatusCode.Conflict);
                }
                user.Password = HashPassword(user);
                user.Email = user.Email.ToLower();

                var emailClaim = new UserClaim
                {
                    ClaimType = ClaimTypes.Email,
                    ClaimValue = user.Email
                };
                user.Claims = new List<UserClaim>();
                user.Claims.Add(emailClaim);

                _userRepository.Add(user);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.Message);
            }
            AddAuthorizationHeader(user);
            return Created(String.Empty, GetUserModel(user));
        }

        private void AddAuthorizationHeader(User user)
        {
            var claims = new List<Claim>();

            if (user.Claims != null)
            {
                foreach (var claim in user.Claims)
                {
                    claims.Add(new Claim(claim.ClaimType, claim.ClaimValue));
                }
            }

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
            string tokenValue = new JwtSecurityTokenHandler().WriteToken(tokenObject);
            token.Append(tokenValue);
            try
            {
                user.Token = tokenValue;
                _userRepository.Update(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Request.HttpContext.Response.Headers.Add("Authorization", token.ToString());
        }

        private string HashPassword(User user)
        {
            var hasher = new PasswordHasher<User>();
            return hasher.HashPassword(user, user.Password);
        }

        private PasswordVerificationResult CheckPasswordHash(User existingUser, User user)
        {
            var hasher = new PasswordHasher<User>();
            return hasher.VerifyHashedPassword(
                existingUser,
                existingUser.Password,
                user.Password
            );
        }

        private UserModel GetUserModel(User user)
        {
            return new UserModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
        }
    }
}