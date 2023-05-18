using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrackSpense.BL.Contracts;
using TrackSpense.DAL.Models;
using TrackSpense.Server.DTOs;
using TrackSpense.Shared.BusinessModels;
using TrackSpense.Shared.JWTTokenSettings;

namespace TrackSpense.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{


    //token
    private readonly TokenSettings _tokenSettings;

    private readonly IUserService _userService;
    private static IConfiguration _config;
    public UserController(IUserService userService, IOptions<TokenSettings> tokenSettings)
    {
        _userService = userService;
        _tokenSettings = tokenSettings.Value;
    }

    //token 2
   /* public UserController(IOptions<TokenSettings> tokenSettings)
    { 
        _tokenSettings = tokenSettings.Value;
    }*/

    [HttpPost("Register")]
    public async Task<bool> RegisterUser([FromBody]Business_User user)
    {
        var UsernameExists = await IsUsernameExists(user.UserName);
        //Encrypting password using BCrpt
        user.Password=BCrypt.Net.BCrypt.HashPassword(user.Password);
        if (UsernameExists == false)
        {   
            await _userService.Add(new Business_User()
            {
                UserId = "",
                UserName = user.UserName,
                Password = user.Password,
                Email = user.Email
            });
        }
        else
        {
            return false;
        }
        return true;
    }

    [HttpPost("Login")]
    public async Task<JWTTokenResponseDto> LoginUser([FromBody] Business_User user)
    {
        var UsernameExists = await IsUsernameExists(user.UserName);
        if (UsernameExists == false) return null; 

        Business_User business_User = await _userService.Get(user.UserName,user.Password);

        //returning a token
        string jwtAccessToken = GenerateJwtAccessToken(business_User);
        var result = new JWTTokenResponseDto
        {
            AccessToken = jwtAccessToken,
        };
         
        return result;

    }

    [HttpGet("IsUsernameExists")]
    public async Task<bool> IsUsernameExists(string Username)
    {
        return await _userService.GetByUserName(Username);
    }

    //token creation

    [HttpPost("GenerateJwtAcessToken")]
    public string GenerateJwtAccessToken(Business_User user)
    {
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.SecretKey));

        var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>();
        claims.Add(new Claim("Id", user.UserId.ToString()));
        claims.Add(new Claim("UserName", user?.UserName ?? string.Empty));
        claims.Add(new Claim("Email", user?.Email ?? string.Empty));

        var securityToken = new JwtSecurityToken(
            issuer: _tokenSettings.Issuer,
            audience: _tokenSettings.Audience,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials,
            claims: claims);

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}
