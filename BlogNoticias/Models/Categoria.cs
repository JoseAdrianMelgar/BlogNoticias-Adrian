using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogNoticias.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(255)]
        public string Descripcion { get; set; }

        public virtual ICollection<Publicacion> Publicaciones { get; set; } = new List<Publicacion>();
    }
}
