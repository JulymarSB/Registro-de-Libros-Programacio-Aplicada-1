using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RegistroLibros.DAL;
using RegistroLibros.Models;

namespace RegistroLibros.Services;

public class LibroService(IDbContextFactory<Contexto> DbFactory)
{
    public async Task<bool> ExisteLibro(String titulo)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Libros.AnyAsync(l =>l.Titulo.ToLower() == titulo.ToLower());
        
    }

    public async Task<bool> Insertar(Libros libro)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Libros.Add(libro);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(Libros libro)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Libros.Update(libro);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Guardar(Libros libro)
    {
        if(libro.LibroId == 0)
        {
            return await Insertar(libro);
        }
        else
        {
            return await Modificar(libro);
        }
    }

    public async Task<Libros?> Buscar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Libros.FirstOrDefaultAsync(l => l.LibroId ==  id);
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Libros.Where(l => l.LibroId == id).ExecuteDeleteAsync() > 0;
    }

    public async Task<List<Libros>> Listar(Expression<Func<Libros, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Libros.Where(criterio).AsNoTracking().ToListAsync();
    }
}