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
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        
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

            // Configuração da entidade Chat
            modelBuilder.Entity<Chat>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.UserContact).HasMaxLength(100);
                entity.Property(e => e.InitialMessage).IsRequired();
                entity.Property(e => e.Status).HasConversion<int>();
                entity.Property(e => e.Source).HasMaxLength(20);
                
                entity.HasOne(e => e.AssignedTechnician)
                      .WithMany()
                      .HasForeignKey(e => e.AssignedTechnicianId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Configuração da entidade ChatMessage
            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                
                entity.Property(e => e.SenderType).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Message).IsRequired();
                
                entity.HasOne(e => e.Chat)
                      .WithMany(c => c.Messages)
                      .HasForeignKey(e => e.ChatId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasOne(e => e.Sender)
                      .WithMany()
                      .HasForeignKey(e => e.SenderId)
                      .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
