using EnviosRapidosGT.Api.Models;

namespace EnviosRapidosGT.Api.Services;

public interface IEstadoEnvioService
{
    bool EsTransicionValida(EstadoEnvio estadoActual, EstadoEnvio nuevoEstado);
    string ObtenerRutaValida();
}
