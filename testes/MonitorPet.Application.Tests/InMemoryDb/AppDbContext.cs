using Microsoft.EntityFrameworkCore;
using MonitorPet.Application.Tests.ModelDb;

namespace MonitorPet.Application.Tests.InMemoryDb;

public class AppDbContext : DbContext
{
    public DbSet<UserDbModel> Users { get; set; } = null!;
    public DbSet<DosadorDbModel> Dosadores { get; set; } = null!;
    public DbSet<UsuarioDosadorDbModel> UsuariosDosadores { get; set; } = null!;
    public DbSet<AgendamentoDbModel> Agendamentos { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseInMemoryDatabase(databaseName: "Test");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserDbModel>()
            .HasKey(table => new { table.Id });

        modelBuilder.Entity<UserDbModel>()
            .HasIndex(table => new { table.Email }).IsUnique();

        modelBuilder.Entity<DosadorDbModel>()
            .HasKey(table => new { table.IdDosador });

        modelBuilder.Entity<UsuarioDosadorDbModel>()
            .HasKey(table => new { table.Id });

        modelBuilder.Entity<UsuarioDosadorDbModel>()
            .Property(table => table.Id);

        modelBuilder.Entity<UsuarioDosadorDbModel>()
            .HasIndex(table => new { table.IdDosador, table.IdUsuario }).IsUnique();

        modelBuilder.Entity<UsuarioDosadorDbModel>()
            .HasOne(e => e.Dosador);

        modelBuilder.Entity<UsuarioDosadorDbModel>()
            .HasOne(e => e.User);

        modelBuilder.Entity<AgendamentoDbModel>()
            .HasKey(table => new { table.Id });

        modelBuilder.Entity<AgendamentoDbModel>()
            .Property(table => table.Id);

        modelBuilder.Entity<AgendamentoDbModel>()
            .HasOne(e => e.Dosador);
    }
}