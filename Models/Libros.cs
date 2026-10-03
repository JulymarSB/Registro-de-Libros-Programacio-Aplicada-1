using System.ComponentModel.DataAnnotations;
namespace RegistroLibros.Models;
public class Libros
{
    [Key]
    public int LibroId{get; set;}

    [Required(ErrorMessage = "Error")]
    public string Titulo {get; set;} = string.Empty;

    [Required(ErrorMessage = "Error")]
    public string Autor {get; set;}= string.Empty;

    [Required(ErrorMessage = "Error")]
    public int AnoPublicacion {get; set;}

    public bool Disponible { get; set; } = true;
}
