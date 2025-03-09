using Astravon.Model.Dtos.Comment;
using Astravon.Model.Dtos.Likes;
using Astravon.Model.Dtos.Post;
using Astravon.Model.Dtos.Teacher;
using Astravon.Model.Dtos.User;
using Astravon.Modules.User.Application.Port;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Astravon.Modules.User.Infraestructure.Controller;

[Route("api/[controller]")]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly IPostInputPort _postInputPort;
    private readonly IPostOutPort _postOutPort;

    public PostsController(IPostInputPort postInputPort, IPostOutPort postOutPort)
    {
        _postInputPort = postInputPort;
        _postOutPort = postOutPort;
    }
    [HttpPost("createComment")]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentDto data)
    {
        await _postInputPort.CreateComment(data);
        var response = _postOutPort.GetResponse;
        return Ok(response);
    }

    [HttpPut("updateComment")]
    public async Task<IActionResult> UpdateComment([FromBody] UpdateCommentDto data)
    {
        await _postInputPort.UpdateComment(data);
        var response = _postOutPort.GetResponse;
        return Ok(response);
    }

    [HttpDelete("deleteComment/{commentId}")]
    public async Task<IActionResult> DeleteComment(int commentId)
    {
        await _postInputPort.DeleteComment(commentId);
        var response = _postOutPort.GetResponse;
        return Ok(response);
    }

    [HttpGet("getCommentById/{commentId}")]
    public async Task<IActionResult> GetCommentById(int commentId)
    {
        var comment = await _postInputPort.GetCommentById(commentId);
        if (comment == null)
        {
            return NotFound();
        }
        return Ok(comment);
    }

    // Post Endpoints
    [HttpPost("createPost")]
    public async Task<IActionResult> CreatePost([FromForm] CreatePostDto data)
    {
        await _postInputPort.CreatePost(data);
        var response = _postOutPort.GetResponse;
        return Ok(response);
    }

    [HttpPut("updatePost")]
    public async Task<IActionResult> UpdatePost([FromForm] UpdatePostDto data)
    {
        await _postInputPort.UpdatePost(data);
        var response = _postOutPort.GetResponse;
        return Ok(response);
    }

    [HttpDelete("deletePost/{postId}")]
    public async Task<IActionResult> DeletePost(int postId)
    {
        await _postInputPort.DeletePost(postId);
        var response = _postOutPort.GetResponse;
        return Ok(response);
    }

    [HttpGet("getPostById/{postId}")]
    public async Task<IActionResult> GetPostById(int postId)
    {
        var post = await _postInputPort.GetPostById(postId);
        if (post == null)
        {
            return NotFound();
        }
        return Ok(post);
    }

    // Like Endpoints
    [HttpPost("createLike")]
    public async Task<IActionResult> CreateLike([FromBody] CreateLikeDto data)
    {
        await _postInputPort.CreateLike(data);
        var response = _postOutPort.GetResponse;
        return Ok(response);
    }

    [HttpDelete("deleteLike/{likeId}")]
    public async Task<IActionResult> DeleteLike(int likeId)
    {
        await _postInputPort.DeleteLike(likeId);
        var response = _postOutPort.GetResponse;
        return Ok(response);
    }

    [HttpGet("getLikeById/{likeId}")]
    public async Task<IActionResult> GetLikeById(int likeId)
    {
        var like = await _postInputPort.GetLikeById(likeId);
        if (like == null)
        {
            return NotFound();
        }
        return Ok(like);
    }
    
    [HttpGet("getPostWithCountsById")]
    public async Task<IActionResult> GetPostWithCountsById()
    {
        var postWithCounts = await _postInputPort.GetPostWithCountsById();
        
        return Ok(postWithCounts);
    }
   
}