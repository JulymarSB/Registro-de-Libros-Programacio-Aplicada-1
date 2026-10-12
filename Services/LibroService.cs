using System.Linq.Expressions;
using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RegistroLibros.Context;
using RegistroLibros.Models;

namespace RegistroLibros.Services;

public class LibroService(IDbContextFactory<Contexto> contextFactory)
    :IServiceResult<Libros, int>
{
     public async Task<Result> Guardar(Libros libro)
    {
        if (!await ExisteTituloLibro(libro.Titulo))
        {
            return Result.Success(false);
        }

        if (!await Existe(libro.LibroId))
        {
            var inserto = await Insertar(libro);
            return Result.Success(inserto);
        }
        else
        {
            var modifico = await Modificar(libro);
            return Result.Success(modifico);
        }
    }
    public async Task<bool> Existe(int libroid)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Libros.
        AnyAsync(l => l.LibroId == libroid);
    }
    public async Task<bool> ExisteTituloLibro(String titulo)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Libros.AnyAsync(l =>l.Titulo.ToLower() == titulo.ToLower());
        
    }

    private async Task<bool> Insertar(Libros libro)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Libros.Update(libro);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Libros libro)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Libros.Update(libro);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result<Libros?>> Buscar(int libroId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Libros
        .FirstOrDefaultAsync(l => l.LibroId ==  libroId);
    }

    public async Task<Result> Eliminar(int libroId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var eliminado = await contexto.Libros
        .Where(l => l.LibroId == libroId)
        .ExecuteDeleteAsync() > 0;

        return Result.Success(eliminado);
    }

    public async Task<Result<List<Libros>>> GetList(Expression<Func<Libros, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Libros
        .Where(criterio)
        .AsNoTracking()
        .ToListAsync();
    }
}
