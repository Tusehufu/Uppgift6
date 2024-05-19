using InternJohan.Dev.Infrastructure.Configuration;
using InternJohan.Dev.Infrastructure.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternJohan.Dev.Infrastructure.Repository
{
    public class PostReplyRepository
    {
        private readonly DatabaseSettings _databaseSettings;

        public PostReplyRepository(IOptions<DatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings.Value;
        }

        public async Task<IEnumerable<PostReply>> GetAllPostReplies()
        {
            IEnumerable<PostReply> postReplies;
            using (var connection = new SqlConnection(_databaseSettings.DefaultConnection))
            {
                postReplies = await connection.QueryAsync<PostReply>(@"
                    SELECT 
                        post_id AS PostId,
                        reply_id AS ReplyId
                    FROM 
                        PostReplies
                ");
            }
            return postReplies;
        }
        public async Task<IEnumerable<Reply>> GetRepliesByPostId(int postId)
        {
            using (var connection = new SqlConnection(_databaseSettings.DefaultConnection))
            {
                var result = await connection.QueryAsync<Reply>(@"
                SELECT 
                    r.reply_id AS ReplyId,
                    r.content AS Content,
                    r.author AS Author,
                    r.timestamp AS Timestamp
                FROM 
                    Replies r
                INNER JOIN 
                    PostReplies pr ON r.reply_id = pr.reply_id
                WHERE 
                    pr.post_id = @PostId
            ", new { PostId = postId });

                return result;
            }
        }
        public async Task<IEnumerable<PostReply>> GetPostReplyByIds(int postId)
        {
            using (var connection = new SqlConnection(_databaseSettings.DefaultConnection))
            {
               var result = await connection.QueryAsync<PostReply>(@"
            SELECT 
                reply_id AS ReplyId,
                post_id as PostId
            FROM 
                PostReplies
            WHERE 
                post_id = @PostId
        ", new { PostId = postId });
               
                
                    return result;
                
            }

        }


        public async Task<bool> InsertPostReply(PostReply postReply)
        {
            try
            {
                using (var connection = new SqlConnection(_databaseSettings.DefaultConnection))
                {
                    await connection.ExecuteAsync(@"
                        INSERT INTO PostReplies (post_id, reply_id) 
                        VALUES (@PostId, @ReplyId)
                    ", postReply);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeletePostReply(int postId, int replyId)
        {
            try
            {
                using (var connection = new SqlConnection(_databaseSettings.DefaultConnection))
                {
                    await connection.ExecuteAsync(@"
                        DELETE FROM PostReplies
                        WHERE 
                            post_id = @PostId AND reply_id = @ReplyId
                    ", new { PostId = postId, ReplyId = replyId });
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<IEnumerable<Reply>> GetRepliesForPost(int postId)
        {
            using (var connection = new SqlConnection(_databaseSettings.DefaultConnection))
            {
                var query = @"
                    SELECT r.reply_id AS ReplyId,
                           r.user_id AS UserId,
                           r.content AS Content,
                           r.timestamp AS Timestamp
                    FROM Replies r
                    INNER JOIN PostReplies pr ON r.reply_id = pr.reply_id
                    WHERE pr.post_id = @PostId";
                return await connection.QueryAsync<Reply>(query, new { PostId = postId });
            }
        }
        public async Task<bool> AddReplyToPost(int postId, int replyId)
        {
            try
            {
                using (var connection = new SqlConnection(_databaseSettings.DefaultConnection))
                {
                    await connection.ExecuteAsync(@"
                        INSERT INTO PostReplies (post_id, reply_id) 
                        VALUES (@PostId, @ReplyId)
                    ", new { PostId = postId, ReplyId = replyId });
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
