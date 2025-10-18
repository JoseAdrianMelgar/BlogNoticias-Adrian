using System.ComponentModel.DataAnnotations;

namespace SistemaInventario.Models
{
    public class Proveedor
    {
        public int ProveedorId { get; set; }

        [Required]
        [StringLength(120)]
        [Display(Name = "Nombre del proveedor")]
        public string Nombre { get; set; }

        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; }
    }
}
