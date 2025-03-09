using Astravon.Model.Dtos.Teacher;
using Astravon.Model.Dtos.User;
using Astravon.Modules.User.Application.Port;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Astravon.Modules.User.Infraestructure.Controller;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserInputPort _userInputPort;
    private readonly IUserOutPort _userOutPort;

    public UserController(IUserInputPort userInputPort, IUserOutPort userOutPort)
    {
        _userInputPort = userInputPort;
        _userOutPort = userOutPort;
    }
    
    [HttpPost("createUser")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto data)
    {
        await _userInputPort.CreateUser(data);
        var response = _userOutPort.GetResponse;
        return Ok(response);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto data)
    {
        await _userInputPort.Login(data);
        var response = _userOutPort.GetResponse;
        return Ok(response);
    }


    [HttpGet]
    public async Task<IActionResult> Get()
    {
        await _userInputPort.GetAllUsersAsync();
        var response = _userOutPort.GetResponse;

        return Ok(response);
    }
    [HttpPost("SendCodeValidation/{email}")]
    public async Task<IActionResult> SendCodeValidation([FromRoute] string email)
    {
        await _userInputPort.SendVerificationEmailAsync(email);
        var response = _userOutPort.GetResponse;
        return Ok(response);
    }

    [HttpGet("ValidateMail")]
    public async Task<IActionResult> ValidateEmail([FromQuery] ValidateDto data)
    {
        await _userInputPort.ValidateCode(data.Email, data.Code);
        var response = _userOutPort.GetResponse;
        return Ok(response);
    }
   
}