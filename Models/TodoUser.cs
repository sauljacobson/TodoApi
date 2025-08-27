using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TodoApi.Models
{
    public class TodoUser : IdentityUser<int>
    {

        public List<TodoItem> TodoItem { get; set; }
    }
}