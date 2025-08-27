using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Dtos
{
    public class NewTodoDto
    {
        [Required]
        public string Title { get; set; }
    }
}