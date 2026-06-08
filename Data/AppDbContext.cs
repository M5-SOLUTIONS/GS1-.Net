using Microsoft.EntityFrameworkCore;
using M5Storage.Models;

namespace M5Storage.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Recurso> Recursos { get; set; }
        public DbSet<RecursoEnergia> RecursosEnergia { get; set; }
        public DbSet<RecursoMedicamento> RecursosMedicamento { get; set; }
        public DbSet<Movimentacao> Movimentacoes { get; set; }
        public DbSet<Alerta> Alertas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Id)
                .UseIdentityColumn();

            modelBuilder.Entity<Recurso>()
                .Property(r => r.Id)
                .UseIdentityColumn();

            modelBuilder.Entity<Movimentacao>()
                .Property(m => m.Id)
                .UseIdentityColumn();

            modelBuilder.Entity<Alerta>()
                .Property(a => a.Id)
                .UseIdentityColumn();

            modelBuilder.Entity<RecursoEnergia>()
                .HasOne(e => e.Recurso)
                .WithOne(r => r.RecursoEnergia)
                .HasForeignKey<RecursoEnergia>(e => e.Id);

            modelBuilder.Entity<RecursoMedicamento>()
                .HasOne(m => m.Recurso)
                .WithOne(r => r.RecursoMedicamento)
                .HasForeignKey<RecursoMedicamento>(m => m.Id);

            modelBuilder.Entity<Movimentacao>()
                .HasOne(m => m.Usuario)
                .WithMany(u => u.Movimentacoes)
                .HasForeignKey(m => m.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Movimentacao>()
                .HasOne(m => m.Recurso)
                .WithMany(r => r.Movimentacoes)
                .HasForeignKey(m => m.RecursoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Alerta>()
                .HasOne(a => a.Recurso)
                .WithMany(r => r.Alertas)
                .HasForeignKey(a => a.RecursoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
