using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternJohan.Dev.Infrastructure.ViewModel
{
    public class UserViewModel
    {
        // Användarens inmatning
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        // Du behöver inte returnera PasswordHash eller CreatedAt i ViewModel
        // eftersom dessa hanteras internt av din applikation.
    }
}
