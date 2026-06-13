using EnviosRapidosGT.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EnviosRapidosGT.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Envio> Envios => Set<Envio>();
    public DbSet<HistorialEstado> HistorialEstados => Set<HistorialEstado>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.Property(c => c.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Telefono).IsRequired().HasMaxLength(25);
            entity.Property(c => c.Email).HasMaxLength(120);
            entity.Property(c => c.Direccion).IsRequired().HasMaxLength(250);
            entity.Property(c => c.Nit).HasMaxLength(20);
        });

        modelBuilder.Entity<Envio>(entity =>
        {
            entity.HasIndex(e => e.CodigoRastreo).IsUnique();
            entity.Property(e => e.CodigoRastreo).IsRequired().HasMaxLength(25);
            entity.Property(e => e.DireccionOrigen).IsRequired().HasMaxLength(250);
            entity.Property(e => e.DireccionDestino).IsRequired().HasMaxLength(250);
            entity.Property(e => e.DepartamentoDestino).IsRequired().HasMaxLength(80);
            entity.Property(e => e.Estado).HasConversion<string>().HasMaxLength(30);
            entity.Property(e => e.PesoKg).HasPrecision(10, 2);
            entity.Property(e => e.TarifaBase).HasPrecision(10, 2);
            entity.Property(e => e.Descuento).HasPrecision(10, 2);
            entity.Property(e => e.TarifaFinal).HasPrecision(10, 2);

            entity.HasOne(e => e.Remitente)
                .WithMany()
                .HasForeignKey(e => e.RemitenteId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Destinatario)
                .WithMany()
                .HasForeignKey(e => e.DestinatarioId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<HistorialEstado>(entity =>
        {
            entity.Property(h => h.Estado).HasConversion<string>().HasMaxLength(30);
            entity.Property(h => h.Ubicacion).IsRequired().HasMaxLength(120);
            entity.Property(h => h.Notas).HasMaxLength(350);

            entity.HasOne(h => h.Envio)
                .WithMany(e => e.HistorialEstados)
                .HasForeignKey(h => h.EnvioId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
