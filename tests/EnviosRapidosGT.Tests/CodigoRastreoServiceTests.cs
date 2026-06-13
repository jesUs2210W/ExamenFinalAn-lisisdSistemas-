using EnviosRapidosGT.Api.Services;

namespace EnviosRapidosGT.Tests;

public class CodigoRastreoServiceTests
{
    [Fact]
    public void GenerarCodigo_DeberiaUsarFormatoSolicitado()
    {
        var service = new CodigoRastreoService();
        var codigo = service.GenerarCodigo(new DateTime(2026, 6, 13), 7);

        Assert.Equal("ENV-20260613-0007", codigo);
    }
}
