using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Dtos
{
    public class SummaryTodoDto
    {
        public int Id { get; set;}
        public string Title { get; set; }
        public bool IsDone { get; set; }
    }
}