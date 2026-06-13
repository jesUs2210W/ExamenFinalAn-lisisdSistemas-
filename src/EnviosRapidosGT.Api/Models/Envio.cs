using System.ComponentModel.DataAnnotations;

namespace EnviosRapidosGT.Api.Models;

public class Envio
{
    public int Id { get; set; }

    [Required]
    [MaxLength(25)]
    public string CodigoRastreo { get; set; } = string.Empty;

    public int RemitenteId { get; set; }
    public Cliente? Remitente { get; set; }

    public int DestinatarioId { get; set; }
    public Cliente? Destinatario { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal PesoKg { get; set; }

    [Required]
    [MaxLength(250)]
    public string DireccionOrigen { get; set; } = string.Empty;

    [Required]
    [MaxLength(250)]
    public string DireccionDestino { get; set; } = string.Empty;

    [Required]
    [MaxLength(80)]
    public string DepartamentoDestino { get; set; } = string.Empty;

    public decimal TarifaBase { get; set; }
    public decimal Descuento { get; set; }
    public decimal TarifaFinal { get; set; }

    public EstadoEnvio Estado { get; set; } = EstadoEnvio.Registrado;
    public int IntentosFallidos { get; set; } = 0;
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    public ICollection<HistorialEstado> HistorialEstados { get; set; } = new List<HistorialEstado>();
}
