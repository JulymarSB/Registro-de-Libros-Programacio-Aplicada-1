using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using RegistroLibros.Context;
using RegistroLibros.Models;

namespace RegistroLibros.Services;

public class PrestamoService(IDbContextFactory<Contexto> contextFactory) : IServiceResult<Prestamos, int>
{
    
    public async Task<Result> Guardar(Prestamos prestamo)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        
        if (!await Existe(prestamo.PrestamoId))
        {
            var inserto = await Insertar(prestamo);
            return Result.Success(inserto);
        }
        else
        {
            var modifico = await Modificar(prestamo);
            return Result.Success(modifico);
        }
    }

    public async Task<Result> Devolver(int prestamoId, DateTime fechaDevolucion)
{
    await using var contexto = await contextFactory.CreateDbContextAsync();

    var prestamo = await contexto.Prestamos.FirstOrDefaultAsync(p => p.PrestamoId == prestamoId);
    if (prestamo == null || prestamo.FechaDevolucion != null)
    {
        return Result.Success("El préstamo no fue encontrado o ya fue devuelto.");
    }
     
    if (fechaDevolucion.Date < prestamo.FechaPrestamo.Date)
    {
        return Result.Success("La fecha de devolución no puede ser anterior a la fecha del préstamo.");
    }

    var libro = await contexto.Libros.FirstOrDefaultAsync(l => l.LibroId == prestamo.LibroId);
    if (libro == null)
    {
        return Result.Success("El libro asociado no fue encontrado.");
    }

    prestamo.FechaDevolucion = fechaDevolucion.Date;
    libro.Disponible = true;
    
    var guardado = await contexto.SaveChangesAsync() > 0;

    if (guardado)
    {
        return Result.Success("Devolución registrada correctamente.");
    }

    return Result.Success("No se pudo guardar la devolución en la base de datos.");
}
    private async Task<bool> Existe(int prestamoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Prestamos.AnyAsync(p => p.PrestamoId == prestamoId);
    }
    private async Task<bool> Insertar(Prestamos prestamo)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var estudianteConPrestamoActivo = await contexto.Prestamos.Include(p => p.Libro).AnyAsync(p => p.EstudianteId == p.EstudianteId && p.Libro.Disponible == false);
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

    public async Task<Result> Eliminar(int prestamoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        var prestamo = await contexto.Prestamos.FindAsync(prestamoId);

        if(prestamo == null)
        {
            return Result.Success(false);
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

        return Result.Success(eliminados);
    }

    public async Task<Result<Prestamos?>> Buscar(int prestamoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Prestamos
        .FirstOrDefaultAsync(p => p.PrestamoId == prestamoId);
    }

    public async Task<Result<List<Prestamos>>> GetList(Expression<Func<Prestamos, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Prestamos
        .AsNoTracking()
        .Where(criterio)
        .ToListAsync();
    }
}