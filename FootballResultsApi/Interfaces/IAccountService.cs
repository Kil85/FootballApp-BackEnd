using FootballResultsApi.Entities;
using FootballResultsApi.Models;

namespace FootballResultsApi.Interfaces
{
    public interface IAccountService
    {
        int Register(LoginUserDto userDto);
        UserDto Login(LoginUserDto loginDto);
        IEnumerable<User> getAllUsers();
    }
}
