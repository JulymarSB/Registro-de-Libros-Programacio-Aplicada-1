using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using RegistroLibros.Models;
using RegistroLibros.Context;
using System.Linq.Expressions;

namespace RegistroLibros.Services;

public class EntradaServices(IDbContextFactory<Contexto> contextFactory) : IServiceResult<Entradas, int>
{
    public async Task<Result> Guardar(Entradas entrada)
    {
        if (!await Existe(entrada.EntradaId))
        {
            var Insertar = await this.Insertar(entrada);
            return Result.Success(Insertar);
        }
        else
        {
            var Modificar = await this.Modificar(entrada);
            return Result.Success(Modificar);
        }
    }

    private async Task<bool> Existe(int entradaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Entradas
            .AnyAsync(e => e.EntradaId == entradaId);
    }

    private async Task<bool> Insertar(Entradas entrada)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        foreach (var detalle in entrada.EntradaDetalle)
        {
            var libro = await contexto.Libros.FindAsync(detalle.LibroId);
            if (libro != null)
            {
                libro.CantidadDisponible += detalle.Cantidad;
            }
        }

        contexto.Entradas.Add(entrada);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Entradas entrada)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var entradaAnterior = await contexto.Entradas
            .Include(e => e.EntradaDetalle)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.EntradaId == entrada.EntradaId);

        if (entradaAnterior == null) return false;

        foreach (var detalleAnterior in entradaAnterior.EntradaDetalle)
        {
            var libro = await contexto.Libros.FindAsync(detalleAnterior.LibroId);
            if (libro != null)
            {
                libro.CantidadDisponible -= detalleAnterior.Cantidad;
            }
        }

        var detallesViejos = await contexto.Set<EntradaDetalle>()
            .Where(d => d.EntradaId == entrada.EntradaId)
            .ToListAsync();
        contexto.Set<EntradaDetalle>().RemoveRange(detallesViejos);

        foreach (var nuevoDetalle in entrada.EntradaDetalle)
        {
            var libro = await contexto.Libros.FindAsync(nuevoDetalle.LibroId);
            if (libro != null)
            {
                libro.CantidadDisponible += nuevoDetalle.Cantidad;
            }
        }

        contexto.Entradas.Update(entrada);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result<Entradas?>> Buscar(int id)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Entradas
            .Include(e => e.EntradaDetalle)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.EntradaId == id);
    }

    public async Task<Result> Eliminar(int entradaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var entrada = await contexto.Entradas
            .Include(e => e.EntradaDetalle)
            .FirstOrDefaultAsync(e => e.EntradaId == entradaId);

        if (entrada == null)
            return Result.Success(false);

        foreach (var detalle in entrada.EntradaDetalle)
        {
            var libro = await contexto.Libros.FindAsync(detalle.LibroId);
            if (libro != null)
            {
                libro.CantidadDisponible -= detalle.Cantidad;
            }
        }

        contexto.Entradas.Remove(entrada);
        var elimino = await contexto.SaveChangesAsync() > 0;

        return Result.Success(elimino);
    }

    public async Task<Result<List<Entradas>>> GetList(Expression<Func<Entradas, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();
        return await contexto.Entradas
            .Include(e => e.EntradaDetalle)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
}