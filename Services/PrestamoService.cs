using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using RegistroLibros.Context;
using RegistroLibros.Models;

namespace RegistroLibros.Services;

public class PrestamoService(IDbContextFactory<Contexto> contextFactory) : Aplicada1.Core.IService<Prestamos, int>
{
    public async Task<bool> Guardar(Prestamos prestamo)
    {
        if (!await Existe(prestamo.PrestamoId))
        {
            return await Insertar(prestamo);
        }
        else
        {
            return await Modificar(prestamo);
        }
    }
    private async Task<bool> Existe(int prestamoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Prestamos.AnyAsync(p => p.PrestamoId == prestamoId);
    }
    private async Task<bool> Insertar(Prestamos prestamo)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var estudianteConPrestamoActivo = await contexto.Prestamos.Include(p => p.Libro).AnyAsync(p => p.EstudianteId == prestamo.EstudianteId && p.Libro.Disponible == false);
        if(estudianteConPrestamoActivo)
        {
            return false;
        }

        var libro = await contexto.Libros.FindAsync(prestamo.LibroId);
        if(libro == null ||!libro.Disponible)
        {
            return false;
        }

        libro.Disponible = false;
        contexto.Libros.Update(libro);

        contexto.Prestamos.Add(prestamo);
        return await contexto.SaveChangesAsync() > 0;

    }

    public async Task<bool> Modificar(Prestamos prestamo)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        contexto.Update(prestamo);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Eliminar(int prestamoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var prestamo = await contexto.Prestamos.FindAsync(prestamoId);

        if(prestamo == null)
        {
            return false;
        }

        var libro = await contexto.Libros.FindAsync(prestamo.LibroId);
        if(libro != null)
        {
            libro.Disponible = true;
            await contexto.SaveChangesAsync();
        }

        int eliminados = await contexto.Prestamos
        .Where(p => p.PrestamoId == prestamoId)
        .ExecuteDeleteAsync();

        return eliminados > 0;
    }

    public async Task<Prestamos?> Buscar(int prestamoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Prestamos
        .FirstOrDefaultAsync(p => p.PrestamoId == prestamoId);
    }

    public async Task<List<Prestamos>> GetList(Expression<Func<Prestamos, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Prestamos
        .AsNoTracking()
        .Where(criterio)
        .ToListAsync();
    }
}