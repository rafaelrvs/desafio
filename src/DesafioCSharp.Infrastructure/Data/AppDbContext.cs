using desafiocs.Domain.Cliente;
using desafiocs.Migrations;
using Desafios.Domain.Cliente;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Cliente> Cliente { get; set; } = default!;
    public DbSet<ClienteEndereco> Endereco { get; set; } = default!;
    public DbSet<ClienteContato> Contato { get; set; } = default!;
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Banco.sqlite");
        base.OnConfiguring(optionsBuilder);


    }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
     
        modelBuilder.Entity<Cliente>()
            .HasKey(c => c.Id);
        modelBuilder.Entity<Cliente>()
            .Property(c => c.Id)
            .ValueGeneratedOnAdd();

        
        modelBuilder.Entity<ClienteEndereco>()
            .HasKey(e => e.Id);
        modelBuilder.Entity<ClienteEndereco>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<ClienteContato>()
            .HasKey(cc => cc.Id);
        modelBuilder.Entity<ClienteContato>()
            .Property(cc => cc.Id)
            .ValueGeneratedOnAdd();
            

    
        modelBuilder.Entity<Cliente>()
            .HasOne(c => c.Endereco)
            .WithOne(e => e.Cliente!)
            .HasForeignKey<ClienteEndereco>(e => e.ClienteId)
            .OnDelete(DeleteBehavior.Cascade);

            
         modelBuilder.Entity<Cliente>()
        .HasOne(c => c.Contato)
        .WithOne(cc => cc.Cliente!)
        .HasForeignKey<ClienteContato>(cc => cc.ClienteId)   
        .OnDelete(DeleteBehavior.Cascade);

    
    }
    }

