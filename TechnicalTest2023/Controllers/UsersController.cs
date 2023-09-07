using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnicalTest2023.DbContext;
using TechnicalTest2023.Logging;
using TechnicalTest2023.Models;
using TechnicalTest2023.Services.Interfaces;

namespace TechnicalTest2023.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserContext _context;
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;

        public UsersController(UserContext context, ILogger<UsersController> logger, IUserService userService)
        {
            _context = context;
            _logger = logger;
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var result = new List<UserDTO>();
            var existingUsers = await _context.Users.Include(a => a.Address).ToListAsync();
            foreach (var existingUser in existingUsers)
            {
                result.Add(UserDTO.Convert(existingUser));
            }
            return result;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUserById(int id)
        { 
            var user = await _context.Users.Include(x => x.Address).SingleOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return UserDTO.Convert(user);
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                var error = new EnrichedErrors
                {
                    EnrichedError = EnrichedErrorType.InvalidUserInputError,
                    ErrorDetails = ModelState.Values.ToUserFacingDescription()
                };

                HttpContext.Features.Set(error);
                _logger.LogError("Unable to add user, as user received is invalid: {0}", ModelState.Values.ToLogs());
                return BadRequest();
            }

            var result = await _userService.TryToCreateUser(user);

            if (!result)
            {
                var error = new EnrichedErrors
                {
                    EnrichedError = EnrichedErrorType.DuplicateUserError,
                    ErrorDetails = "You have already created a user"
                };

                HttpContext.Features.Set(error);
                return Conflict();
            }

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id },  UserDTO.Convert(user));
        }
    }
}
