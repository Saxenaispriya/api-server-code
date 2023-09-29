using Microsoft.EntityFrameworkCore;

namespace UserCredentialsApp.Models
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options):base(options)
        {

        }

        public DbSet<UserRegister> userRegisters { get; set; }
    }
}
