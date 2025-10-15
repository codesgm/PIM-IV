using Microsoft.EntityFrameworkCore;
using PimApi.Models;

namespace PimApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configuração da entidade Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                
                entity.HasIndex(e => e.Email).IsUnique();
                
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Senha).IsRequired();
                
                entity.Property(e => e.Status).HasConversion<int>();
                entity.Property(e => e.PerfilAcesso).HasConversion<int>();
            });

            // Configuração da entidade Faq
            modelBuilder.Entity<Faq>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                
                entity.Property(e => e.Pergunta).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Resposta).IsRequired();
                entity.Property(e => e.Categoria).HasConversion<int>();
                entity.Property(e => e.Ativo).HasDefaultValue(true);
                entity.Property(e => e.DataCriacao).HasDefaultValueSql("GETUTCDATE()");
            });
        }
    }
}
