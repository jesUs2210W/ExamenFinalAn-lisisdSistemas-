using EnviosRapidosGT.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace EnviosRapidosGT.Api.DTOs;

public class ActualizarEstadoRequest
{
    [Required]
    public EstadoEnvio NuevoEstado { get; set; }

    [Required]
    [MaxLength(120)]
    public string Ubicacion { get; set; } = string.Empty;

    [MaxLength(350)]
    public string? Notas { get; set; }
}
