using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.Commands.CreateToDoItem;

namespace ToDoList.Api.Controllers;

[Route("api/v1/todoitems1")]
[ApiController]
public class ToDoItem1sController : ControllerBase
{
    private readonly IMediator _mediator;

    public ToDoItem1sController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<long>> Create(CreateToDoItemCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    //[HttpGet("{id}")]
    //public async Task<ActionResult<BookDto>> GetById(int id)
    //{
    //    // We'll implement query handler for this later
    //    return Ok();
    //}
}
