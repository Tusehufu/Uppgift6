﻿using InternJohan.Dev.Infrastructure.Configuration;
using InternJohan.Dev.Infrastructure.Models;
using Microsoft.Extensions.Options;
using Dapper;
using Microsoft.Data.SqlClient;
using InternJohan.Dev.Infrastructure.ViewModel;
using Microsoft.Extensions.Hosting;

namespace InternJohan.Dev.Infrastructure.Repository
{

    public class ReplyService
    {
        private readonly ReplyRepository _replyRepository;

        public ReplyService(ReplyRepository replyRepository)
        {
            _replyRepository = replyRepository;
        }

        public async Task<IEnumerable<Reply>> GetAllReplies()
        {
            return await _replyRepository.GetAllReplies();
        }

        public async Task<Reply> GetReplyById(int id)
        {
            return await _replyRepository.GetReplyById(id);
        }

        public async Task<int> CreateReply(Reply reply, int userId, int postId)
        {
            await _replyRepository.InsertReplyWithAuthor(reply, userId, postId);
            Console.WriteLine(postId);
            return reply.ReplyId;
        }

        public async Task UpdateReply(Reply reply)
        {
            await _replyRepository.UpdateReply(reply);
        }

        public async Task<bool> RemoveReply(int id)
        {
           return await _replyRepository.DeleteReply(id);
        }
    }
}
