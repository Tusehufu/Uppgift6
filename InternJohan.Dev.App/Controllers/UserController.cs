using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using InternJohan.Dev.Infrastructure.Models;
using InternJohan.Dev.API.Services;
using InternJohan.Dev.Infrastructure.ViewModel;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using InternJohan.Dev.Infrastructure.Repository;

namespace InternJohan.Dev.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        // GET: api/users
        [HttpGet]
        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userService.GetAllUsers();
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public async Task<IActionResult> Add(UserViewModel userViewModel)
        {
            // Skapa ett nytt User-objekt och mappa egenskaperna från UserViewModel
            User user = new User
            {
                Username = userViewModel.Username,
                Email = userViewModel.Email,
                Password = userViewModel.Password,
                CreatedAt = DateTime.UtcNow,
                RoleId = 1
            };

            // Lägg till användaren i databasen
            int newUserId = await _userService.AddUser(user);

            // Returnera det nyskapade användarens ID med 201 Created
            return CreatedAtAction(nameof(GetById), new { id = newUserId }, user);
        }

        // PUT: api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            bool success = await _userService.UpdateUser(id, user);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool success = await _userService.DeleteUser(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        // Åtgärd för att lägga till en roll till en användare
        [HttpPost("{userId}/roles/{roleId}")]
        public async Task<IActionResult> AddRoleToUser(int userId, int roleId)
        {
            var success = await _userService.AddRoleToUser(userId, roleId);

            if (!success)
            {
                return NotFound("Användaren eller rollen hittades inte.");
            }

            return NoContent();
        }
        [HttpGet("id")]
        public async Task<IActionResult> GetUserIdByUsername(string username)
        {
            // Hämta användar-ID från databasen baserat på användarnamnet
            var user = await _userService.FindUserByUsername(username);

            if (user == null)
            {
                return NotFound("Användaren hittades inte.");
            }

            return Ok( user.Id );
        }
    }
}
