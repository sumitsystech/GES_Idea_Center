using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES_IdeaSystem.Application.interfaces
{
    public interface IAuthService
    {
        Task<string> Login(string email, string password);
        //Task<AuthResponse> Register(RegisterDto dto);
        //Task<AuthResponse> Login(LoginDto dto);
        //Task<AuthResponse> Refresh(string refreshToken);
        Task Logout(string refreshToken);
    }
}
