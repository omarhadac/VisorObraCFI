using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Web;

namespace VisorObraCFI.Models
{
    [Table("PryAvance")]
    public class PryAvance
	{
        [Key]
        public int Id { get; set; }
        public DateTime Mes { get; set; }
        public DateTime FechaDeAlta { get; set; }
        public decimal? AvanceTeorico { get; set; }
        public decimal? AvanceReal { get; set; }
        public decimal? RelacionRealTeorico { get; set; }
        public decimal? AvanceRealMensual { get; set; }
        public string Observaciones { get; set; }
        public int? PryCertificacionesMensuales_Id { get; set; }
        public int? PryProyectoContratacionDirectaCertificacion_Id { get; set; }
        public int? PryProyectoAdicionalCertificacion_Id { get; set; }
        public int? PryProyectoLegitimoAbonoCertificacion_Id { get; set; }
        public int PryAvancePresupuestario_Id { get; set; }
        public int? PryProyectoPlanificacion_Id { get; set; }
        public int? PryProyectoContratacionDirecta_Id { get; set; }
        public int? PryProyectoAdicional_Id { get; set; }
        public int? PryProyectoLegitimoAbono_Id { get; set; }
        public bool Eliminado { get; set; }
        public decimal? AvanceRealInicial { get; set; }
        public bool? IsFirst { get; set; }
        public int? UsuModif { get; set; }
        public DateTime? FechaModif { get; set; }
        public int? AvanceFinanciero { get; set; }
        public int? NumeroConvenio { get; set; }
        public int? NumeroCertificado { get; set; }
        public int IdTipo { get; set; }
        public int? PryProyectoAlteracion_Nro { get; set; }
        public int? PryProyectoAlteracion_ID { get; set; }
        [NotMapped]
        public string MesFormatted => Mes.ToString("MM-yy", CultureInfo.CreateSpecificCulture("es-ES"));

    }
}