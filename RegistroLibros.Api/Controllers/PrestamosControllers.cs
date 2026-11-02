using Microsoft.AspNetCore.Mvc;
using RegistroLibros.Models;
using RegistroLibros.Services;

namespace RegistroLibros.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrestamosController(PrestamoService prestamosService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await prestamosService.GetList(p => true);
        if (!result.IsSuccess) 
            return BadRequest(result);
        return Ok(result.Value);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await prestamosService.Buscar(id);
        if (!result.IsSuccess || result.Value == null) 
            return NotFound();
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Prestamos prestamo)
    {
        var result = await prestamosService.Guardar(prestamo);
        if (!result.IsSuccess) 
            return BadRequest(result);
        return CreatedAtAction(nameof(Get), new { id = prestamo.PrestamoId }, prestamo);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await prestamosService.Eliminar(id);
        if (!result.IsSuccess) 
            return BadRequest();
        return Ok();
    }
}