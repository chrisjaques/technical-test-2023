using KellermanSoftware.CompareNetObjects;
using Microsoft.EntityFrameworkCore;
using TechnicalTest2023.DbContext;
using TechnicalTest2023.Models;
using TechnicalTest2023.Services.Interfaces;

namespace TechnicalTest2023.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly UserContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(UserContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> TryToCreateUser(User user)
        {
            var existingUsers = FindUsersByName(user.FirstName, user.LastName);

            if (existingUsers is not null)
            {
                var compare = new CompareLogic();
                compare.Config.MembersToIgnore.Add("Id"); // Ids are auto generated and unique, they will never be the same

                foreach (var existingUser in existingUsers)
                {
                    var comparisonResult = compare.Compare(existingUser, user);
                    if (!comparisonResult.AreEqual) continue;

                    _logger.LogError($"Unable to add user, as user already exists with id: [{existingUser.Id}]");

                    return false;
                }
            }

            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to save user");
                return false;
            }

            return true;
        }

        private IEnumerable<User>? FindUsersByName(string firstName, string lastName)
        {
            return _context.Users?.Where(x =>
                    string.Equals(firstName, x.FirstName, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(lastName, x.LastName, StringComparison.OrdinalIgnoreCase))
                .Include(x => x.Address);
        }
    }
}
