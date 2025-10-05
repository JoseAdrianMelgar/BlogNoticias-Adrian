using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogNoticias.Models
{
    public class Publicacion
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; }

        [Required]
        public string Contenido { get; set; }

        public DateTime FechaPublicacion { get; set; } = DateTime.UtcNow;

        public int AutorId { get; set; }

        public int CategoriaId { get; set; }

        public virtual Usuario Autor { get; set; }

        public virtual Categoria Categoria { get; set; }

        public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
    }
}
