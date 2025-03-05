using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VisorObraCFI.Models
{
    [Table("LicProyectoFecha")]
    public class LicProyectoFecha
	{
        [Key]
        public int idLicProyectoFecha { get; set; }
        public int idProyecto { get; set; }
        public DateTime? fechaPublicacion { get; set; }
        public DateTime? fechaCierre { get; set; }
        public DateTime? fechaApertura { get; set; }
        public string horaApertura { get; set; }
        public int? esFracasada { get; set; }
    }
}