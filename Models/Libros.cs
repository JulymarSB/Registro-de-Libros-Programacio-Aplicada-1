using System.ComponentModel.DataAnnotations;

namespace RegistroLibros.Models;

public partial class Libros
{
    [Key]
    public int LibroId{ get; set; }

    [Required(ErrorMessage = "Error. El titulo es obligatorio.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Error. El autor es obligatorio-")]
    public string Autor { get; set; } = string.Empty;

    [Required(ErrorMessage = "Error. El año de publicación es obligatorio.")]
    public int AnoPublicacion { get; set; }

    public bool Disponible { get; set; } = true;
}
