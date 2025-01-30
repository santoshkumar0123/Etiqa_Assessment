using Etiqa_Assessment_REST_API.Controllers;
using Etiqa_Assessment_REST_API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Numerics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Etiqa_Assessment_REST_API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _userDbContext;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(UserDbContext userDbContext, ILogger<UserRepository> logger)
        {
            _userDbContext = userDbContext ??
                throw new ArgumentNullException(nameof(userDbContext));
            _logger = logger;
        }

        public async Task<PaginatedList<User>> GetUsersAsync(int pageIndex, int pageSize)
        {
            var userLists = _userDbContext.users.FromSqlRaw("exec GetUsersList").AsEnumerable()
                .OrderBy(b => b.username)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var count = await _userDbContext.users.CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginatedList<User>(userLists, pageIndex, totalPages);
        }

        public async Task<User> AddUserAsync(User objUserDetails)
        {       
            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@username", objUserDetails.username));
            parameter.Add(new SqlParameter("@mail", objUserDetails.mail));
            parameter.Add(new SqlParameter("@phonenumber", Convert.ToInt32(objUserDetails.phonenumber)));
            parameter.Add(new SqlParameter("@skillsets", objUserDetails.skillsets));
            parameter.Add(new SqlParameter("@hobby", objUserDetails.hobby));
            try
            {
                var result = await Task.Run(() => _userDbContext.Database
               .ExecuteSqlRawAsync(@"exec RegisterNewUser @username, @mail, @phonenumber, @skillsets, @hobby", parameter.ToArray()));
            }
            catch (SqlException ex) {
                _logger.LogError(ex, "LogError: An error occurred while processing the request.");
            }
            return objUserDetails;
        }

        public async Task UpdateUserAsync(User objUserDetails)
        {
            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@username", objUserDetails.username));
            parameter.Add(new SqlParameter("@mail", objUserDetails.mail));
            parameter.Add(new SqlParameter("@phonenumber", Convert.ToInt32(objUserDetails.phonenumber)));
            parameter.Add(new SqlParameter("@skillsets", objUserDetails.skillsets));
            parameter.Add(new SqlParameter("@hobby", objUserDetails.hobby));

            // Execute the stored procedure
            await _userDbContext.Database
               .ExecuteSqlRawAsync(@"exec UpdateUser @username, @mail, @phonenumber, @skillsets, @hobby", parameter.ToArray());
        }

        public async Task DeleteUserAsync(string _username)
        {
            // Call stored procedure to delete user
            var usernameParam = new SqlParameter("@username", _username);

            await _userDbContext.Database.ExecuteSqlRawAsync("EXEC DeleteUser @username", usernameParam);
        }

        public Task<IEnumerable<User>> WildcardSearchAsync(string _username, string _mail)
        {
            // Replace '?' with '_' (SQL single-character wildcard) if using SQL Server
            _username = _username.Replace('?', '_');
            _mail = _mail.Replace('?', '_');
            // Use LIKE for wildcard search with % as the wildcard
            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@username", _username));
            parameter.Add(new SqlParameter("@mail", _mail));
            var userLists = _userDbContext.users
            .FromSqlRaw(@"exec WildcardSearch @username, @mail", parameter.ToArray()).AsEnumerable(); ;
            return Task.FromResult(userLists);
        }

        public async Task ArchiveUserAsync(string _username)
        {
            // Call stored procedure to delete user
            var usernameParam = new SqlParameter("@username", _username);

            await _userDbContext.Database.ExecuteSqlRawAsync("EXEC ArchiveUser @username", usernameParam);
        }

        public async Task UnarchiveUserAsync(string _username)
        {
            var usernameParam = new SqlParameter("@username", _username);
            try
            {
                // Execute the stored procedure
                await _userDbContext.Database.ExecuteSqlRawAsync("EXEC UnarchiveUser @username", usernameParam);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "LogError: An error occurred while processing the request.");
            }
        }
    }
}
