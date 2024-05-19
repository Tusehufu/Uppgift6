using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using InternJohan.Dev.Infrastructure.Models;
using InternJohan.Dev.Infrastructure.Repository;
using InternJohan.Dev.Infrastructure.ViewModel;
using System.Security.Claims;
using InternJohan.Dev.Infrastructure.Services;
using Microsoft.Extensions.Hosting;


    [Route("api/[controller]")]
    [ApiController]
    public class RepliesController : ControllerBase
    {
        private readonly ReplyService _replyService;

        public RepliesController(ReplyService replyService)
        {
            _replyService = replyService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reply>>> GetAllReplies()
        {
            var replies = await _replyService.GetAllReplies();
            return Ok(replies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reply>> GetReplyById(int id)
        {
            var reply = await _replyService.GetReplyById(id);
            if (reply == null)
            {
                return NotFound();
            }
            return reply;
        }

        [HttpPost]
        public async Task<ActionResult<Reply>> CreateReply(int postId, ReplyViewModel replyViewModel)
        {
            // Kontrollera om modellen är giltig
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Hämta den inloggade användarens ID från HttpContext
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Du måste vara inloggad för att skapa ett inlägg.");
            }
            // Convert the user ID to an integer or handle conversion errors
            int userId;
            if (!int.TryParse(userIdClaim.Value, out userId))
            {
                Console.WriteLine("Ogiltigt id");
                return BadRequest("Ogiltigt användar-ID.");
            }

            {
                //int postId = replyViewModel.PostId; // Uppdatera detta beroende på hur du får postens ID i ReplyViewModel
                //Console.WriteLine($"PostId: {postId}");
                // Skapa ett nytt Reply-objekt från viewmodel
                var reply = new Reply
                {
                    Content = replyViewModel.Content,
                    Timestamp = replyViewModel.Timestamp,
                    UserId = userId,
                    PostId = postId // Använd postId från frontend

                };
                Console.WriteLine($"Reply", postId);
                // Lägg till inlägget
                var id = await _replyService.CreateReply(reply, userId, postId);
                var author = reply.Author;
                // Skapa ett svarsobjekt med inläggets ID och författare
                var response = new
                {
                    ReplyId = id,
                    Author = author
                };
                return Ok(response);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReply(int id, Reply reply)
        {
            if (id != reply.ReplyId)
            {
                return BadRequest();
            }
            await _replyService.UpdateReply(reply);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveReply(int id)
        {
            //var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            //if (userIdClaim == null)
            //{
            //    return Unauthorized("Du måste vara inloggad för att ta bort svaret.");
            //}

            //// Convert the user ID to an integer or handle conversion errors
            //int userId;
            //if (!int.TryParse(userIdClaim.Value, out userId))
            //{
            //    Console.WriteLine("Ogiltigt id");
            //    return BadRequest("Ogiltigt användar-ID.");
            //}

            var result = await _replyService.RemoveReply(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok("Tja");
        }

    }


