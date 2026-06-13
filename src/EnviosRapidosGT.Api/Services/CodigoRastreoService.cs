namespace EnviosRapidosGT.Api.Services;

public class CodigoRastreoService : ICodigoRastreoService
{
    public string GenerarCodigo(DateTime fecha, int correlativo)
    {
        if (correlativo <= 0)
        {
            throw new ArgumentException("El correlativo debe ser mayor a cero.", nameof(correlativo));
        }

        return $"ENV-{fecha:yyyyMMdd}-{correlativo:0000}";
    }
}
