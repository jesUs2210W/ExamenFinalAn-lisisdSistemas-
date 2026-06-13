using EnviosRapidosGT.Api.Services;

namespace EnviosRapidosGT.Tests;

public class TarifaServiceTests
{
    private readonly TarifaService _service = new();

    [Theory]
    [InlineData(0.5, 25)]
    [InlineData(1.0, 25)]
    [InlineData(1.01, 45)]
    [InlineData(5.0, 45)]
    [InlineData(5.01, 75)]
    [InlineData(10.0, 75)]
    [InlineData(10.01, 100)]
    public void CalcularTarifaBase_DeberiaRespetarRangos(double peso, double esperado)
    {
        var resultado = _service.CalcularTarifaBase((decimal)peso);
        Assert.Equal((decimal)esperado, resultado);
    }

    [Fact]
    public void CalcularTarifaFinal_ConNitValido_AplicaCincoPorCientoDescuento()
    {
        var resultado = _service.CalcularTarifaFinal(2.5m, aplicarDescuento: true);
        Assert.Equal(42.75m, resultado);
    }

    [Theory]
    [InlineData("1234567-8", true)]
    [InlineData("1234567-K", true)]
    [InlineData("CF", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    [InlineData("ABC-123", false)]
    public void NitValido_DeberiaValidarFormato(string? nit, bool esperado)
    {
        var resultado = _service.NitValido(nit);
        Assert.Equal(esperado, resultado);
    }
}
