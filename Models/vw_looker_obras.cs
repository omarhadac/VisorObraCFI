using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VisorObraCFI.Models
{
    public class vw_looker_obras
    {
        [Key]
        public int PryProyecto_Id { get; set; }
        public int? CodGIS { get; set; }
        public int? PryProyectoPlanificacion_Id { get; set; }
        public int? IdDepartamento { get; set; }
        public string Departamento { get; set; }
        public string departamentoString { get; set; }

        public string Nombre { get; set; }
        public string Expediente { get; set; }
        public string Domicilio { get; set; }
        public string Dimension { get; set; }
        public string Categoria { get; set; }
        public string Estado { get; set; }
        public int? IdEstado { get; set; }
        public int? PryStage_Id { get; set; }
        public string Etapa { get; set; }
        public decimal MontoContratado { get; set; }
        public decimal? MontoOficial { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public DateTime? FechaFinActualizada { get; set; }
        public string Empresa { get; set; }
        public string Empresa_Cuit { get; set; }
        public int? OrganismoId { get; set; }
        public string Organismo { get; set; }
        public int? DependenciaId { get; set; }
        public string Dependencia { get; set; }
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
        public string TipoObra { get; set; }
        public decimal? MontoAdicional { get; set; }
        public int? CantidadAdicional { get; set; }
        public decimal? MontoContratacionDirecta { get; set; }
        public int? CantidadContratacionDirecta { get; set; }
        public decimal? MontoGastosGrales { get; set; }
        public int? CantidadGastosGrales { get; set; }
        public decimal? MontoLegAbono { get; set; }
        public int? CantidadLegAbono { get; set; }
        public decimal? TotalMultas { get; set; }
        public int? CantidadMulta { get; set; }
        public decimal? MontoSupresion { get; set; }
        public int? CantidadSupresion { get; set; }
        public decimal? MontoAnticipoFinanciero { get; set; }
        public decimal? MontoAnticipoFinancieroVP { get; set; }
        public int? CantidadAnticipos { get; set; }
        public decimal? OB_AcumuladoMensual { get; set; }
        public decimal? OB_AvanceReal { get; set; }
        public decimal? OB_AvanceTeorico { get; set; }
        public DateTime? OB_UltMesCert { get; set; }
        public int? OB_CantCert { get; set; }
        public decimal? OB_VarPrecio { get; set; }
        public decimal? OB_MontoPagado { get; set; }
        public int? AD_CantCertificados { get; set; }
        public decimal? AD_MontoCertificado { get; set; }
        public decimal? AD_MontoCertificadoVP { get; set; }
        public DateTime? AD_UltimoMesCertificado { get; set; }
        public int? CD_CantCertificados { get; set; }
        public decimal? CD_MontoCertificado { get; set; }
        public decimal? CD_MontoCertificadoVP { get; set; }
        public DateTime? CCD_UltimoMesCertificado { get; set; }
        public int? LA_CantCertificados { get; set; }
        public decimal? LA_MontoCertificado { get; set; }
        public decimal? LA_MontoCertificadoVP { get; set; }
        public DateTime? LA_UltimoMesCertificado { get; set; }
        public string Fuente { get; set; }
        public bool? MostrarEnGis { get; set; }
        public string AdminContrato { get; set; }
        public string Inspector { get; set; }
        public string Financiamiento { get; set; }
        public int? IdOrgano { get; set; }
        public string NombreOrgano { get; set; }
        public string ObservacionSubetapa { get; set; }
        public bool esEntregada { get; set; }
        public string puntosLineaSeleccionado { get; set; }
        public string segmentTypes { get; set; }
        public string puntosLinea { get; set; }
    }
}