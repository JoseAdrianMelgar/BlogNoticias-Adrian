using System;
using System.ComponentModel.DataAnnotations;

namespace SistemaInventario.Models
{
    public enum TipoMovimiento
    {
        Entrada = 1,
        Salida = 2
    }

    public class MovimientoInventario
    {
        public int MovimientoInventarioId { get; set; }

        [Display(Name = "Producto")]
        [Required]
        public int ProductoId { get; set; }

        public virtual Producto Producto { get; set; }

        [Display(Name = "Tipo de movimiento")]
        [Required]
        public TipoMovimiento Tipo { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero.")]
        public int Cantidad { get; set; }

        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }

        [Display(Name = "Proveedor")]
        public int? ProveedorId { get; set; }

        public virtual Proveedor Proveedor { get; set; }

        [Display(Name = "Observaciones")]
        public string Observaciones { get; set; }
    }
}
