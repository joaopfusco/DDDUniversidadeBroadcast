using DDDUniversidadeBroadcast.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace DDDUniversidadeBroadcast.Infra.Data
{
    public partial class AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : DbContext(options)
    {
        private readonly string _connectionString = GetConnectionString(configuration);

        private static string GetConnectionString(IConfiguration configuration)
        {
            var envConnection = Environment.GetEnvironmentVariable("DEFAULT_CONNECTION");
            var appsettingsConnection = configuration.GetConnectionString("DefaultConnection");

            if (!string.IsNullOrWhiteSpace(envConnection))
                return envConnection;

            if (!string.IsNullOrWhiteSpace(appsettingsConnection))
                return appsettingsConnection;

            throw new Exception("Não há ConnectionString.");
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Seguidor> Seguidores { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Postagem> Postagens { get; set; }
        public DbSet<Participante> Participantes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(e => e.Nome).IsRequired();
                entity.HasIndex(e => e.Nome).IsUnique();

                entity.Property(e => e.Email).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();

                entity.Property(e => e.Telefone).IsRequired();
                entity.HasIndex(e => e.Telefone).IsUnique();

                entity.Property(e => e.Curso).IsRequired();
            });

            modelBuilder.Entity<Seguidor>(entity =>
            {
                entity.HasOne(e => e.Seguido)
                    .WithMany(e => e.Seguidores)
                    .HasForeignKey(e => e.SeguidoId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.Segue)
                    .WithMany(e => e.Seguindo)
                    .HasForeignKey(e => e.SegueId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasIndex(e => new { e.SegueId, e.SeguidoId }).IsUnique();
            });

            modelBuilder.Entity<Evento>(entity =>
            {
                entity.Property(e => e.Nome).IsRequired();
                entity.HasIndex(e => e.Nome).IsUnique();

                entity.Property(e => e.Local).IsRequired();
            });

            modelBuilder.Entity<Postagem>(entity =>
            {
                entity.Property(e => e.Conteudo).IsRequired();

                entity.HasOne(e => e.Autor)
                    .WithMany(e => e.Postagens)
                    .HasForeignKey(e => e.AutorId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.Evento)
                    .WithMany(e => e.Postagens)
                    .HasForeignKey(e => e.EventoId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Participante>(entity =>
            {
                entity.HasOne(e => e.Evento)
                    .WithMany(e => e.Participantes)
                    .HasForeignKey(e => e.EventoId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.Usuario)
                    .WithMany()
                    .HasForeignKey(e => e.UsuarioId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasIndex(e => new { e.EventoId, e.UsuarioId }).IsUnique();
            });

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
