using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SistemaInventario.Models;

namespace SistemaInventario.ViewModels
{
    public class MovimientoInventarioViewModel
    {
        public MovimientoInventarioViewModel()
        {
            Fecha = DateTime.Now;
            Tipo = TipoMovimiento.Entrada;
            Cantidad = 1;
        }

        [Display(Name = "Tipo de movimiento")]
        [Required]
        public TipoMovimiento Tipo { get; set; }

        [Display(Name = "Producto")]
        [Required]
        public int ProductoId { get; set; }

        [Display(Name = "Proveedor")]
        public int? ProveedorId { get; set; }

        [Display(Name = "Cantidad")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero.")]
        public int Cantidad { get; set; }

        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }

        [Display(Name = "Observaciones")]
        public string Observaciones { get; set; }

        public IEnumerable<SelectListItem> Productos { get; set; }
        public IEnumerable<SelectListItem> Proveedores { get; set; }
    }
}
