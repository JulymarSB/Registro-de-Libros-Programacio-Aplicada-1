using Microsoft.AspNetCore.Mvc;
using RegistroLibros.Models;
using RegistroLibros.Services;

namespace RegistroLibros.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EntradasController(EntradaServices entradasService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await entradasService.GetList(e => true);
        if (!result.IsSuccess) 
            return BadRequest(result);
        return Ok(result.Value);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await entradasService.Buscar(id);
        if (!result.IsSuccess || result.Value == null) 
            return NotFound();
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Entradas entrada)
    {
        var result = await entradasService.Guardar(entrada);
        if (!result.IsSuccess) 
            return BadRequest(result);
        return CreatedAtAction(nameof(Get), new { id = entrada.EntradaId }, entrada);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await entradasService.Eliminar(id);
        if (!result.IsSuccess) 
            return BadRequest();
        return Ok();
    }
}