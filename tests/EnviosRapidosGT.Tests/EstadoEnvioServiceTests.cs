using EnviosRapidosGT.Api.Models;
using EnviosRapidosGT.Api.Services;

namespace EnviosRapidosGT.Tests;

public class EstadoEnvioServiceTests
{
    private readonly EstadoEnvioService _service = new();

    [Theory]
    [InlineData(EstadoEnvio.Registrado, EstadoEnvio.EnTransito, true)]
    [InlineData(EstadoEnvio.EnTransito, EstadoEnvio.EnReparto, true)]
    [InlineData(EstadoEnvio.EnReparto, EstadoEnvio.Entregado, true)]
    [InlineData(EstadoEnvio.EnReparto, EstadoEnvio.EnDevolucion, true)]
    [InlineData(EstadoEnvio.EnDevolucion, EstadoEnvio.Devuelto, true)]
    [InlineData(EstadoEnvio.Registrado, EstadoEnvio.Entregado, false)]
    [InlineData(EstadoEnvio.Entregado, EstadoEnvio.Devuelto, false)]
    [InlineData(EstadoEnvio.Devuelto, EstadoEnvio.EnTransito, false)]
    public void EsTransicionValida_DeberiaRespetarFlujo(EstadoEnvio actual, EstadoEnvio nuevo, bool esperado)
    {
        var resultado = _service.EsTransicionValida(actual, nuevo);
        Assert.Equal(esperado, resultado);
    }
}
