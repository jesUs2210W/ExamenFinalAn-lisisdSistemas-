using System.ComponentModel.DataAnnotations;

namespace EnviosRapidosGT.Api.DTOs;

public class CrearClienteRequest
{
    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [MaxLength(25)]
    public string Telefono { get; set; } = string.Empty;

    [MaxLength(120)]
    public string? Email { get; set; }

    [Required]
    [MaxLength(250)]
    public string Direccion { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Nit { get; set; }
}
