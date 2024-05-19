using InternJohan.Dev.Infrastructure.Configuration;
using InternJohan.Dev.Infrastructure.Models;
using Microsoft.Extensions.Options;
using Dapper;
using Microsoft.Data.SqlClient;
using InternJohan.Dev.Infrastructure.ViewModel;

namespace InternJohan.Dev.Infrastructure.Repository
{

    public class PostReplyService
    {
        private readonly PostReplyRepository _postReplyRepository;

        public PostReplyService(PostReplyRepository postReplyRepository)
        {
            _postReplyRepository = postReplyRepository;
        }

        public async Task<IEnumerable<PostReply>> GetAllPostReplies()
        {
            return await _postReplyRepository.GetAllPostReplies();
        }

        public async Task<IEnumerable<PostReply>> GetPostRepliesByPostId(int postId)
        {
            return await _postReplyRepository.GetPostReplyByIds(postId);
        }

        public async Task CreatePostReply(PostReply postReply)
        {
            await _postReplyRepository.InsertPostReply(postReply);
        }

        public async Task DeletePostReply(int postId, int replyId)
        {
            await _postReplyRepository.DeletePostReply(postId, replyId);
        }
        public Task<IEnumerable<Reply>> GetRepliesByPostId(int postId)
        {
            return _postReplyRepository.GetRepliesByPostId(postId);
        }
        //public async Task<IEnumerable<Reply>> GetRepliesForPost(int postId)
        //{
        //    var postReplies = await _postReplyRepository.GetPostReplyByIds(postId);
        //    var replies = new List<Reply>();

        //    foreach (var postReply in postReplies)
        //    {
        //        var reply = await _postReplyRepository.GetReplyById(postReply.ReplyId);
        //        if (reply != null)
        //        {
        //            replies.Add(reply);
        //        }
        //    }

        //    return replies;
        //}

        //// New method to add a reply to a post
        //public async Task<bool> AddReplyToPost(int postId, Reply reply)
        //{
        //    // Insert the reply into Replies table
        //    var isInserted = await _postReplyRepository.InsertReply(reply);

        //    if (isInserted)
        //    {
        //        // Associate the reply with the post
        //        var postReply = new PostReply
        //        {
        //            PostId = postId,
        //            ReplyId = reply.ReplyId
        //        };
        //        return await _postReplyRepository.InsertPostReply(postReply);
        //    }
        //    return false;
        //}
        //public async Task<IEnumerable<Reply>> GetRepliesForPost(int postId)
        //{
        //    return await _postReplyRepository.GetRepliesForPost(postId);
        //}
    }
}