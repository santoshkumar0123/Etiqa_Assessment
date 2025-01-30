using Etiqa_Assessment_REST_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
namespace Etiqa_Assessment_REST_API.Repository
{
    public interface IUserRepository
    {
        Task<PaginatedList<User>> GetUsersAsync(int pageIndex, int pageSize);
        Task<User> AddUserAsync(User objUserDetails);
        Task UpdateUserAsync(User objUserDetails);
        Task DeleteUserAsync(string username);
        Task<IEnumerable<User>> WildcardSearchAsync(string username, string mail);
        Task ArchiveUserAsync(string username);
        Task UnarchiveUserAsync(string username);
    }
}
