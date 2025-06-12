using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisorObraCFI.Models
{
    [Table("PryArchivosObra")]
    public class PryArchivosObra
    {
        [Key]
        public int IdArchivo { get; set; }
        public int? IdProyecto { get; set; }
        public int? EstadoProyecto { get; set; }
        public string ArchivoAdjunto { get; set; }
        public string Descripcion { get; set; }
        public string NombreArchivo { get; set; }
        public int? Estado { get; set; }
        public DateTime? FechaCarga { get; set; }
        public DateTime? FechaImagen { get; set; }
        public int? IdTipoArchivo { get; set; }
        [NotMapped]
        public string Url => $"https://buhogestion.mendoza.gov.ar/Files/PryArchivosObra/{NombreArchivo}";
        [NotMapped]
        public string fechaImagenString => FechaImagen.HasValue ? FechaImagen.Value.ToString("dd/MM/yyyy") : "Sin fecha";

    }
}