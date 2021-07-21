using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Application.Interfaces;
using WebApp.Application.Models.DataTransferObjects.Incoming.Users;
using WebApp.Data.Entities;

namespace WebApp.Api.Controllers
{
    [ApiController]
    //[ApiVersion("1.0")]
    //[ApiExplorerSettings(GroupName = "v1")]
    [ApiVersionNeutral]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IAuthenticationManager _authManager;

        public AccountController(ILoggerManager logger, 
            IMapper mapper, 
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IAuthenticationManager authManager)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _authManager = authManager;
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="userRegistration"></param>
        /// <returns>Created user</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto userRegistration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var role in userRegistration.Roles)
            {
                if (await _roleManager.FindByNameAsync(role) == null)
                    return BadRequest(ModelState);
            }

            var user = _mapper.Map<User>(userRegistration);
            var result = await _userManager.CreateAsync(user, userRegistration.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            
            await _userManager.AddToRolesAsync(user, userRegistration.Roles);
            return StatusCode(201);
        }

        /// <summary>
        /// Authenticate a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Authenticated user</returns>
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserAuthenticationDto user)
        {
            if (!await _authManager.ValidateUser(user))
            {
                _logger.LogWarn($"{nameof(Authenticate)}: Authentication failed. Wrong user name or password.");
                return Unauthorized();
            }
            return Ok(new { Token = await _authManager.CreateToken() });
        }

    }
}
