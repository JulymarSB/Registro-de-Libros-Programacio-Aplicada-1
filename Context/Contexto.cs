using Microsoft.EntityFrameworkCore;
using RegistroLibros.Models;

namespace RegistroLibros.DAL;

public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto>options) : base(options){}


    public DbSet<Libros> Libros {get; set;}

    public DbSet<Estudiantes> Estudiantes { get; set; }
}