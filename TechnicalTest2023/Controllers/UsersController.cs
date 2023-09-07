using KellermanSoftware.CompareNetObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnicalTest2023.DbContext;
using TechnicalTest2023.Models;

namespace TechnicalTest2023.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserContext _context;
        private readonly ILogger<UsersController> _logger;

        public UsersController(UserContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            return await _context.Users.Include(a => a.Address).ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        { 
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                var error = new EnrichedErrors
                {
                    EnrichedError = EnrichedErrorType.InvalidUserInputError,
                    ErrorDetails = ModelState.Values.ToHuman()
                };

                HttpContext.Features.Set(error);
                _logger.LogError("Unable to add user, as user received is invalid: {0}", ModelState.Values.ToLogs());
                return BadRequest();
            }

            var existingUsers = FindUsersByName(user.FirstName, user.LastName);

            if (existingUsers is not null)
            {
                var compare = new CompareLogic();
                compare.Config.MembersToIgnore.Add("Id"); // Ids are auto generated and unique, they will never be the same

                foreach (var existingUser in existingUsers)
                {
                    var comparisonResult = compare.Compare(existingUser, user);
                    if (!comparisonResult.AreEqual) continue;

                    var error = new EnrichedErrors
                    {
                        EnrichedError = EnrichedErrorType.DuplicateUserError,
                        ErrorDetails = "You have already created a user"
                    };

                    HttpContext.Features.Set(error);
                    _logger.LogError($"Unable to add user, as user already exists with id: [{existingUser.Id}]");
                    return Conflict();
                }
            }
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
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
