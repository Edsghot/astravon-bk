using Astravon.HUb;
using Astravon.Model.Dtos.Comment;
using Astravon.Model.Dtos.Likes;
using Astravon.Model.Dtos.Post;
using Astravon.Model.Dtos.Teacher;
using Astravon.Model.Dtos.User;
using Astravon.Modules.User.Application.Port;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Astravon.Modules.User.Infraestructure.Controller;

[Route("api/[controller]")]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly IHubContext<PostHub> _hubContext;
    private readonly IPostInputPort _postInputPort;
    private readonly IPostOutPort _postOutPort;

    public PostsController(IPostInputPort postInputPort, IPostOutPort postOutPort,IHubContext<PostHub> hubContext)
    {
        _postInputPort = postInputPort;
        _postOutPort = postOutPort;
        _hubContext = hubContext;
    }
    [HttpPost("createComment")]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentDto data)
    {
        await _postInputPort.CreateComment(data);
        await _hubContext.Clients.All.SendAsync("RefreshPosts");
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
        await _postInputPort.GetCommentById(commentId);
        var response = _postOutPort.GetResponse;

        return Ok(response);
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
        await _postInputPort.GetPostById(postId);
        var response = _postOutPort.GetResponse;

        return Ok(response);
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
        await _postInputPort.GetLikeById(likeId);
        var response = _postOutPort.GetResponse;

        return Ok(response);
    }
    
    [HttpGet("getAllPostWithCounts")]
    public async Task<IActionResult> GetAllPostWithCounts()
    {
         await _postInputPort.GetPostWithCountsById();
        var response = _postOutPort.GetResponse;

        return Ok(response);
    }
    
    [HttpGet("getAllCommentByPost/{postId}")]
    public async Task<IActionResult> getAllCommentByPost([FromRoute] int postId)
    {
        await _postInputPort.GetCommentByPost(postId);
        var response = _postOutPort.GetResponse;

        return Ok(response);
    }
   
}