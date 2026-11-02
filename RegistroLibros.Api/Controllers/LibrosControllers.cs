using Microsoft.AspNetCore.Mvc;
using RegistroLibros.Models;
using RegistroLibros.Services;
using SQLitePCL;

namespace RegistroLibros.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LibrosController(LibroService librosService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var result = await librosService.GetList(l => true);
        if (!result.IsSuccess) 
            return BadRequest(result);
        return Ok(result.Value);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> Get(int id)
    {
        var result = await librosService.Buscar(id);
        if (!result.IsSuccess || result.Value == null) 
            return NotFound();
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<Libros>> Post(Libros libro)
    {
        var result = await librosService.Guardar(libro);
        if (!result.IsSuccess) 
            return BadRequest(result);
        return CreatedAtAction(nameof(Get), new { id = libro.LibroId }, libro);
    }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await librosService.Eliminar(id);
        if (!result.IsSuccess) 
            return BadRequest();
        return Ok();
    }
}