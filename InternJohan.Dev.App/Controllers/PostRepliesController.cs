using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using InternJohan.Dev.Infrastructure.Models;
using InternJohan.Dev.Infrastructure.Repository;
using InternJohan.Dev.Infrastructure.ViewModel;
using System.Security.Claims;


[Route("api/[controller]")]
[ApiController]
public class PostRepliesController : ControllerBase
{
    private readonly PostReplyService _postReplyService;

    public PostRepliesController(PostReplyService postReplyService)
    {
        _postReplyService = postReplyService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostReply>>> GetAllPostReplies()
    {
        var postReplies = await _postReplyService.GetAllPostReplies();
        return Ok(postReplies);
    }

    [HttpGet("{postId}/replies")]
    public async Task<IActionResult> GetRepliesByPostId(int postId)
    {
        var replies = await _postReplyService.GetRepliesByPostId(postId);
        return Ok(replies);
    }

    //[HttpPost]
    //public async Task<ActionResult> CreatePostReply(PostModel postReplyViewModel)
    //{
    //    // Kontrollera om modellen är giltig
    //    if (!ModelState.IsValid)
    //    {
    //        return BadRequest(ModelState);
    //    }

    //    // Skapa ett nytt PostReply-objekt från viewmodel
    //    var postReply = new PostReply
    //    {
    //        Content = postReplyViewModel.Content,
    //        Created = postReplyViewModel.Created
    //        // Fyll på med ytterligare attribut om det behövs
    //    };

    //    // Skapa postreply
    //    await _postReplyService.CreatePostReply(postReply);

    //    // Returnera svar med skapad status och det skapade postreply-objektet
    //    return CreatedAtAction(nameof(GetPostRepliesByPostId), new { postId = postReply.Id }, postReply);
    //}


    //[HttpDelete("{postId}/{replyId}")]
    //public async Task<IActionResult> DeletePostReply(int postId, int replyId)
    //{
    //    await _postReplyService.DeletePostReply(postId, replyId);
    //    return NoContent();
    //}
    //[HttpPost("{postId}/replies")]
    //public async Task<IActionResult> AddReplyToPost(int postId, [FromBody] Reply reply)
    //{
    //    var success = await _postReplyService.AddReplyToPost(postId, reply);
    //    if (success)
    //    {
    //        return Ok();
    //    }
    //    else
    //    {
    //        return StatusCode(500, "Failed to add reply to post.");
    //    }
    //}
}
