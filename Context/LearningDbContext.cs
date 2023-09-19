using Microsoft.EntityFrameworkCore;


namespace project_k;

public class LearningDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public LearningDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("learning"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeDataModel>().Property(e => e.Id).ValueGeneratedOnAdd();
    }
    public DbSet<EmployeeDataModel> Employee { get; set; }
}