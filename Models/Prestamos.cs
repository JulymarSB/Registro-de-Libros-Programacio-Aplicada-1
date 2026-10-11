using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroLibros.Models;

public partial class Prestamos
{
    [Key]
    public int PrestamoId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un estudiante.")]
    [ForeignKey("Estudiante")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un estudiante válido.")]
    public int EstudianteId { get; set; }

    public virtual  Estudiantes Estudiante { get; set; }


    [Required(ErrorMessage = "Debe seleccionar un libro.")]
    [ForeignKey("Libro")]
    [Range(1, int.MaxValue, ErrorMessage = = "Debe seleccionar un libro válido.")]
    public int LibroId { get; set; }

    public virtual Libros Libro { get; set; }
    
    public DateTime FechaPrestamo { get; set; } = DateTime.Now;
    public DateTime? FechaDevolucion { get; set; } = DateTime.Now;
}
