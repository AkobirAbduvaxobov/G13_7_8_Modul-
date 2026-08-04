using FluentAssertions;
using Moq;
using System.Net;
using System.Net.Http.Json;
using ToDoList.Application.Abstractions;
using ToDoList.Application.Dtos;
using ToDoList.Domain.Entities;

namespace IntegrationTests;

public class ToDoItemsControllerIntegrationTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ToDoItemsControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateAuthenticatedClient();
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/todoitems");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<PagedResult<ToDoItemGetDto>>();

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetById_ShouldReturnOk()
    {
        // Arrange
        var createDto = new ToDoItemCreateDto
        {
            Title = "Integration Test",
            Description = "Description"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/v1/todoitems", createDto);

        var created = await createResponse.Content
            .ReadFromJsonAsync<ToDoItemGetDto>();

        // Act
        var response = await _client.GetAsync($"/api/v1/todoitems/{created!.ToDoItemId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var todo = await response.Content.ReadFromJsonAsync<ToDoItemGetDto>();

        todo.Should().NotBeNull();
        todo!.ToDoItemId.Should().Be(created.ToDoItemId);
    }

    [Fact]
    public async Task Create_ShouldReturnCreated()
    {
        // Arrange

        var currentUserMock = new Mock<ICurrentUserService>();

        currentUserMock.Setup(x => x.UserId).Returns(1L);
        currentUserMock.Setup(x => x.UserName).Returns("integration.user");
        currentUserMock.Setup(x => x.FirstName).Returns("Integration");
        currentUserMock.Setup(x => x.LastName).Returns("Tester");
        currentUserMock.Setup(x => x.Email).Returns("integration@test.com");
        currentUserMock.Setup(x => x.Role).Returns(UserRole.Admin);

        var dto = new ToDoItemCreateDto
        {
            Title = "Learn Integration Tests",
            Description = "Write success test",
            DueDate = DateTime.UtcNow.AddDays(5)
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/todoitems", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await response.Content.ReadFromJsonAsync<ToDoItemGetDto>();

        created.Should().NotBeNull();
        created!.Title.Should().Be(dto.Title);
    }

    [Fact]
    public async Task Update_ShouldReturnOk()
    {
        // Arrange
        var createDto = new ToDoItemCreateDto
        {
            Title = "Old Title"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/v1/todoitems", createDto);

        var created = await createResponse.Content
            .ReadFromJsonAsync<ToDoItemGetDto>();

        var updateDto = new ToDoItemUpdateDto
        {
            Title = "New Title",
            Description = "Updated Description",
            IsCompleted = false,
            Priority = PriorityLevel.High,
            DueDate = DateTime.UtcNow.AddDays(10)
        };

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/v1/todoitems/{created!.ToDoItemId}",
            updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var updated = await response.Content.ReadFromJsonAsync<ToDoItemGetDto>();

        updated.Should().NotBeNull();
        updated!.Title.Should().Be("New Title");
        updated.Priority.Should().Be(PriorityLevel.High);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent()
    {
        // Arrange
        var createDto = new ToDoItemCreateDto
        {
            Title = "Delete Me"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/v1/todoitems", createDto);

        var created = await createResponse.Content
            .ReadFromJsonAsync<ToDoItemGetDto>();

        // Act
        var response = await _client.DeleteAsync(
            $"/api/v1/todoitems/{created!.ToDoItemId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task ToggleComplete_ShouldReturnOk()
    {
        // Arrange
        var createDto = new ToDoItemCreateDto
        {
            Title = "Complete Me"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/v1/todoitems", createDto);

        var created = await createResponse.Content
            .ReadFromJsonAsync<ToDoItemGetDto>();

        // Act
        var response = await _client.PatchAsync(
            $"/api/v1/todoitems/{created!.ToDoItemId}/complete",
            null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var todo = await response.Content.ReadFromJsonAsync<ToDoItemGetDto>();

        todo.Should().NotBeNull();
        todo!.IsCompleted.Should().BeTrue();
    }
}

