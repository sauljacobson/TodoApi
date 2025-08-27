using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TodoApi.Data;
using TodoApi.Models; 
using TodoApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using TodoApi.Extensions;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<TodoUser> _userManager;
        public TodoController(AppDbContext context, UserManager<TodoUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllSyncTodoItems()
        {
            var username = User.GetUsername();
            var todoUser = await _userManager.FindByNameAsync(username);
            var todoUserId = todoUser.Id;

            var todoItems = _context.TodoItems
                                .Where(item => item.UserId == todoUserId)
                                .ToList();

            return Ok(todoItems);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetTodoItem(int id)
        {
            var username = User.GetUsername();
            var todoUser = await _userManager.FindByNameAsync(username);
            var todoUserId = todoUser.Id; 

            var todoItem = _context.TodoItems.Find(id);

            if (todoItem == null || todoItem.UserId != todoUserId)
            {
                return NotFound();
            }
           
            return Ok(todoItem);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddTodoItem([FromBody] NewTodoDto dto)
        {
            var username = User.GetUsername(); 
            var todoUser = await _userManager.FindByNameAsync(username);
            var todoUserId = todoUser.Id;

            var todoItem = new TodoItem
            {
                Title = dto.Title,
                IsDone = false,
                UserId = todoUserId
            };

            _context.TodoItems.Add(todoItem);
            _context.SaveChanges();

            return Ok(todoItem);
        }

        [HttpPut("{id}")] 
        [Authorize]
        public async Task<IActionResult> UpdateTodoItem(int id, UpdateTodoItemDto dto)
        {
            var username = User.GetUsername(); 
            var todoUser = await _userManager.FindByNameAsync(username);
            var todoUserId = todoUser.Id;

            var todoItem = _context.TodoItems.Find(id); 

            if (todoItem == null || todoItem.UserId != todoUserId)
            {
                return NotFound();
            }

            todoItem.Title = dto.Title;
            todoItem.IsDone = dto.IsDone; 

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            var username = User.GetUsername(); 
            var todoUser = await _userManager.FindByNameAsync(username);
            var todoUserId = todoUser.Id;

            var todoItem = _context.TodoItems.Find(id);

            if (todoItem == null || todoItem.UserId != todoUserId)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            _context.SaveChanges();

            return NoContent();
        }
    }
}