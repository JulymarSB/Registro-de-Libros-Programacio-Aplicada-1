using System.Linq.Expressions;
using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RegistroLibros.DAL;
using RegistroLibros.Models;



namespace RegistroLibros.Services;

public class PrestamoService(IDbContextFactory<Contexto> DbFactory) : IService<Prestamos, int>
{
    public async Task<bool> Guardar(Prestamos prestamo)
    {
        if(prestamo.PrestamoId == 0)
        {
            return await Insertar(prestamo);
        }
        else
        {
            return await Modificar(prestamo);
        }
    }

    private async Task<bool>Insertar(Prestamos prestamo)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var estudianteConPrestamo = await contexto.Prestamos.AnyAsync(p => p.EstudianteId == prestamo.EstudianteId);

        if(estudianteConPrestamo)
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
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Prestamos.Update(prestamo);
        return await contexto.SaveChangesAsync()>0;
    }

    public async Task<bool>Eliminar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var prestamo = await contexto.Prestamos.FindAsync(id);

        if(prestamo == null)
        {
            return false;
        }

        var libro = await contexto.Libros.FindAsync(prestamo.LibroId);
        if(libro != null)
        {
            libro.Disponible = true;
            contexto.Libros.Update(libro);
        }

        contexto.Prestamos.Remove(prestamo);
        return await contexto.SaveChangesAsync() >0;
    }

    public async Task<Prestamos?>Buscar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Prestamos.AsNoTracking().FirstOrDefaultAsync(p => p.PrestamoId == id);
    }

    public async Task<List<Prestamos>> GetList(Expression<Func<Prestamos, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Prestamos.AsNoTracking().Where(criterio).ToListAsync();
    }
}