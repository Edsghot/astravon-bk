using Astravon.Configuration.Shared;
using Astravon.Model.Dtos.Comment;
using Astravon.Model.Dtos.Likes;
using Astravon.Model.Dtos.Post;
using Astravon.Model.Dtos.User;

namespace Astravon.Modules.User.Application.Port;

public interface IPostOutPort : IBasePresenter<object>
{
    void GetAllUsersAsync(IEnumerable<UserDto> data);
    void Login(UserDto data);
    void GetPostWithCounts(List<PostWithCountsDto> enumerable);
    void GetLikeById(LikeDto data);
    
    void GetPostById(PostDto data);
    void GetCommentById(CommentDto data);
    void GetCommentByPost(List<CommentDto> data);
}