using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Dtos
{
    public class UpdateTodoItemDto
    {
        public string? Title { get; set; }
        public bool IsDone { get; set; }
    }
}