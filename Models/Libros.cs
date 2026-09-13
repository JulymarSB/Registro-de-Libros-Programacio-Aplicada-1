using System.ComponentModel.DataAnnotations;
namespace RegistroLibros.Models;
public class Libros
{
    [Key]
    public int LibroId{get; set;}

    [Required(ErrorMessage = "Error")]
    public string Titulo {get; set;} = null!;
    [Required(ErrorMessage = "Error")]
    public string Autor {get; set;}= null!;
    [Required(ErrorMessage = "Error")]
    public int AnoPublicacion {get; set;}
    

}