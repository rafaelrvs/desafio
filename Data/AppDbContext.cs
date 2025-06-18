using desafiocs.Clientes;
using Microsoft.EntityFrameworkCore;

    public class AppDbContext : DbContext
    {      
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Contato> Contato { get; set; }
        public DbSet<Endereco> Endereco { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Banco.sqlite");
        base.OnConfiguring(optionsBuilder);
            
        }
    }

