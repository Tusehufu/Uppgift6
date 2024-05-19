using System.Collections.Generic;
using System.Threading.Tasks;
using InternJohan.Dev.Infrastructure.Models;
using InternJohan.Dev.Infrastructure.Repository;

namespace InternJohan.Dev.API.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly RoleRepository _roleRepository;

        public UserService(UserRepository userRepository, RoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        // Metod för att hämta alla användare
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            // Hämta alla användare och inkludera deras respektive rollobjekt
            return await _userRepository.FindAllWithRoles();
        }

        // Metod för att hämta en användare efter ID
        public async Task<User> GetUserById(int id)
        {
            // Hämta användaren med dess rollobjekt
            return await _userRepository.FindByIdWithRole(id);
        }

        // Metod för att lägga till en ny användare
        public async Task<int> AddUser(User user)
        {
            // Hämta standardrollen "User"
            var standardRole = await _roleRepository.FindByName("User");

            // Se till att standardrollen finns
            if (standardRole == null)
            {
                throw new InvalidOperationException("Standardrollen 'User' kunde inte hittas.");
            }

            // Tilldela standardrollen till användaren
            user.RoleId = standardRole.Id;

            // Lägg till användaren i databasen
            return await _userRepository.Add(user);
        }

        // Metod för att uppdatera en användare
        public async Task<bool> UpdateUser(int id, User user)
        {
            // Se till att användarens ID stämmer
            if (id != user.Id)
            {
                return false;
            }

            // Uppdatera användaren
            return await _userRepository.Update(user);
        }

        // Metod för att ta bort en användare
        public async Task<bool> DeleteUser(int id)
        {
            return await _userRepository.Delete(id);
        }

        // Metod för att lägga till en roll till en användare
        public async Task<bool> AddRoleToUser(int userId, int roleId)
        {
            // Hämta användare och roll baserat på ID
            var user = await _userRepository.FindByIdWithRole(userId);
            var role = await _roleRepository.FindById(roleId);

            // Se till att användaren och rollen finns
            if (user == null || role == null)
            {
                return false; // Användaren eller rollen hittades inte
            }

            // Uppdatera användarens roll till den nya rollen
            user.RoleId = role.Id;

            // Uppdatera användaren i databasen
            return await _userRepository.Update(user);
        }
        // Lägg till en metod för att hitta en användare baserat på användarnamn
        public async Task<User> FindUserByUsername(string username)
        {
            return await _userRepository.FindByUsername(username);
        }

    }
}
