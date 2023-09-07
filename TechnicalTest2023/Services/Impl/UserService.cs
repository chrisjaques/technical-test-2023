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

        public async Task<User?> TryToCreateUser(UserDTO userDto)
        {
            var user = User.Convert(userDto);
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

                    return null;
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
                return null;
            }

            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.Include(a => a.Address).ToListAsync();
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _context.Users.Include(x => x.Address).SingleOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Assumes a small database where this approach is fine, as it doesn't scale the best, eventually new indexes will need to be added and
        /// the query updated to include elements from the address i.e. street name or city.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        private IEnumerable<User>? FindUsersByName(string firstName, string lastName)
        {
            return _context.Users?.Where(x =>
                    string.Equals(firstName, x.FirstName, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(lastName, x.LastName, StringComparison.OrdinalIgnoreCase))
                .Include(x => x.Address);
        }
    }
}
