using GES_IdeaSystem.Application;
using GES_IdeaSystem.Application.interfaces;
using GES_IdeaSystem.Domain;
using GES_IdeaSystem.Domain.DTO;
using GES_IdeaSystem.Domain.Entities;
using GES_IdeaSystem.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace GES_IdeaSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly ITokenService _tokenService;
        private readonly IMemoryCache _cache;

        public AuthController(IUserRepository repo, ITokenService tokenService, IMemoryCache cache)
        {
            _repo = repo;
            _tokenService = tokenService;
            _cache = cache;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            var user = await _repo.GetByUsernameAsync(dto.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized();// Hash in real project
          

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            refreshToken.UserId = user.Id;

            await _repo.SaveRefreshTokenAsync(refreshToken);
            await _repo.SaveChangesAsync();

            return Ok(new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(string refreshToken)
        {
            var storedToken = await _repo.GetRefreshTokenAsync(refreshToken);

            if (storedToken == null || storedToken.Expires < DateTime.UtcNow || storedToken.IsRevoked)
                return Unauthorized();

            var newAccessToken = _tokenService.GenerateAccessToken(storedToken.User);

            return Ok(new TokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = refreshToken
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto dto)
        {
            if (await _repo.UsernameExistsAsync(dto.Username))
                return BadRequest("Username already exists");

            var user = new User
            {
                Email = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role="User"

            };

            await _repo.AddUserAsync(user);
            await _repo.SaveChangesAsync();

            return Ok("User created successfully");
        }


        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var token = Request.Headers["Authorization"]
                .ToString().Replace("Bearer ", "");

            _cache.Remove(token);

            return Ok("Logged out successfully");
        }

        [HttpGet("vote-count")]
        public async Task<IActionResult> GetUserVoteCount()
        {
            var result = await _repo.GetUserVoteCount();
            return Ok(result);
        }
    }
}
