using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RegistroLibros.Context;
using RegistroLibros.Models;
using System.Linq.Expressions;
using System.Runtime.InteropServices.Marshalling;

namespace RegistroLibros.Services;

public class EstudiantesService(IDbContextFactory<Contexto> contextFactory) : Aplicada1.Core.IService<RegistroLibros.Models.Estudiantes, int >
{
    public async Task<bool> ExisteNombre(string nombres)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
            
            return await contexto.Estudiantes.AnyAsync(e => e.Nombres.ToLower() == nombres.ToLower());
    }
    private async Task<bool> Existe(string nombres, int estudianteId = 0)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
            
            return await contexto.Estudiantes.AnyAsync(e => e.EstudianteId != estudianteId);
    }

    private async Task<bool> Insertar(Estudiantes estudiante)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Estudiantes.Add(estudiante);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Estudiantes estudiante)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Estudiantes.Update(estudiante);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Guardar(Estudiantes estudiante)
    {
        if (!await Existe(estudiante.Nombres, estudiante.EstudianteId))
            return false;

        if (estudiante.EstudianteId == 0)
            return await Insertar(estudiante);
        else
            return await Modificar(estudiante);
    }

    public async Task<bool> Eliminar(int estudianteId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var estudiante = await contexto.Estudiantes.Where(e => e.EstudianteId == estudianteId).ExecuteDeleteAsync();
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Estudiantes?> Buscar(int id)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Estudiantes
            .FirstOrDefaultAsync(e => e.EstudianteId == id);
    }

    public async Task<List<Estudiantes>> GetList(Expression<Func<Estudiantes, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Estudiantes.Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
}
