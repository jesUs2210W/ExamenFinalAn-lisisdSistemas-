using EnviosRapidosGT.Api.Models;

namespace EnviosRapidosGT.Api.Services;

public class EstadoEnvioService : IEstadoEnvioService
{
    private static readonly Dictionary<EstadoEnvio, EstadoEnvio[]> Transiciones = new()
    {
        [EstadoEnvio.Registrado] = [EstadoEnvio.EnTransito],
        [EstadoEnvio.EnTransito] = [EstadoEnvio.EnReparto],
        [EstadoEnvio.EnReparto] = [EstadoEnvio.Entregado, EstadoEnvio.EnDevolucion],
        [EstadoEnvio.EnDevolucion] = [EstadoEnvio.Devuelto],
        [EstadoEnvio.Entregado] = [],
        [EstadoEnvio.Devuelto] = []
    };

    public bool EsTransicionValida(EstadoEnvio estadoActual, EstadoEnvio nuevoEstado)
    {
        return Transiciones.TryGetValue(estadoActual, out var permitidos)
            && permitidos.Contains(nuevoEstado);
    }

    public string ObtenerRutaValida()
    {
        return "Registrado -> EnTransito -> EnReparto -> Entregado | EnReparto -> EnDevolucion -> Devuelto";
    }
}
