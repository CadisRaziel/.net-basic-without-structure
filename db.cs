using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<Product> Products { get; set; }    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Adicionando restrições a cada tipo de atribudo da tabela
        modelBuilder.Entity<Product>().HasKey(p => p.Id);
        modelBuilder.Entity<Product>().Property(p => p.Code).IsRequired(false);
        modelBuilder.Entity<Product>().Property(p => p.Name).HasMaxLength(50).IsRequired(false);
        modelBuilder.Entity<Product>().Property(p => p.Description).HasMaxLength(150).IsRequired(false);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=DESKTOP-QSN1B4G\\SQLEXPRESS;Database=Products;User=sa;Password=rhythms;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=YES;");
    }
}