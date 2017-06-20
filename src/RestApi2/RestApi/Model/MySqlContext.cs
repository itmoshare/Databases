using Microsoft.EntityFrameworkCore;
using MySql.Data.Entity;
using MySql.Data.Types;

namespace RestApi.Model
{
    public class MySqlContext : DbContext
    {
        public MySqlContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Staff> staff { get; set; }
    }
}
