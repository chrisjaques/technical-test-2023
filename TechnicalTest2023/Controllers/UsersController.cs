using Microsoft.AspNetCore.Mvc;
using TechnicalTest2023.Logging;
using TechnicalTest2023.Models;
using TechnicalTest2023.Services.Interfaces;

namespace TechnicalTest2023.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        // Use a service to prevent logic from being added to controllers, also discourages access to db context within a controller
        private readonly IUserService _userService;

        public UsersController(ILogger<UsersController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var result = new List<UserDTO>();
            var existingUsers = await _userService.GetUsers();
            foreach (var existingUser in existingUsers)
            {
                result.Add(UserDTO.Convert(existingUser));
            }
            return result;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDTO>> GetUserById(int id)
        { 
            var user = await _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            return UserDTO.Convert(user);
        }

        // POST: api/Users
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<UserDTO>> PostUser(UserDTO user)
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

            var userCreated = await _userService.TryToCreateUser(user);

            if (userCreated is null)
            {
                var error = new EnrichedErrors
                {
                    EnrichedError = EnrichedErrorType.DuplicateUserError,
                    ErrorDetails = "You have already created a user"
                };

                HttpContext.Features.Set(error);
                return Conflict();
            }

            // We know userCreated is not null at this point so suppress warning
            return CreatedAtAction(nameof(GetUserById), new { id = userCreated.Id },  UserDTO.Convert(userCreated));
        }
    }
}
