using System.Text.RegularExpressions;

namespace EnviosRapidosGT.Api.Services;

public class TarifaService : ITarifaService
{
    public decimal CalcularTarifaBase(decimal pesoKg)
    {
        if (pesoKg <= 0)
        {
            throw new ArgumentException("El peso debe ser mayor a cero.", nameof(pesoKg));
        }

        if (pesoKg <= 1m)
        {
            return 25m;
        }

        if (pesoKg <= 5m)
        {
            return 45m;
        }

        if (pesoKg <= 10m)
        {
            return 75m;
        }

        return 100m;
    }

    public decimal CalcularTarifaFinal(decimal pesoKg, bool aplicarDescuento)
    {
        var tarifaBase = CalcularTarifaBase(pesoKg);
        var total = aplicarDescuento ? tarifaBase * 0.95m : tarifaBase;
        return Math.Round(total, 2, MidpointRounding.AwayFromZero);
    }

    public bool NitValido(string? nit)
    {
        if (string.IsNullOrWhiteSpace(nit))
        {
            return false;
        }

        var limpio = nit.Trim().Replace(" ", "");

        if (limpio.Equals("CF", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return Regex.IsMatch(limpio, @"^\d{1,8}-?[\dKk]$");
    }
}
