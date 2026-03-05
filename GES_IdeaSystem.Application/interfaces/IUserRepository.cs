using GES_IdeaSystem.Domain.DTO;
using GES_IdeaSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES_IdeaSystem.Application.interfaces
{
    //public interface IUserRepository
    //{
    //    Task<User> GetByUsernameAsync(string username);
    //    Task SaveRefreshTokenAsync(RefreshToken refreshToken);
    //    Task<RefreshToken> GetRefreshTokenAsync(string token);
    //    Task SaveChangesAsync();



    //}
    public interface IUserRepository
    {
        Task<User> GetByUsernameAsync(string username);
        Task<bool> UsernameExistsAsync(string username);
        Task AddUserAsync(User user);
        Task SaveRefreshTokenAsync(RefreshToken refreshToken);
        Task<RefreshToken> GetRefreshTokenAsync(string token);
        Task SaveChangesAsync();
        Task<IEnumerable<UserVoteCountDto>> GetUserVoteCount();
    }
}
