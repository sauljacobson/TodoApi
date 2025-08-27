using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Dtos;
using TodoApi.Extensions;
using TodoApi.Interfaces;
using TodoApi.Models;
namespace TodoApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<TodoUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<TodoUser> _signInManager;

        public UserController(UserManager<TodoUser> userManager, ITokenService tokenService, SignInManager<TodoUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCurrentLoggedInUser()
        {
            var username = User.GetUsername();
 
            var todoUser = await _userManager.FindByNameAsync(username);

            return Ok(new DisplayUserDto
            {
                Username = todoUser.UserName,
                Email = todoUser.Email
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginUserDto dto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(user => user.UserName == dto.Username.ToLower());

            if (user == null)
            {
                return Unauthorized("Invalid username or password!");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized("Invalid username or password!");
            }

            return Ok(new NewUserDto
            {
                    Username = user.UserName,
                    Email = user.Email, 
                    Token = _tokenService.CreateToken(user)
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto dto)
        {
            var user = new TodoUser
            {
                UserName = dto.Username,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                return Ok(new NewUserDto
                {
                    Username = user.UserName,
                    Email = user.Email, 
                    Token = _tokenService.CreateToken(user)
                });
            }

            return BadRequest(result.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }
    }
}