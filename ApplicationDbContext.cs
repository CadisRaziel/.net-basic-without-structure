using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }    
    public DbSet<Category> Category { get; set; }    
    //public DbSet<Category> Categories { get; set; } // -> para renomear a tabela no banco de 'Category' para 'Categories' eu faço o comentario la em baixo no modelBuilder
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Adicionando restrições a cada tipo de atribudo da tabela
        modelBuilder.Entity<Product>().HasKey(p => p.Id);
        modelBuilder.Entity<Product>().Property(p => p.Code).IsRequired(false);
        modelBuilder.Entity<Product>().Property(p => p.Name).HasMaxLength(50).IsRequired(false);
        modelBuilder.Entity<Product>().Property(p => p.Description).HasMaxLength(150).IsRequired(false);
        //modelBuilder.Entity<Category>().ToTable("Categories") //-> Caso eu queira que ao inves de criar no banco como 'Category' eu criar como 'Categories'
    }  
}