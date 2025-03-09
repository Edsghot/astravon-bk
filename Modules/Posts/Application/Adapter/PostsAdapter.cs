using System.Net;
using Astravon.HUb;
using Astravon.Model.Dtos.Comment;
using Astravon.Model.Dtos.Likes;
using Astravon.Model.Dtos.Post;
using Astravon.Model.Dtos.Teacher;
using Astravon.Model.Dtos.User;
using Astravon.Modules.User.Application.Port;
using Astravon.Modules.User.Domain.Entity;
using Astravon.Modules.User.Domain.IRepository;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MailKit.Net.Smtp;
using Mapster;
using Microsoft.AspNetCore.SignalR;
using MimeKit;
using Org.BouncyCastle.Crypto.Generators;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommentDto = Astravon.Model.Dtos.Comment.CommentDto;


namespace Astravon.Modules.Posts.Application.Adapter;

public class PostAdapter: IPostInputPort
{
    private readonly IPostRepository _postRepository;
    private readonly IPostOutPort _postOutPort;
    private readonly string _smtpServer = "smtp.gmail.com";
    private readonly int _smtpPort = 587;
    private readonly string _smtpUser = "edsghotSolutions@gmail.com";
    private readonly string _smtpPass = "lfqpacmpmnvuwhvb";
    private readonly Cloudinary _cloudinary;
    private readonly IHubContext<AstravonHub> _hubContext;
    private readonly DateTime _peruDateTime;



    public PostAdapter(IPostRepository repository,IPostOutPort OutPort,IHubContext<AstravonHub> hubContext)
    {
        _postRepository = repository;
        _postOutPort = OutPort;
        var account = new Account("dd0qlzyyk", "952839112726724", "7fxZGsz7Lz2vY5Ahp6spldgMTW4");
        _cloudinary = new Cloudinary(account);
        _hubContext = hubContext;
        var peruTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
        _peruDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, peruTimeZone);
    }


    public async Task CreateComment(CreateCommentDto request)
    {
        var post = await _postRepository.GetAsync<PostEntity>(x => x.Id == request.PostId);
        if(post == null)
        {
            _postOutPort.Error("No se encontro la publicacion");
            return;
        }
        
        var user = await _postRepository.GetAsync<UserEntity>(x => x.Id == request.UserId);
        if(user == null)
        {
            _postOutPort.Error("No se encontro el usuario a crear");
            return;
        }
        var comment = request.Adapt<CommentEntity>();
        comment.DateComment = _peruDateTime;
        await _postRepository.AddAsync(comment);
        _postOutPort.Ok("Su comentario fue agregado con exito");
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Sistema", $"Nuevo usuario creado: {user.FirstName} {user.LastName}");
    }
    
    public async Task CreatePost(CreatePostDto request)
    {
        var user = await _postRepository.GetAsync<UserEntity>(x => x.Id == request.UserId);
        if (user == null)
        {
            _postOutPort.Error("No se encontro el usuario a crear");
            return;
        }

        var post = request.Adapt<PostEntity>();
        post.PublicationDate = _peruDateTime;

        if (request.MediaFile != null)
        {
            post.UrlMedia = await UploadMediaAsync(request.MediaFile, "posts");
        }

        await _postRepository.AddAsync(post);
        _postOutPort.Ok("Su publicación fue agregada con éxito");
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Sistema", $"Nueva publicación creada por: {user.FirstName} {user.LastName}");
    }
    
    public async Task CreateLike(CreateLikeDto request)
    {
        var post = await _postRepository.GetAsync<PostEntity>(x => x.Id == request.PostId);
        if (post == null)
        {
            _postOutPort.Error("No se encontro la publicacion");
            return;
        }

        var user = await _postRepository.GetAsync<UserEntity>(x => x.Id == request.UserId);
        if (user == null)
        {
            _postOutPort.Error("No se encontro el usuario a crear");
            return;
        }

        var like = request.Adapt<LikeEntity>();
        await _postRepository.AddAsync(like);
        _postOutPort.Ok("Su like fue agregado con éxito");
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Sistema", $"Nuevo like agregado por: {user.FirstName} {user.LastName}");
    }
    
    public async Task UpdateComment(UpdateCommentDto request)
    {
        var comment = await _postRepository.GetAsync<CommentEntity>(x => x.Id == request.Id);
        if (comment == null)
        {
            _postOutPort.Error("No se encontró el comentario");
            return;
        }

        comment.Content = request.Content ?? comment.Content;
        await _postRepository.UpdateAsync(comment);
        _postOutPort.Ok("Comentario actualizado con éxito");
    }
    public async Task GetCommentById(int commentId)
    {
        var comment = await _postRepository.GetAsync<CommentEntity>(x => x.Id == commentId);
        if (comment == null)
        {
            _postOutPort.Error("No se encontró el comentario");
            return;
        }

        var response =  comment.Adapt<CommentDto>();
        _postOutPort.GetCommentById(response);
    }
    public async Task DeleteComment(int commentId)
    {
        var comment = await _postRepository.GetAsync<CommentEntity>(x => x.Id == commentId);
        if (comment == null)
        {
            _postOutPort.Error("No se encontró el comentario");
            return;
        }

        await _postRepository.DeleteAsync(comment);
        _postOutPort.Ok("Comentario eliminado con éxito");
    }
    
    public async Task GetPostWithCountsById()
    {
        var posts = await _postRepository.GetAllAsync<PostEntity>();
        var postsWithCounts = new List<PostWithCountsDto>();
        foreach (var post in posts)
        {
            var likeCount = await _postRepository.CountAsync<LikeEntity>(x => x.PostId == post.Id);
            var commentCount = await _postRepository.CountAsync<CommentEntity>(x => x.PostId == post.Id);

            var postWithCounts = post.Adapt<PostWithCountsDto>();
            postWithCounts.LikeCount = likeCount;
            postWithCounts.CommentCount = commentCount;
            postsWithCounts.Add(postWithCounts);
        }
       
        _postOutPort.GetPostWithCounts(postsWithCounts);
    }

 

    // Post CRUD
    

    public async Task UpdatePost(UpdatePostDto request)
    {
        var post = await _postRepository.GetAsync<PostEntity>(x => x.Id == request.Id);
        if (post == null)
        {
            _postOutPort.Error("No se encontró la publicación");
            return;
        }

        post.Content = request.Content ?? post.Content;
        post.PostUrl = request.PostUrl ?? post.PostUrl;
        post.UpdatePost = _peruDateTime;

        if (request.MediaFile != null)
        {
            post.UrlMedia = await UploadMediaAsync(request.MediaFile, "posts");
        }

        await _postRepository.UpdateAsync(post);
        _postOutPort.Ok("Publicación actualizada con éxito");
    }

    public async Task DeletePost(int postId)
    {
        var post = await _postRepository.GetAsync<PostEntity>(x => x.Id == postId);
        if (post == null)
        {
            _postOutPort.Error("No se encontró la publicación");
            return;
        }

        await _postRepository.DeleteAsync(post);
        _postOutPort.Ok("Publicación eliminada con éxito");
    }

    public async Task GetPostById(int postId)
    {
        var post = await _postRepository.GetAsync<PostEntity>(x => x.Id == postId);
        if (post == null)
        {
            _postOutPort.Error("No se encontró la publicación");
            return;
        }

        var response =  post.Adapt<PostDto>();
        _postOutPort.GetPostById(response);
    }

    // Like CRUD


    public async Task DeleteLike(int likeId)
    {
        var like = await _postRepository.GetAsync<LikeEntity>(x => x.Id == likeId);
        if (like == null)
        {
            _postOutPort.Error("No se encontró el like");
            return;
        }

        await _postRepository.DeleteAsync(like);
        _postOutPort.Ok("Like eliminado con éxito");
    }

    public async Task GetLikeById(int likeId)
    {
        var like = await _postRepository.GetAsync<LikeEntity>(x => x.Id == likeId);
        if (like == null)
        {
            _postOutPort.NotFound("No se encontró el like");
            return;
        }

        var response =  like.Adapt<LikeDto>();
        
        _postOutPort.GetLikeById(response);
    }
    
    public async Task GetCommentByPost(int postId)
    {
        var comments = await _postRepository.GetAllAsync<CommentEntity>(x => x.Where(x => x.PostId == postId));
        
        var response =  comments.Adapt<List<CommentDto>>();
        
        _postOutPort.GetCommentByPost(response);
    }

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    private async Task<string> UploadMediaAsync(IFormFile file, string folder)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("El archivo no puede estar vacío.");

        var fileExtension = Path.GetExtension(file.FileName).ToLower();
        var allowedImageFormats = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
        var allowedVideoFormats = new[] { ".mp4", ".avi", ".mov", ".mkv", ".wmv" };

        var isImage = allowedImageFormats.Contains(fileExtension);
        var isVideo = allowedVideoFormats.Contains(fileExtension);

        await using var stream = file.OpenReadStream();
    
        ImageUploadParams uploadParams;

        if (isImage)
        {
            uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folder,
                Transformation = new Transformation().Width(500).Height(500).Crop("fill")
            };
        }
        else if (isVideo)
        {
            uploadParams = new VideoUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folder,
                Transformation = new Transformation().Width(1080).Height(720).Crop("limit")
            };
        }
        else
        {
            var rawUploadParams = new RawUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folder
            };
            var uploadResult = await _cloudinary.UploadAsync(rawUploadParams);
            return uploadResult.StatusCode == HttpStatusCode.OK ? uploadResult.Url.AbsoluteUri : string.Empty;
        }

        var uploadResult2 = await _cloudinary.UploadAsync(uploadParams);
   
        return uploadResult2.StatusCode == HttpStatusCode.OK ? uploadResult2.Url.AbsoluteUri : string.Empty;
    }

    /*
     public async Task GetAllUsersAsync()
     {
         var users = await _userRepository.GetAllAsync<UserEntity>();

         if (!users.Any())
         {
             _userOutPort.NotFound("No users found.");
             return;
         }

         var userDtos = users.Select(user => new UserDto
         {
             Id = user.Id,
             FirstName = user.FirstName,
             LastName = user.LastName,
             Email = user.Mail
         });
         
         _userOutPort.GetAllUsersAsync(userDtos);

     }
     
     public async Task Login(LoginDto loginRequest)
     {
         var user = await _userRepository.GetAsync<UserEntity>(x =>
             x.Mail == loginRequest.Email && x.Password == loginRequest.Password);
         if (user == null)
         {
             _userOutPort.Error("Revise bien tus credenciales");
             return;
         }

         var teacher = user.Adapt<UserDto>();

         _userOutPort.Login(teacher);
     }
     


    public async Task SendVerificationEmailAsync(string toEmail)
    {
        var code = GenerateCode();

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Astravon", _smtpUser));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = "Código de verificación";

        message.Body = new TextPart("html")
        {
            Text = $"<h2>Bievenido a Astravon<b></b> Tu código de verificación es: <b>{code}</b></h2><p>Este código expira en unos minutos.</p>"
        };

        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_smtpUser, _smtpPass);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            var validatioon = await _userRepository.GetAsync<ValidateEntity>(x => x.Email == toEmail);
            if (validatioon != null)
            {
                validatioon.Code = code;
                await _userRepository.UpdateAsync(validatioon);
            }
            else
            {
                var verification = new ValidateEntity
                {
                    Email = toEmail,
                    Code = code
                };
                await _userRepository.AddAsync(verification);
            }

            _userOutPort.Ok("Código de verificación enviado");
        }
        catch (Exception ex)
        {
            _userOutPort.Error("Error: " + ex.Message);
        }
    }

    public async Task ValidateCode(string email, string inputCode)
    {
        var data = await _userRepository.GetAsync<ValidateEntity>(x => x.Email == email && x.Code == inputCode);

        if (data == null)
        {
            _userOutPort.Error("El codigo es incorrecto intente de nuevo!");
            return;
        }

        _userOutPort.Ok("El codigo es correcto");
    }
    
    */
    
    #region Private Methods
    
    private string GenerateCode()
    {
        var random = new Random();
        var code = random.Next(100000, 999999).ToString();
        return code;
    }    
    #endregion
    
    /*
     *   private async Task<string> UploadImage(IFormFile file, string folder)
    {
        await using var streamCover = file.OpenReadStream();
        var uploadParamsCover = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, streamCover),
            Transformation = new Transformation().Width(500).Height(500).Crop("fill"),
            Folder = folder
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParamsCover);

        if (uploadResult.StatusCode == HttpStatusCode.OK) return uploadResult.Url.AbsoluteUri;
        return "";
    }
     * 
     */
}