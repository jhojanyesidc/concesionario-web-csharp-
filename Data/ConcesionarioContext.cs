using ConcesionarioWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ConcesionarioWeb.Data;

public class ConcesionarioContext : DbContext
{
    public ConcesionarioContext(DbContextOptions<ConcesionarioContext> options) : base(options) { }

    public DbSet<TipoVehiculo> TipoVehiculos => Set<TipoVehiculo>();
    public DbSet<TipoConductor> TipoConductores => Set<TipoConductor>();
    public DbSet<Vehiculo> Vehiculos => Set<Vehiculo>();
    public DbSet<Conductor> Conductores => Set<Conductor>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TipoVehiculo>(entity =>
        {
            entity.ToTable("tipo_vehiculo", "dbo");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(255).IsRequired();
        });

        modelBuilder.Entity<TipoConductor>(entity =>
        {
            entity.ToTable("tipo_conductor", "dbo");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(255).IsRequired();
        });

        modelBuilder.Entity<Vehiculo>(entity =>
        {
            entity.ToTable("vehiculo", "dbo");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Marca).HasColumnName("marca").HasMaxLength(255).IsRequired();
            entity.Property(e => e.Modelo).HasColumnName("modelo").HasMaxLength(255).IsRequired();
            entity.Property(e => e.Matricula).HasColumnName("matricula").HasMaxLength(255).IsRequired();
            entity.Property(e => e.Año).HasColumnName("año").IsRequired();
            entity.Property(e => e.IdTipoVehiculo).HasColumnName("id_tipo_vehiculo").IsRequired();
            
            // Relación con TipoVehiculo
            entity.HasOne(v => v.TipoVehiculo)
                  .WithMany()
                  .HasForeignKey(v => v.IdTipoVehiculo)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Conductor>(entity =>
        {
            entity.ToTable("conductor", "dbo");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(255).IsRequired();
            entity.Property(e => e.TipoLicencia).HasColumnName("tipo_licencia").HasMaxLength(255).IsRequired();
            entity.Property(e => e.IdTipoConductor).HasColumnName("id_tipo_conductor").IsRequired();
            
            // Relación con TipoConductor
            entity.HasOne(c => c.TipoConductor)
                  .WithMany()
                  .HasForeignKey(c => c.IdTipoConductor)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
