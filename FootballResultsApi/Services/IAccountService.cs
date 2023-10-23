using FootballResultsApi.Models;

namespace FootballResultsApi.Services
{
    public interface IAccountService
    {
        int Register(LoginUserDto userDto);
        string Login(LoginUserDto loginDto);
    }
}
