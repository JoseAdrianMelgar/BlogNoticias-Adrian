using System;
using System.ComponentModel.DataAnnotations;

namespace BlogNoticias.Models
{
    public class Comentario
    {
        public int Id { get; set; }

        [Required]
        public string Contenido { get; set; }

        public DateTime FechaComentario { get; set; } = DateTime.UtcNow;

        public int AutorId { get; set; }

        public int PublicacionId { get; set; }

        public virtual Usuario Autor { get; set; }

        public virtual Publicacion Publicacion { get; set; }
    }
}
