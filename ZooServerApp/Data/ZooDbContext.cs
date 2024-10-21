namespace Data.ZooDbContext;
using Entity.Animal;
using Microsoft.EntityFrameworkCore;

public class ZooDbContext : DbContext
{
    private const string connectionString = "Server=localhost;Database=ZooDb;User Id=SA;Password=Secret12345;TrustServerCertificate=True;";
    public DbSet<Animal> Animals { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(connectionString);
        base.OnConfiguring(optionsBuilder);
    }
}



