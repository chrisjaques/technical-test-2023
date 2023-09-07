using TechnicalTest2023.Models;

namespace TechnicalTest2023.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> TryToCreateUser(User user);
    }
}
