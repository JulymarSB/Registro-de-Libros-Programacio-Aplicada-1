using Microsoft.AspNetCore.Mvc;
using RegistroLibros.Models;
using RegistroLibros.Services;

namespace RegistroLibros.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EstudiantesController(EstudiantesService estudiantesService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await estudiantesService.GetList(e => true);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result.Value);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await estudiantesService.Buscar(id);
        if(!result.IsSuccess || result.Value == null)
            return NotFound();
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Estudiantes estudiante)
    {
        var result = await estudiantesService.Guardar(estudiante);
        if(!result.IsSuccess)
            return BadRequest(result);
        return CreatedAtAction(nameof(Get), new {id = estudiante.EstudianteId}, estudiante);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await estudiantesService.Eliminar(id);
        if (!result.IsSuccess) 
            return BadRequest();
        return Ok();
    }

}