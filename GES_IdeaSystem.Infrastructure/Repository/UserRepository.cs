using GES_IdeaSystem.Application.interfaces;
using GES_IdeaSystem.Domain.DTO;
using GES_IdeaSystem.Domain.Entities;
using GES_IdeaSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES_IdeaSystem.Infrastructure.Repository
{
    public class UserRepository :  IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u =>u.Email == username);
        }

        public async Task SaveRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == token);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Users
                .AnyAsync(u => u.Email == username);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        //public async Task<IEnumerable<UserVoteCountDto>> GetUserVoteCount()
        //{
        //    return await _context.Users
        //        .Select(u => new UserVoteCountDto
        //        {
        //            UserId = u.Id,
        //            Email = u.Email,
        //            VoteCount = _context.Votes.Count(v => v.UserId == u.Id)
        //        }).ToListAsync();
        //}

        public async Task<IEnumerable<UserVoteCountDto>> GetUserVoteCount()
        {
            return await _context.Users
                .Select(u => new UserVoteCountDto
                {
                    UserId = u.Id,
                    Email = u.Email,
                    VoteCount = _context.Ideas
                        .Where(i => i.CreatedBy == u.Id)
                        .SelectMany(i => i.Votes)
                        .Count()
                }).ToListAsync();
        }
    }
}
