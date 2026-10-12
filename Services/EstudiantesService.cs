using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RegistroLibros.Context;
using RegistroLibros.Models;
using System.Linq.Expressions;

namespace RegistroLibros.Services;

public class EstudiantesService(
    IDbContextFactory<Contexto> contextFactory) 
    : IServiceResult<Estudiantes, int>
{
    public async Task<Result> Guardar(Estudiantes estudiante)
    {
        if (await ExisteNombre(estudiante.Nombres, estudiante.EstudianteId))
        {
            return Result.Success(false);
        }

        if (!await Existe(estudiante.EstudianteId))
        {
            var inserto = await Insertar(estudiante);
            return Result.Success(inserto);
        }
        else
        {
            var modifico = await Modificar(estudiante);
            return Result.Success(modifico);
        }
    }

    private async Task<bool> Existe(int estudianteId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Estudiantes
            .AnyAsync(e => e.EstudianteId == estudianteId);
    }

    public async Task<bool> ExisteNombre(string nombres, int estudianteId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Estudiantes
            .AnyAsync(e => e.Nombres.ToLower() == nombres.ToLower() && e.EstudianteId != estudianteId);
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


    public async Task<Result<Estudiantes?>> Buscar(int id)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Estudiantes
            .FirstOrDefaultAsync(e => e.EstudianteId == id);
    }

 public async Task<Result> Eliminar(int estudianteId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var eliminado = await contexto.Estudiantes
            .Where(e => e.EstudianteId == estudianteId)
            .ExecuteDeleteAsync() > 0;

        return Result.Success(eliminado);
    }

     public async Task<Result<List<Estudiantes>>> GetList(Expression<Func<Estudiantes, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Estudiantes
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
}