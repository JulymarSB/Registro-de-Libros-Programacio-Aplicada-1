using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroLibros.Models;

public class EntradaDetalle
{
    [Key]
    public int Id { get; set; }

    public int EntradaId { get; set; }

    public int LibroId { get; set; }

    public int Cantidad { get; set; }

    [ForeignKey("EntradaId")]
    public virtual Entradas? Entrada { get; set; }
}
