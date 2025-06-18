using Desafios.Domain.Cliente;
using Microsoft.EntityFrameworkCore;

    public class AppDbContext : DbContext
    {      
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<ClienteEndereco> Endereco { get; set; }
      
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Banco.sqlite");
        base.OnConfiguring(optionsBuilder);
    
            
        }
    }

