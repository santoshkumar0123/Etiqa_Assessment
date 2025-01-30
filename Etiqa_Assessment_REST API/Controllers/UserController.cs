using Etiqa_Assessment_REST_API.Models;
using Etiqa_Assessment_REST_API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Etiqa_Assessment_REST_API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/[controller]")] // for backward compatibility
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserRepository userRepository, ILogger<UserController> logger)
        {
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));
            _logger = logger;
        }

        [HttpGet]
        [Route("users")]
        //[Authorize]
        public async Task<ActionResult<ApiResponse>> GetUsers(int pageIndex = 1, int pageSize = 1)
        {
            var userLists = await _userRepository.GetUsersAsync(pageIndex, pageSize);
            return new ApiResponse(true, "Retrieved users list", userLists);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("users")]
        public async Task<ActionResult<ApiResponse>> AddUser(User userDetails)
        {
            if (userDetails != null)
            {
                try
                {
                    var insertResult = await _userRepository.AddUserAsync(userDetails);
                    return new ApiResponse(true, "New user added successfully!", insertResult);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "LogError: An error occurred while processing the request.");
                }                
            }
            return Ok("User registration failed");
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        [Route("users")]
        public async Task<IActionResult> UpdateUser([FromBody] User objUserDetails)
        {
            try
            {
                if (objUserDetails == null)
                {
                    return BadRequest("Invalid data.");
                }
                // Call the User Repository to update the user
                await _userRepository.UpdateUserAsync(objUserDetails);
                return new JsonResult("User updated Successfully !");  // Return NoContent for a successful update
            }
            catch (Exception ex) {
                _logger.LogError(ex, "LogError: An error occurred while processing the request.");
            }
            return new JsonResult("User updation failed!");
        }
        
        [HttpDelete]
        [Route("users")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            await _userRepository.DeleteUserAsync(username);
            return new JsonResult("User deleted Successfully");
        }

        // GET: api/users/search?query=wildcard
        [HttpGet]
        [Route("users/wildcardsearch")]
        public async Task<ActionResult<ApiResponse>> WildcardSearch(string username, string mail)
        {
            // Validate input
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(mail))
            {
                return new ApiResponse(true, "Search Results", "No data");
            }           
            var searchResult = await _userRepository.WildcardSearchAsync(username, mail);
            return new ApiResponse(true, "Search Results", searchResult);
        }

        [HttpDelete]
        [Route("users/archiveuser")]
        public async Task<IActionResult> ArchiveUser(string username)
        {
            await _userRepository.ArchiveUserAsync(username);
            return new JsonResult("User archived Successfully");
        }

        [HttpPost]
        [Route("users/unarchiveuser")]
        public async Task<IActionResult> UnarchiveUser(string username)
        {
            if (username != null)
            {
                try
                {
                    await _userRepository.UnarchiveUserAsync(username);
                    return new JsonResult("User unarchived Successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "LogError: An error occurred while processing the request.");
                }
            }
            return Ok("User unarchival failed");
        }
    }
}
