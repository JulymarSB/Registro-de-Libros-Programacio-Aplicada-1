using System.Linq.Expressions;
using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RegistroLibros.Context;
using RegistroLibros.Models;

namespace RegistroLibros.Services;

public class LibroService(IDbContextFactory<Contexto> contextFactory)
    :Aplicada1.Core.IService<Libros, int>
{
     public async Task<bool> Guardar(Libros libro)
    {
        if (!await Existe(libro.LibroId))
        {
            return await Insertar(libro);
        }
        else
        {
            return await Modificar(libro);
        }
    }
    private async Task<bool> Existe(int libroid)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Libros.AnyAsync(l => l.LibroId == libroid);
    }
    private async Task<bool> ExisteTituloLibro(String titulo)
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

    public async Task<Libros?> Buscar(int libroId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Libros.FirstOrDefaultAsync(l => l.LibroId ==  libroId);
    }

    public async Task<bool> Eliminar(int libroId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Libros
        .Where(l => l.LibroId == libroId)
        .ExecuteDeleteAsync() > 0;
    }

    public async Task<List<Libros>> GetList(Expression<Func<Libros, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Libros
        .Where(criterio)
        .AsNoTracking()
        .ToListAsync();
    }
}
