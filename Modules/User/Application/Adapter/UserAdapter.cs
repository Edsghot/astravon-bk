using Astravon.Model.Dtos.Teacher;
using Astravon.Modules.User.Application.Port;
using Astravon.Modules.User.Domain.Entity;
using Astravon.Modules.User.Domain.IRepository;
using CloudinaryDotNet;
using MailKit.Net.Smtp;
using Mapster;
using MimeKit;
using UserDto = Astravon.Model.Dtos.User.UserDto;

namespace Astravon.Modules.User.Application.Adapter;

public class UserAdapter: IUserInputPort
{
    private readonly IUserRepository _userRepository;
    private readonly IUserOutPort _userOutPort;
    private readonly string _smtpServer = "smtp.gmail.com";
    private readonly int _smtpPort = 587;
    private readonly string _smtpUser = "edsghotSolutions@gmail.com";
    private readonly string _smtpPass = "lfqpacmpmnvuwhvb";
    private readonly Cloudinary _cloudinary;



    public UserAdapter(IUserRepository repository,IUserOutPort userOutPort)
    {
        _userRepository = repository;
        _userOutPort = userOutPort;
        var account = new Account("dd0qlzyyk", "952839112726724", "7fxZGsz7Lz2vY5Ahp6spldgMTW4");
        _cloudinary = new Cloudinary(account);
    }
    
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