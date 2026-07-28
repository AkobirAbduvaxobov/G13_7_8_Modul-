using MediatR;
using ToDoList.Application.Abstractions;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Commands.CreateToDoItem;

public class CreateToDoItemHandler : IRequestHandler<CreateToDoItemCommand, long>
{
    private readonly IBaseCommandRepository<ToDoItem> _repository;
    public CreateToDoItemHandler(IBaseCommandRepository<ToDoItem> repository)
    {
        _repository = repository;
    }

    public async Task<long> Handle(CreateToDoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = new ToDoItem
        {
            Title = request.Title,
            Description = request.Description,
            Priority = request.Priority,
            DueDate = request.DueDate,
            ReminderAt = request.ReminderAt
        };

        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();

        return entity.ToDoItemId;
    }
}
