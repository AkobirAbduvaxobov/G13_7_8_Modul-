namespace ToDoList.Application.Dtos;

public class ToDoItemStatisticsDto
{
    public int TotalCount { get; set; }
    public int CompletedCount { get; set; }
    public int PendingCount { get; set; }
    public int OverdueCount { get; set; }
    public Dictionary<string, int> CountByPriority { get; set; } = new();
}
