using EnviosRapidosGT.Api.Data;
using EnviosRapidosGT.Api.DTOs;
using EnviosRapidosGT.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnviosRapidosGT.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly AppDbContext _db;

    public ClientesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearClienteRequest request)
    {
        var cliente = new Cliente
        {
            Nombre = request.Nombre.Trim(),
            Telefono = request.Telefono.Trim(),
            Email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim(),
            Direccion = request.Direccion.Trim(),
            Nit = string.IsNullOrWhiteSpace(request.Nit) ? null : request.Nit.Trim(),
            FechaRegistro = DateTime.UtcNow
        };

        _db.Clientes.Add(cliente);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(ObtenerPorId), new { id = cliente.Id }, cliente);
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var clientes = await _db.Clientes
            .OrderByDescending(c => c.Id)
            .ToListAsync();

        return Ok(clientes);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var cliente = await _db.Clientes.FindAsync(id);

        if (cliente is null)
        {
            return NotFound(new { mensaje = "Cliente no encontrado." });
        }

        return Ok(cliente);
    }
}
