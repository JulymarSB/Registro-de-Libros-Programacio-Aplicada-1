using Microsoft.EntityFrameworkCore;
using RegistroLibros.Models;

namespace RegistroLibros.Context;

public class Contexto : DbContext
{
    public Contexto(DbContextOptions <Contexto> options) : base(options){}

    public virtual DbSet<Libros> Libros {get; set;}
    public virtual DbSet<Estudiantes> Estudiantes { get; set; }
    public virtual DbSet<Prestamos> Prestamos { get; set; }

    public virtual DbSet<Entradas> Entradas { get; set; }
}
