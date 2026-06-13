namespace EnviosRapidosGT.Api.Services;

public interface ITarifaService
{
    decimal CalcularTarifaBase(decimal pesoKg);
    decimal CalcularTarifaFinal(decimal pesoKg, bool aplicarDescuento);
    bool NitValido(string? nit);
}
