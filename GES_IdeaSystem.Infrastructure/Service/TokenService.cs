using GES_IdeaSystem.Application.interfaces;
using GES_IdeaSystem.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GES_IdeaSystem.Infrastructure.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly IMemoryCache _cache;

        public TokenService(IConfiguration config, IMemoryCache cache)
        {
            _config = config;
            _cache = cache;
        }

        //public string GenerateAccessToken(User user)
        //{
        //    var claims = new[]
        //    {
        //    new Claim(ClaimTypes.Name, user.Email),
        //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        //};

        //    var key = new SymmetricSecurityKey(
        //        Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(
        //        issuer: _config["Jwt:Issuer"],
        //        audience: _config["Jwt:Audience"],
        //        claims: claims,
        //        expires: DateTime.UtcNow.AddMinutes(15),
        //        signingCredentials: creds);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}

        public string GenerateAccessToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            // Store token in cache
            _cache.Set(
                token,
                user.Id,
                TimeSpan.FromMinutes(15)); // same as token expiry

            return token;
        }
        public RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };
        }
    }
}
