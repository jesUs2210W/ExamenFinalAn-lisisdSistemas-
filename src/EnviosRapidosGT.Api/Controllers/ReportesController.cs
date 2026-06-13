using EnviosRapidosGT.Api.Data;
using EnviosRapidosGT.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnviosRapidosGT.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportesController : ControllerBase
{
    private readonly AppDbContext _db;

    public ReportesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("eficiencia")]
    public async Task<IActionResult> Eficiencia()
    {
        var envios = await _db.Envios
            .Include(e => e.HistorialEstados)
            .ToListAsync();

        var total = envios.Count;
        var entregados = envios.Count(e => e.Estado == EstadoEnvio.Entregado);
        var devueltos = envios.Count(e => e.Estado == EstadoEnvio.Devuelto || e.Estado == EstadoEnvio.EnDevolucion);
        var enProceso = envios.Count(e => e.Estado is EstadoEnvio.Registrado or EstadoEnvio.EnTransito or EstadoEnvio.EnReparto);
        var totalIntentosFallidos = envios.Sum(e => e.IntentosFallidos);

        var eficiencia = total == 0 ? 0 : Math.Round((decimal)entregados / total * 100m, 2);
        var tasaDevolucion = total == 0 ? 0 : Math.Round((decimal)devueltos / total * 100m, 2);

        var entregasConTiempo = envios
            .Select(e => new
            {
                Envio = e,
                Entregado = e.HistorialEstados
                    .Where(h => h.Estado == EstadoEnvio.Entregado)
                    .OrderBy(h => h.FechaHora)
                    .FirstOrDefault()
            })
            .Where(x => x.Entregado is not null)
            .ToList();

        var promedioHorasEntrega = entregasConTiempo.Count == 0
            ? 0
            : Math.Round(entregasConTiempo.Average(x => (x.Entregado!.FechaHora - x.Envio.FechaRegistro).TotalHours), 2);

        var porEstado = envios
            .GroupBy(e => e.Estado.ToString())
            .ToDictionary(g => g.Key, g => g.Count());

        return Ok(new
        {
            totalEnvios = total,
            entregados,
            devueltosOEnDevolucion = devueltos,
            enProceso,
            totalIntentosFallidos,
            porcentajeEficienciaEntrega = eficiencia,
            porcentajeDevolucion = tasaDevolucion,
            promedioHorasEntrega,
            totalFacturado = envios.Sum(e => e.TarifaFinal),
            totalDescuentosAplicados = envios.Sum(e => e.Descuento),
            enviosPorEstado = porEstado
        });
    }
}
