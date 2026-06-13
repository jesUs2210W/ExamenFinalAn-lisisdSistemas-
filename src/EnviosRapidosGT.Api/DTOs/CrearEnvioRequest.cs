using System.ComponentModel.DataAnnotations;

namespace EnviosRapidosGT.Api.DTOs;

public class CrearEnvioRequest
{
    [Required]
    public int RemitenteId { get; set; }

    [Required]
    public int DestinatarioId { get; set; }

    [Range(typeof(decimal), "0.01", "999999")]
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

    [Required]
    [MaxLength(120)]
    public string UbicacionRegistro { get; set; } = string.Empty;

    [MaxLength(350)]
    public string? Notas { get; set; }
}
