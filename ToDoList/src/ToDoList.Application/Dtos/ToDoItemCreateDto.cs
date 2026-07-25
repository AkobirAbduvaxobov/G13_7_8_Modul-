using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Dtos;

public  class ToDoItemCreateDto
{
    [Required(ErrorMessage = "Title kiritilishi shart!")]
    [MaxLength(200, ErrorMessage = "Title 200 ta belgidan oshmasligi kerak!")]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public PriorityLevel Priority { get; set; } = PriorityLevel.Low; 

    public DateTime? DueDate { get; set; }

    public DateTime? ReminderAt { get; set; }
}
