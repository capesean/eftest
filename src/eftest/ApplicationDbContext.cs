using Microsoft.EntityFrameworkCore;

namespace eftest
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Test> Tests { get; set; }
    }

    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
