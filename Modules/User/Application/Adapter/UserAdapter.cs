using Astravon.HUb;
using Astravon.Model.Dtos.Teacher;
using Astravon.Model.Dtos.User;
using Astravon.Modules.User.Application.Port;
using Astravon.Modules.User.Domain.Entity;
using Astravon.Modules.User.Domain.IRepository;
using CloudinaryDotNet;
using MailKit.Net.Smtp;
using Mapster;
using Microsoft.AspNetCore.SignalR;
using MimeKit;
using Org.BouncyCastle.Crypto.Generators;
using BCrypt.Net;


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
    private readonly IHubContext<AstravonHub> _hubContext;



    public UserAdapter(IUserRepository repository,IUserOutPort userOutPort,IHubContext<AstravonHub> hubContext)
    {
        _userRepository = repository;
        _userOutPort = userOutPort;
        var account = new Account("dd0qlzyyk", "952839112726724", "7fxZGsz7Lz2vY5Ahp6spldgMTW4");
        _cloudinary = new Cloudinary(account);
        _hubContext = hubContext;
        
    }


    public async Task CreateUser(CreateUserDto createUserRequest)
    {
        // Verificar si el correo ya está registrado
        var userMail = await _userRepository.GetAsync<UserEntity>(x => x.Mail == createUserRequest.Mail);
        if (userMail != null)
        {
            _userOutPort.Error("El correo ya está registrado");
            return;
        }

        // Mapear DTO a Entidad
        var user = createUserRequest.Adapt<UserEntity>();

        // Hashear la contraseña de forma segura con BCrypt
        user.Password = BCrypt.Net.BCrypt.HashPassword(createUserRequest.Password, workFactor: 12);

        // Guardar el usuario en la base de datos
        await _userRepository.AddAsync(user);

        // Respuesta de éxito
        _userOutPort.Ok("Usuario creado correctamente");

        // Notificar a los clientes en tiempo real con SignalR
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Sistema", $"Nuevo usuario creado: {user.FirstName} {user.LastName}");
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
             Mail = user.Mail
         });
         
         _userOutPort.GetAllUsersAsync(userDtos);

     }
     
     public async Task Login(LoginDto loginRequest)
     {
         var user = await _userRepository.GetAsync<UserEntity>(x => x.Mail == loginRequest.Mail);
         if (user == null)
         {
             _userOutPort.Error("Correo o contraseña incorrectos.");
             return;
         }

         // Verificar la contraseña correctamente
         if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
         {
             _userOutPort.Error("Correo o contraseña incorrectos.");
             return;
         }
         var userDto = user.Adapt<UserDto>();
         _userOutPort.Login(userDto);
     }



    public async Task SendVerificationEmailAsync(string toEmail)
    {
        var code = GenerateCode();
        var nameGmail = toEmail.Split('@')[0].ToString();

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Astravon", _smtpUser));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = "Código de verificación "+code;

        message.Body = new TextPart("html")
        {
            Text = $"<div\n  style=\"\n    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;\n    background-color: #f4f4f4;\n    margin: 0;\n    padding: 0;\n  \"\n>\n  <div\n    style=\"\n      max-width: 600px;\n      margin: 0 auto;\n      background-color: #ffffff;\n      border-radius: 8px;\n      box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);\n    \"\n  >\n    <div\n      style=\"\n        display: flex;\n        align-items: center;\n        justify-content: flex-end;\n        padding: 20px;\n        background-color: #f0f0f0;\n        border-bottom: 2px solid #f0f0f0;\n      \"\n    >\n      <img\n        src=\"https://static.vecteezy.com/system/resources/previews/019/514/652/non_2x/letter-a-logo-design-for-business-and-company-identity-with-luxury-concept-free-vector.jpg\"\n        alt=\"Astravon Logo\"\n        style=\"\n          margin-right: 10px;\n          width: 50px;\n          height: auto;\n          border-radius: 20%;\n        \"\n      />\n      <h1 style=\"font-size: 24px; margin: 0; font-weight: bolder\">\n        <span style=\"color: #6d4510\">Astravon</span>\n      </h1>\n    </div>\n    <div style=\"padding: 20px; text-align: justify\">\n      <h2 style=\"font-size: 22px; color: #333333\">\n        <span style=\"color: #b77e1b\">Hola, </span> {nameGmail}\n      </h2>\n      <p style=\"font-size: 16px; color: #555555\">\n        \u2705 ¡Gracias por registrarte para obtener una cuenta en JhedGost! \u2b50\n        Antes de comenzar, necesitamos confirmar tu identidad. Por favor, copia\n        el siguiente código e introdúcelo en la aplicación para verificar tu\n        dirección de correo electrónico: \u2b07\ud83d\udd11\n      </p>\n      <div\n        style=\"\n          background-color: #f0f0f0;\n          padding: 5px;\n          border-radius: 8px;\n          margin-top: 10px;\n          text-align: center;\n          font-size: 24px;\n          font-weight: bold;\n          color: #333333;\n        \"\n      >\n        <p>{code}</p>\n      </div>\n    </div>\n    <div\n      style=\"\n        text-align: center;\n        padding: 20px;\n        background-color: #7c4c27;\n        border-top: 2px solid #f0f0f0;\n      \"\n    >\n      <p style=\"font-size: 14px; color: #fff; margin: 10px 0\">\n        Nunca pares de aprender,<br />\n        Team JhedGost.\n      </p>\n      <img\n        src=\"https://static.vecteezy.com/system/resources/previews/019/514/652/non_2x/letter-a-logo-design-for-business-and-company-identity-with-luxury-concept-free-vector.jpg\"\n        alt=\"Astravon Logo\"\n        style=\"\n          margin-top: 20px;\n          width: 50px;\n          height: 50px;\n          border-radius: 50%;\n          object-fit: cover;\n        \"\n      />\n      <p style=\"font-size: 14px; color: #fff; margin: 10px 0\">\n        Enviado por JHEDGOST Av. Manuel Olguin Nro. 325, Abancay, Perú 2024\n      </p>\n    </div>\n\u00a0\u00a0</div>\n</div>"
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
        var code = random.Next(1000, 9999).ToString();
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