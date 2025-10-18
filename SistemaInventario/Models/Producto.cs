using System.ComponentModel.DataAnnotations;

namespace SistemaInventario.Models
{
    public class Producto
    {
        public int ProductoId { get; set; }

        [Required]
        [StringLength(120)]
        [Display(Name = "Nombre del producto")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Display(Name = "Precio costo")]
        [Range(typeof(decimal), "0", "9999999999999999")]
        public decimal PrecioCosto { get; set; }

        [Display(Name = "Precio venta")]
        [Range(typeof(decimal), "0", "9999999999999999")]
        public decimal PrecioVenta { get; set; }

        [Display(Name = "Stock actual")]
        [Range(0, int.MaxValue)]
        public int StockActual { get; set; }

        [Display(Name = "Stock mínimo")]
        [Range(0, int.MaxValue)]
        public int StockMinimo { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; }

        [Display(Name = "Proveedor")]
        public int ProveedorId { get; set; }

        public virtual Proveedor Proveedor { get; set; }
    }
}
