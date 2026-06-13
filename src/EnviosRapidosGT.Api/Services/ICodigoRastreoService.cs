namespace EnviosRapidosGT.Api.Services;

public interface ICodigoRastreoService
{
    string GenerarCodigo(DateTime fecha, int correlativo);
}
