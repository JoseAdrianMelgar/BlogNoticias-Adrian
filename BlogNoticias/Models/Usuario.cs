using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogNoticias.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        public string NombreCompleto { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(120)]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        public bool EsAdministrador { get; set; }

        public virtual ICollection<Publicacion> Publicaciones { get; set; } = new List<Publicacion>();

        public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
    }
}
