using System.ComponentModel.DataAnnotations;

namespace EnviosRapidosGT.Api.DTOs;

public class IntentoFallidoRequest
{
    [Required]
    [MaxLength(120)]
    public string Ubicacion { get; set; } = string.Empty;

    [MaxLength(350)]
    public string? Notas { get; set; }
}
