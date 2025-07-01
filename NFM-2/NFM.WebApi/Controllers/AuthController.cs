using Centric.Finance.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NFM.Domain.Models;
using NFM.Domain.Models.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NFM.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtOptions _jwtOptions;

    public AuthController(UserManager<IdentityUser> userManager, IOptions<JwtOptions> jwtOptions)
    {
        _userManager = userManager;
        _jwtOptions = jwtOptions.Value;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var user = await _userManager.FindByEmailAsync(loginRequestDto.Username);
        if (user == null)
        {
            return BadRequest("Invalid username or password!");
        }

        var checkPasswordResult =
            await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
        if (!checkPasswordResult)
        {
            return BadRequest("Invalid username or password!");
        }

        var roles = await _userManager.GetRolesAsync(user);
        if (roles == null ||
            roles.Count == 0)
        {
            return BadRequest("Something went wrong. Please contact administrator.");
        }

        var jwtToken = CreateJwtToken(user, roles.ToList());

        var expirationDate = DateTime.UtcNow.AddMinutes(_jwtOptions.ValidFor.TotalMinutes);
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = expirationDate,
            SameSite = SameSiteMode.None,
            Path = "/"
        };
        Response.Cookies.Append("auth_token", jwtToken, cookieOptions);

        return Ok(jwtToken);
    }


    [HttpPost]
    [Route("logout")]
    public IActionResult LogOut()
    {
        Response.Cookies.Delete("auth_token");
        return Ok();
    }

    [HttpPost]
    [Route("registerAdmin")]
    [Authorize(Roles = AppUserRole.Admin)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest createUserRequest)
    {
        var identityUser = new IdentityUser
        {
            UserName = createUserRequest.Username,
            Email = createUserRequest.Username
        };

        var identityResult =
            await _userManager.CreateAsync(identityUser, createUserRequest.Password);

        if (identityResult.Succeeded)
        {
            identityResult =
                await _userManager.AddToRolesAsync(identityUser, new[] { AppUserRole.Admin });

            if (identityResult.Succeeded)
            {
                return Ok();
            }
        }

        return BadRequest(identityResult.Errors);
    }

    [HttpPost]
    [Route("registerOperator")]
    [Authorize(Roles = $"{AppUserRole.Admin},{AppUserRole.Operator}")]
    public async Task<IActionResult> CreateOperatorUser(
        [FromBody] CreateUserRequest createUserRequest)
    {
        var identityUser = new IdentityUser
        {
            UserName = createUserRequest.Username,
            Email = createUserRequest.Username
        };

        var identityResult =
            await _userManager.CreateAsync(identityUser, createUserRequest.Password);

        if (identityResult.Succeeded)
        {
            identityResult =
                await _userManager.AddToRolesAsync(identityUser, new[] { AppUserRole.Operator });

            if (identityResult.Succeeded)
            {
                return Ok();
            }
        }

        return BadRequest(identityResult.Errors);
    }

    private string CreateJwtToken(IdentityUser user, List<string> roles)
    {
        // Create Claims
        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.Email, user.Email));
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = new JwtSecurityToken(_jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: _jwtOptions.SigningCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}