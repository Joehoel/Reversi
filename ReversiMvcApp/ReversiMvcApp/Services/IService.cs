using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReversiMvcApp.Services
{
    public interface IService<T>
    {
        Task<List<T>> GetAsync(string path);
        Task<T> GetAsync(string id, string path);
        Task<bool> AddAsync(T item, string path);
        Task<bool> UpdateAsync(string id, T item, string path);
        Task<bool> UpdateSpecialAsync(string id, object item, string path);
        Task<bool> DeleteAsync(string id, string path);
    }
}
