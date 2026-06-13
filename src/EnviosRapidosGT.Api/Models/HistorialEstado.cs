using System.ComponentModel.DataAnnotations;

namespace EnviosRapidosGT.Api.Models;

public class HistorialEstado
{
    public int Id { get; set; }

    public int EnvioId { get; set; }
    public Envio? Envio { get; set; }

    public EstadoEnvio Estado { get; set; }

    [Required]
    [MaxLength(120)]
    public string Ubicacion { get; set; } = string.Empty;

    public DateTime FechaHora { get; set; } = DateTime.UtcNow;

    [MaxLength(350)]
    public string? Notas { get; set; }
}
