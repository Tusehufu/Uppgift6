using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using InternJohan.Dev.Infrastructure.Models;
using InternJohan.Dev.Infrastructure.Repository;
using InternJohan.Dev.Infrastructure.ViewModel;
using System.Security.Claims;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace InternJohan.Dev.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SportEventController : ControllerBase
    {
        private readonly SportEventService _sportEventService;
        public SportEventController(SportEventService sportEventService)
        {
            _sportEventService = sportEventService;
        }
        [HttpGet]

        public async Task<ActionResult<IEnumerable<SportEventViewModel>>> GetSportEvents()
        {
            var sportEvents = await _sportEventService.GetAll();
            return Ok(sportEvents);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<SportEvent>> GetSportEvent(int id)
        {
            var sportEvent = await _sportEventService.FindById(id);

            if (sportEvent == null)
            {
                return NotFound("Sportevent not found");
            }

            return Ok(sportEvent);
        }

        [HttpPut("{id}")]
        //[Authorize(Policy = "SportEventAccess")]
        public async Task<IActionResult> UpdateSportEvent(int id, SportEventViewModel sportEventViewModel)
        {
            // Hämta den inloggade användarens ID från HttpContext
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            Console.WriteLine(userIdClaim);
            if (userIdClaim == null)
            {
                return Unauthorized("Du måste vara inloggad för att uppdatera ett SportEvent.");
            }

            // Konvertera användar-ID till ett heltal eller hantera konverteringsfel
            int userId;
            if (!int.TryParse(userIdClaim.Value, out userId))
            {
                Console.WriteLine("Ogiltigt id");
                return BadRequest("Ogiltigt användar-ID.");
            }
            Console.WriteLine($"{userIdClaim.Value}");
            Console.WriteLine(userId);
            var oldSportEvent = await _sportEventService.FindById(id);
            if (oldSportEvent == null)
            {
                return NotFound("Sportevent not found");
            }

            // Uppdatera SportEvent-objektet med nya värden från viewmodel
            oldSportEvent.Sport = sportEventViewModel.Sport;
            oldSportEvent.NeededParticipants = sportEventViewModel.NeededParticipants;
            oldSportEvent.Participants = sportEventViewModel.Participants;
            oldSportEvent.DateTime = sportEventViewModel.DateTime;
            oldSportEvent.Location = sportEventViewModel.Location;
            //oldSportEvent.UserHost = new User { Id = userId };
            await _sportEventService.Update(oldSportEvent);
            return NoContent();
        }




        [HttpPost]
        public async Task<ActionResult> PostSportEvent(SportEventViewModel sportEventViewModel)
        {
            // Hämta den inloggade användarens ID från HttpContext
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Du måste vara inloggad för att skapa ett SportEvent.");
            }

            // Convert the user ID to an integer or handle conversion errors
            int userId;
            if (!int.TryParse(userIdClaim.Value, out userId))
            {
                Console.WriteLine("Ogiltigt id");
                return BadRequest("Ogiltigt användar-ID.");
            }

            // Skapa ett nytt SportEvent-objekt från viewmodel
            SportEvent sportEvent = new SportEvent
            {
                Sport = sportEventViewModel.Sport,
                NeededParticipants = sportEventViewModel.NeededParticipants,
                Participants = sportEventViewModel.Participants,
                DateTime = sportEventViewModel.DateTime,
                Location = sportEventViewModel.Location,
                // Sätt UserHost till den inloggade användarens ID
                UserHost = new User { Id = userId }
            };

            // Lägg till sportEventet
            var id = await _sportEventService.Add(sportEvent);

            return Ok(id);
        }

        [HttpDelete("{id}")]
        //[Authorize(Policy = "SportEventAccess")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Du måste vara inloggad för att ta bort evenemanget.");
            }

            // Convert the user ID to an integer or handle conversion errors
            int userId;
            if (!int.TryParse(userIdClaim.Value, out userId))
            {
                Console.WriteLine("Ogiltigt id");
                return BadRequest("Ogiltigt användar-ID.");
            }

            // Anropa service-metoden för att försöka ta bort evenemanget
            var success = await _sportEventService.DeleteEvent(userId, id);

            if (success)
            {
                return Ok("Evenemanget har tagits bort.");
            }
            else
            {
                return BadRequest("Du har inte behörighet att ta bort detta evenemang.");
            }
        }

        // Endpoint för att gå med i evenemanget

        [HttpPost("{id}/join")]
        public async Task<IActionResult> JoinSportEvent(int id)
        {

            // Hämta den inloggade användarens ID från HttpContext
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Du måste vara inloggad för att gå med i evenemanget.");
            }

            // Convert the user ID to an integer or handle conversion errors
            int userId;
            if (!int.TryParse(userIdClaim.Value, out userId))
            {
                Console.WriteLine("Ogiltigt id");
                return BadRequest("Ogiltigt användar-ID.");
            }

            //Kontrollera om användaren redan har gått med i evenemanget
            var isAlreadyParticipant = await _sportEventService.IsUserParticipant(userId, id);
            if (isAlreadyParticipant)
            {
                return BadRequest("Användaren har redan gått med i evenemanget.");
            }

            // Hämta evenemanget från databasen
            var sportEvent = await _sportEventService.FindById(id);
            if (sportEvent == null)
            {
                return NotFound("Sportevent not found");
            }

            // Lägg till användaren som deltagare i evenemanget
            var success = await _sportEventService.JoinEvent(userId, id);
            if (success)
            {
                return Ok(new { success = true });
            }
            else
            {
                return BadRequest("Kunde inte gå med i evenemanget.");
            }
        }
        [HttpDelete("{eventId}/participant/{userId}")]
        public async Task<IActionResult> RemoveParticipant(int eventId, int userId)
        {
            try
            {
                await _sportEventService.RemoveParticipant(userId, eventId);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }
    }
}
