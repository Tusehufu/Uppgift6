using System.Collections.Generic;
using System.Threading.Tasks;
using InternJohan.Dev.Infrastructure.Models;
using InternJohan.Dev.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace InternJohan.Dev.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly RoleService _roleService;

        public RoleController(RoleService roleService)
        {
            _roleService = roleService;
        }

        // GET: api/roles
        [HttpGet]
        public async Task<IEnumerable<Role>> GetAll()
        {
            return await _roleService.GetAllRoles();
        }

        // GET: api/roles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var role = await _roleService.GetRoleById(id);

            if (role == null)
            {
                return NotFound();
            }

            return Ok(role);
        }
    }
}
