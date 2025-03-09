using Astravon.Model.Dtos.Comment;
using Astravon.Model.Dtos.Likes;
using Astravon.Model.Dtos.Post;
using Astravon.Model.Dtos.Teacher;
using Astravon.Model.Dtos.User;
using CommentDto = Astravon.Model.Dtos.Comment.CommentDto;

namespace Astravon.Modules.User.Application.Port;

public interface IPostInputPort
{
    Task CreateComment(CreateCommentDto request);
    Task CreatePost(CreatePostDto request);
    Task CreateLike(CreateLikeDto request);
    Task UpdateComment(UpdateCommentDto request);
    Task DeleteComment(int commentId);
    Task GetCommentById(int commentId);
    
    Task UpdatePost(UpdatePostDto request);
    Task DeletePost(int postId);
    Task GetPostById(int postId);
    
    Task DeleteLike(int likeId);
    Task GetLikeById(int likeId);
    
    Task GetPostWithCountsById();
    Task GetCommentByPost(int postId);
}