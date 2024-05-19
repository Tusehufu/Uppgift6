using System.Collections.Generic;
using System.Threading.Tasks;
using InternJohan.Dev.Infrastructure.Models;
using InternJohan.Dev.Infrastructure.Repository;

namespace InternJohan.Dev.API.Services
{
    public class RoleService
    {
        private readonly RoleRepository _roleRepository;

        public RoleService(RoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        // Metod för att hämta alla roller
        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            return await _roleRepository.FindAll();
        }

        // Metod för att hämta en roll efter ID
        public async Task<Role> GetRoleById(int id)
        {
            return await _roleRepository.FindById(id);
        }
    }
}
