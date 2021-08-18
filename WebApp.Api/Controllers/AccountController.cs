using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Application.Abstractions;
using WebApp.Application.Interfaces;
using WebApp.Application.Models.DataTransferObjects.Incoming.Users;
using WebApp.Application.Models.DataTransferObjects.Outgoing.Users;
using WebApp.Application.Models.RequestFeatures.Baskets;
using WebApp.Data.Entities;

namespace WebApp.Api.Controllers
{
    [ApiController]
    [ApiVersionNeutral]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IAuthenticationManager _authManager;

        private readonly IRepositoryManager _repository;

        public AccountController(ILoggerManager logger, 
            IMapper mapper, 
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IAuthenticationManager authManager,
            IRepositoryManager repository)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _authManager = authManager;
            _repository = repository;
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

            var userAuth = _mapper.Map<UserAuthenticationDto>(userRegistration);
            if (!await _authManager.ValidateUser(userAuth))
            {
                _logger.LogWarn($"{nameof(Authenticate)}: Authentication failed. Wrong user name or password.");
                return Unauthorized();
            }

            var token = await _authManager.CreateToken();
            var authUser = await _userManager.FindByNameAsync(userRegistration.UserName);
            var roles = await _userManager.GetRolesAsync(authUser);

            return Ok(new 
            { 
                Token = token, 
                Roles = roles
            });
        }

        /// <summary>
        /// Authenticate a user
        /// </summary>
        /// <param name="userAuthentication"></param>
        /// <returns>Authenticated user</returns>
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserAuthenticationDto userAuthentication)
        {
            if (!await _authManager.ValidateUser(userAuthentication))
            {
                _logger.LogWarn($"{nameof(Authenticate)}: Authentication failed. Wrong user name or password.");
                return Unauthorized();
            }

            var token = await _authManager.CreateToken();
            var authUser = await _userManager.FindByNameAsync(userAuthentication.UserName);
            var roles = await _userManager.GetRolesAsync(authUser);

            return Ok(new 
            { 
                Token = token,
                Roles = roles
            });
        }

        /// <summary>
        /// Get user data
        /// </summary>
        /// <returns>Authenticated user</returns>
        [Authorize]
        [HttpGet("get_user_data")]
        public async Task<IActionResult> GetUserData()
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var userData = _mapper.Map<UserFullInfoDto>(user);
                return Ok(userData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetUserData)} action {ex} ");
                return StatusCode(500, "Internal server error");
            }
        }
        
        /// <summary>Delete a user</summary>
        /// <param name="userValid"></param>
        /// <returns>No content</returns>
        [Authorize]
        [HttpDelete("delete_user")]
        public async Task<IActionResult> DeleteUser([FromBody] UserValidationDto userValid)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userValid.UserName);

                //basket deleting
                var p = new BasketParameters();
                var basket = await _repository.Baskets.GetAllBasketItemsAsync(p, false, user.Id);
                foreach (var b in basket)
                    _repository.Baskets.Delete(b);
                await _repository.SaveAsync();

                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded == false)
                {
                    foreach (var error in result.Errors)
                        ModelState.TryAddModelError(error.Code, error.Description);

                    return BadRequest(ModelState);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(DeleteUser)} action {ex} ");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>Change user password</summary>
        /// <param name="passwords"></param>
        /// <returns>No content</returns>
        [Authorize]
        [HttpPut("change_password")]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordDto passwords)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var result = await _userManager.ChangePasswordAsync(user, passwords.OldPassword, passwords.NewPassword);

                if (result.Succeeded == false)
                {
                    foreach (var error in result.Errors)
                        ModelState.TryAddModelError(error.Code, error.Description);

                    return BadRequest(ModelState);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(ChangePassword)} action {ex} ");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
