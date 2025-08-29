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
                .FromSqlInterpolated($"CALL GetTodosByUserId({todoUserId});")
                .ToList()
                .Select(todo => new SummaryTodoDto
                {
                    Id = todo.Id,
                    Title = todo.Title,
                    IsDone = todo.IsDone
                });

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

            var result = await _context.Database
                .ExecuteSqlInterpolatedAsync($"CALL AddNewTodo({dto.Title}, {todoUserId})");

            if (result == 1)
                return Ok(todoItem); 
            
            return BadRequest("Insert failed");
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

            var rowsAffected = await _context.Database
                .ExecuteSqlInterpolatedAsync($"CALL UpdateTodo({id}, {dto.Title}, {dto.IsDone}, {todoUserId});");

            if (rowsAffected == 1 || rowsAffected == 0)
                return NoContent();

            return BadRequest("Update Failed!");
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

            var rowsAffected = await _context.Database
                .ExecuteSqlInterpolatedAsync($"CALL DeleteTodo({id}, {todoUserId});");

            if (rowsAffected == 1)
                return Ok("Todo deleted");

            return BadRequest("Deletion failed"); 
        }
    }
}