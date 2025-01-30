using Microsoft.EntityFrameworkCore;

namespace Etiqa_Assessment_REST_API.Models
{
    public class UserDbContext:DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options):base(options)
        {
            
        }
        public DbSet<User> users { get; set; } = null!;
    }
}
