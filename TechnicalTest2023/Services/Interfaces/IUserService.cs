using TechnicalTest2023.Models;

namespace TechnicalTest2023.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> TryToCreateUser(UserDTO userDto);
        Task<IEnumerable<User>> GetUsers();
        Task<User?> GetUserById(int id);
    }
}
