using Astravon.Configuration.Shared;
using Astravon.Model.Dtos.Comment;
using Astravon.Model.Dtos.Likes;
using Astravon.Model.Dtos.Post;
using Astravon.Model.Dtos.User;
using Astravon.Modules.User.Application.Port;

namespace Astravon.Modules.User.Infraestructure.Presenter;

public class PostPresenter : BasePresenter<object>, IPostOutPort
{
    public void NotFound(string message = "Data not found")
    {
        base.NotFound(message);
    }
    
    public void GetAllUsersAsync(IEnumerable<UserDto> data)
    {
        Success(data, "Datos exitosos");
    }
    
    
    public void Login(UserDto data)
    {
        Success(data, "Datos exitosos");
    }

    public void GetPostWithCounts(List<PostWithCountsDto> enumerable)
    {
        Success(enumerable,"Datos exitosos");
    }

    public void GetLikeById(LikeDto data)
    {
        Success(data,"data");
    }

    public void GetPostById(PostDto data)
    {
        Success(data,"data");
    }

    public void GetCommentById(CommentDto data)
    {
        Success(data,"data");
    }

    public void GetCommentByPost(List<CommentDto> data)
    {
        Success(data,"data");
    }
}