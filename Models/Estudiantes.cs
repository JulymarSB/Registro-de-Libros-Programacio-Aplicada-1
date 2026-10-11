using System.ComponentModel.DataAnnotations;

namespace RegistroLibros.Models;

public partial class Estudiantes
{
		[Key]
		public int EstudianteId { get; set; }

		[Required(ErrorMessage = "El campo Nombres es obligatorio.")] 
		public string Nombres { get; set; } = string.Empty;

		[Required(ErrorMessage = "El campo Dirección es obligatorio.")]
		public string Direccion { get; set; } = string.Empty;

		[Required(ErrorMessage = "El campo Email es obligatorio.")]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "El campo Fecha de Nacimiento es obligatorio.")]
		public DateTime? FechaNacimiento { get; set; }
		
}
