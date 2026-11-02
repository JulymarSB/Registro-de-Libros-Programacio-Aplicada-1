using System.ComponentModel.DataAnnotations;
using Aplicada1.Core;

namespace RegistroLibros.Models;

public class Entradas
{
    [Key]
    public int EntradaId { get; set; }
    
    [Required(ErrorMessage = "La fecha es obligatoria")]
    public DateTime Fecha { get; set; } = DateTime.Now;
    
    [Required(ErrorMessage = "El concepto es obligatorio")]
    public string Concepto { get; set; } = string.Empty;

    public int Total { get; set; }

    public virtual ICollection<EntradaDetalle> EntradaDetalle { get; set; } = new List<EntradaDetalle>();
}