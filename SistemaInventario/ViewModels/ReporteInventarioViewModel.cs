using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SistemaInventario.Models;

namespace SistemaInventario.ViewModels
{
    public class ReporteInventarioFiltro
    {
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int? ProductoId { get; set; }
        public int? ProveedorId { get; set; }
        public string AgruparPor { get; set; }
    }

    public class ReporteInventarioViewModel
    {
        public ReporteInventarioViewModel()
        {
            Filtro = new ReporteInventarioFiltro
            {
                AgruparPor = "dia"
            };
            Movimientos = new List<MovimientoInventario>();
        }

        public ReporteInventarioFiltro Filtro { get; set; }
        public IEnumerable<MovimientoInventario> Movimientos { get; set; }
        public int TotalEntradas { get; set; }
        public int TotalSalidas { get; set; }
        public IEnumerable<SelectListItem> Productos { get; set; }
        public IEnumerable<SelectListItem> Proveedores { get; set; }
        public string ChartLabelsJson { get; set; }
        public string ChartDataJson { get; set; }
        public string ChartTitle { get; set; }
    }
}
