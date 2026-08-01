using Moq;
using ToDoList.Application.Abstractions;
using ToDoList.Application.Services;
using ToDoList.Domain.Entities;

namespace Application.Services;

public class ToDoItemServiceTests
{
    private readonly Mock<IBaseRepository<ToDoItem>> _repository;
    private readonly Mock<ICurrentUserService> _currentUser;
    private readonly ToDoItemService _service;

    public ToDoItemServiceTests()
    {
        _repository = new Mock<IBaseRepository<ToDoItem>>();
        _currentUser = new Mock<ICurrentUserService>();

        _service = new ToDoItemService(
            _repository.Object,
            _currentUser.Object);
    }

    [Fact]
    public async Task DeleteAsync_TodoItemExists_DeletesTodoItem()
    {
        // Arrange

        var todoItem1 = new ToDoItem { ToDoItemId = 1L, UserId = 1L, Title = "Test1", IsCompleted = false };
        var todoItem2 = new ToDoItem { ToDoItemId = 2L, UserId = 2L, Title = "Test2", IsCompleted = true };
        var todoItemId = 1L;

        _currentUser.Setup(c => c.UserId).Returns(1L);

        _repository.Setup(r => r.GetAllQuery()).
            Returns(new List<ToDoItem>
            {
                todoItem1, todoItem2
            }.AsQueryable());

        _repository.Setup(r => r.Update(todoItem1));
        _repository.Setup(r => r.SaveChangesAsync());


        // Act
        await _service.DeleteAsync(todoItemId);

        // Assert
        _repository.Verify(r => r.SaveChangesAsync(), Times.Once);
        _repository.Verify(r => r.Update(todoItem1), Times.Once);
    }

}
