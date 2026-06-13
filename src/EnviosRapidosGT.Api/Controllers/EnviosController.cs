using EnviosRapidosGT.Api.Data;
using EnviosRapidosGT.Api.DTOs;
using EnviosRapidosGT.Api.Models;
using EnviosRapidosGT.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnviosRapidosGT.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnviosController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ITarifaService _tarifaService;
    private readonly IEstadoEnvioService _estadoService;
    private readonly ICodigoRastreoService _codigoService;

    public EnviosController(
        AppDbContext db,
        ITarifaService tarifaService,
        IEstadoEnvioService estadoService,
        ICodigoRastreoService codigoService)
    {
        _db = db;
        _tarifaService = tarifaService;
        _estadoService = estadoService;
        _codigoService = codigoService;
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearEnvioRequest request)
    {
        var remitente = await _db.Clientes.FindAsync(request.RemitenteId);
        var destinatario = await _db.Clientes.FindAsync(request.DestinatarioId);

        if (remitente is null)
        {
            return BadRequest(new { mensaje = "El remitente no existe." });
        }

        if (destinatario is null)
        {
            return BadRequest(new { mensaje = "El destinatario no existe." });
        }

        var fecha = DateTime.UtcNow;
        var inicioDia = fecha.Date;
        var finDia = inicioDia.AddDays(1);
        var correlativo = await _db.Envios.CountAsync(e => e.FechaRegistro >= inicioDia && e.FechaRegistro < finDia) + 1;
        var codigo = _codigoService.GenerarCodigo(fecha, correlativo);

        while (await _db.Envios.AnyAsync(e => e.CodigoRastreo == codigo))
        {
            correlativo++;
            codigo = _codigoService.GenerarCodigo(fecha, correlativo);
        }

        var aplicaDescuento = _tarifaService.NitValido(remitente.Nit) || _tarifaService.NitValido(destinatario.Nit);
        var tarifaBase = _tarifaService.CalcularTarifaBase(request.PesoKg);
        var tarifaFinal = _tarifaService.CalcularTarifaFinal(request.PesoKg, aplicaDescuento);

        var envio = new Envio
        {
            CodigoRastreo = codigo,
            RemitenteId = remitente.Id,
            Remitente = remitente,
            DestinatarioId = destinatario.Id,
            Destinatario = destinatario,
            PesoKg = request.PesoKg,
            DireccionOrigen = request.DireccionOrigen.Trim(),
            DireccionDestino = request.DireccionDestino.Trim(),
            DepartamentoDestino = request.DepartamentoDestino.Trim(),
            TarifaBase = tarifaBase,
            Descuento = tarifaBase - tarifaFinal,
            TarifaFinal = tarifaFinal,
            Estado = EstadoEnvio.Registrado,
            IntentosFallidos = 0,
            FechaRegistro = fecha
        };

        envio.HistorialEstados.Add(new HistorialEstado
        {
            Estado = EstadoEnvio.Registrado,
            Ubicacion = request.UbicacionRegistro.Trim(),
            FechaHora = fecha,
            Notas = string.IsNullOrWhiteSpace(request.Notas)
                ? "Envío registrado correctamente."
                : request.Notas.Trim()
        });

        _db.Envios.Add(envio);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(ObtenerPorId), new { id = envio.Id }, MapEnvio(envio));
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var envios = await _db.Envios
            .Include(e => e.Remitente)
            .Include(e => e.Destinatario)
            .OrderByDescending(e => e.Id)
            .ToListAsync();

        return Ok(envios.Select(e => MapEnvio(e)).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var envio = await _db.Envios
            .Include(e => e.Remitente)
            .Include(e => e.Destinatario)
            .Include(e => e.HistorialEstados.OrderByDescending(h => h.FechaHora))
            .FirstOrDefaultAsync(e => e.Id == id);

        if (envio is null)
        {
            return NotFound(new { mensaje = "Envío no encontrado." });
        }

        return Ok(MapEnvio(envio, incluirHistorial: true));
    }

    [HttpGet("rastreo/{codigo}")]
    public async Task<IActionResult> Rastrear(string codigo)
    {
        var normalizado = codigo.Trim().ToUpperInvariant();

        var envio = await _db.Envios
            .Include(e => e.Remitente)
            .Include(e => e.Destinatario)
            .Include(e => e.HistorialEstados.OrderByDescending(h => h.FechaHora))
            .FirstOrDefaultAsync(e => e.CodigoRastreo == normalizado);

        if (envio is null)
        {
            return NotFound(new { mensaje = "No se encontró ningún envío con ese código de rastreo." });
        }

        return Ok(MapEnvio(envio, incluirHistorial: true));
    }

    [HttpPut("{id:int}/estado")]
    public async Task<IActionResult> ActualizarEstado(int id, [FromBody] ActualizarEstadoRequest request)
    {
        var envio = await _db.Envios
            .Include(e => e.Remitente)
            .Include(e => e.Destinatario)
            .Include(e => e.HistorialEstados)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (envio is null)
        {
            return NotFound(new { mensaje = "Envío no encontrado." });
        }

        if (!_estadoService.EsTransicionValida(envio.Estado, request.NuevoEstado))
        {
            return BadRequest(new
            {
                mensaje = $"No se puede cambiar de {envio.Estado} a {request.NuevoEstado}.",
                rutaValida = _estadoService.ObtenerRutaValida()
            });
        }

        envio.Estado = request.NuevoEstado;

        envio.HistorialEstados.Add(new HistorialEstado
        {
            Estado = request.NuevoEstado,
            Ubicacion = request.Ubicacion.Trim(),
            FechaHora = DateTime.UtcNow,
            Notas = string.IsNullOrWhiteSpace(request.Notas)
                ? $"Cambio de estado a {request.NuevoEstado}."
                : request.Notas.Trim()
        });

        await _db.SaveChangesAsync();
        return Ok(MapEnvio(envio, incluirHistorial: true));
    }

    [HttpPost("{id:int}/intento-fallido")]
    public async Task<IActionResult> RegistrarIntentoFallido(int id, [FromBody] IntentoFallidoRequest request)
    {
        var envio = await _db.Envios
            .Include(e => e.Remitente)
            .Include(e => e.Destinatario)
            .Include(e => e.HistorialEstados)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (envio is null)
        {
            return NotFound(new { mensaje = "Envío no encontrado." });
        }

        if (envio.Estado == EstadoEnvio.Entregado || envio.Estado == EstadoEnvio.Devuelto)
        {
            return BadRequest(new { mensaje = "No se pueden registrar intentos fallidos en un envío finalizado." });
        }

        if (envio.Estado != EstadoEnvio.EnReparto)
        {
            return BadRequest(new { mensaje = "Solo se pueden registrar intentos fallidos cuando el envío está EnReparto." });
        }

        if (envio.IntentosFallidos >= 3)
        {
            return BadRequest(new { mensaje = "El envío ya alcanzó el máximo de 3 intentos fallidos." });
        }

        envio.IntentosFallidos++;
        var fecha = DateTime.UtcNow;
        var notas = string.IsNullOrWhiteSpace(request.Notas)
            ? $"Intento fallido #{envio.IntentosFallidos}."
            : $"Intento fallido #{envio.IntentosFallidos}. {request.Notas.Trim()}";

        if (envio.IntentosFallidos == 3)
        {
            envio.Estado = EstadoEnvio.EnDevolucion;
            notas += " Se alcanzó el tercer intento; el envío cambia automáticamente a EnDevolucion.";
        }

        envio.HistorialEstados.Add(new HistorialEstado
        {
            Estado = envio.Estado,
            Ubicacion = request.Ubicacion.Trim(),
            FechaHora = fecha,
            Notas = notas
        });

        await _db.SaveChangesAsync();
        return Ok(MapEnvio(envio, incluirHistorial: true));
    }

    [HttpGet("{id:int}/historial")]
    public async Task<IActionResult> ObtenerHistorial(int id)
    {
        var existe = await _db.Envios.AnyAsync(e => e.Id == id);

        if (!existe)
        {
            return NotFound(new { mensaje = "Envío no encontrado." });
        }

        var historial = await _db.HistorialEstados
            .Where(h => h.EnvioId == id)
            .OrderByDescending(h => h.FechaHora)
            .Select(h => new
            {
                h.Id,
                h.EnvioId,
                estado = h.Estado.ToString(),
                h.Ubicacion,
                h.FechaHora,
                h.Notas
            })
            .ToListAsync();

        return Ok(historial);
    }

    private static object MapEnvio(Envio envio, bool incluirHistorial = false)
    {
        return new
        {
            envio.Id,
            envio.CodigoRastreo,
            estado = envio.Estado.ToString(),
            envio.PesoKg,
            envio.DireccionOrigen,
            envio.DireccionDestino,
            envio.DepartamentoDestino,
            envio.TarifaBase,
            envio.Descuento,
            envio.TarifaFinal,
            envio.IntentosFallidos,
            envio.FechaRegistro,
            remitente = envio.Remitente is null ? null : new
            {
                envio.Remitente.Id,
                envio.Remitente.Nombre,
                envio.Remitente.Telefono,
                envio.Remitente.Nit
            },
            destinatario = envio.Destinatario is null ? null : new
            {
                envio.Destinatario.Id,
                envio.Destinatario.Nombre,
                envio.Destinatario.Telefono,
                envio.Destinatario.Nit
            },
            historial = incluirHistorial
                ? envio.HistorialEstados
                    .OrderByDescending(h => h.FechaHora)
                    .Select(h => new
                    {
                        h.Id,
                        estado = h.Estado.ToString(),
                        h.Ubicacion,
                        h.FechaHora,
                        h.Notas
                    })
                : null
        };
    }
}
