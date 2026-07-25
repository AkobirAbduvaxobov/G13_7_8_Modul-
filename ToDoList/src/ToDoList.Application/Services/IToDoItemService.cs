using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoList.Application.Services
{
    public interface IToDoItemService
    {
        Task<IEnumerable<object>> GetAllAsync(); 
    }
}