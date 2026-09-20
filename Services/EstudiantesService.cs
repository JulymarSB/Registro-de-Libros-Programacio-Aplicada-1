using Microsoft.EntityFrameworkCore;
using RegistroLibros.DAL;
using RegistroLibros.Models;
using System.Linq.Expressions;

namespace RegistroLibros.Services;

public class EstudiantesService
{
    private readonly Contexto _contexto;

    public EstudiantesService(Contexto contexto)
    {
        _contexto = contexto;
    }

    public async Task<bool> ExisteNombre(string nombres, int estudianteId = 0)
    {
        return await _contexto.Estudiantes
            .AnyAsync(e => e.Nombres.ToLower() == nombres.ToLower() && e.EstudianteId != estudianteId);
    }

    public async Task<bool> Insertar(Estudiantes estudiante)
    {
        _contexto.Estudiantes.Add(estudiante);
        return await _contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(Estudiantes estudiante)
    {
        _contexto.Estudiantes.Update(estudiante);
        return await _contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Guardar(Estudiantes estudiante)
    {
        if (await ExisteNombre(estudiante.Nombres, estudiante.EstudianteId))
            return false;

        if (estudiante.EstudianteId == 0)
            return await Insertar(estudiante);
        else
            return await Modificar(estudiante);
    }

    public async Task<bool> Eliminar(int id)
    {
        var estudiante = await _contexto.Estudiantes.FindAsync(id);
        if (estudiante == null) return false;

        _contexto.Estudiantes.Remove(estudiante);
        return await _contexto.SaveChangesAsync() > 0;
    }

    public async Task<Estudiantes?> Buscar(int id)
    {
        return await _contexto.Estudiantes
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.EstudianteId == id);
    }

    public async Task<List<Estudiantes>> Listar(Expression<Func<Estudiantes, bool>> criterio)
    {
        return await _contexto.Estudiantes
            .AsNoTracking()
            .Where(criterio)
            .ToListAsync();
    }
}