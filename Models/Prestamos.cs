using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RegistroLibros.Models;

public class Prestamos
{
    [Key]
    public int PrestamoId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un estudiante.")]
    public int EstudianteId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un libro.")]
    public int LibroId { get; set; }

    public DateTime FechaPrestamo { get; set; } = DateTime.Now;

    public DateTime FechaDevolucion { get; set; }
}