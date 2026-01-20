using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VisorObraCFI.Models
{
    [Table("PryProyecto")]
    public class PryProyecto
	{
        public int Id { get; set; }
        public string CodBip { get; set; }
        public int? CodGIS { get; set; }
        public int? CUC { get; set; }
        public string Nombre { get; set; }
        public string Expediente { get; set; }
        public string Observacion { get; set; }
        public DateTime? FechaDeAlta { get; set; }
        public int PryFuenteFinanciamiento_Id { get; set; }
        public int? TipoDeContratacion_Id { get; set; }
        public string Dirección { get; set; }
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
        public decimal? Latitud_20240418 { get; set; }
        public decimal? Longitud_20240418 { get; set; }
        public int? GrlZone_Id { get; set; }
        public int? GrlDepartament_Id { get; set; }
        public int? PryOrganismoEjecutor_Id { get; set; }
        public int? PryEmpresa_Id { get; set; }
        public bool Eliminado { get; set; }
        public int? GrlTypeState_Id { get; set; }
        public int? GrlTypeState_Id_al_20240418 { get; set; }
        public int? Dias { get; set; }
        public bool? EsAlteracion { get; set; }
        public int? IdProyectoPadre { get; set; }
        public int? Plazo { get; set; }
        public decimal? cantIntervenciones { get; set; }
        public int? usuCarga { get; set; }
        public int? oficinaCarga { get; set; }
        public int? tipoAlteracion_Id { get; set; }
        public byte[] File { get; set; }
        public int? usuModificacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int? tipoObra_Id { get; set; }
        public string descripcion { get; set; }
        public int? tipoUnidad_Id { get; set; }
        public int? prioridad_Type_Id { get; set; }
        public int? Subcategoria_Id { get; set; }
        public string codigoObra { get; set; }
        public string normaContrato { get; set; }
        public string normaAdjudicacion { get; set; }
        public string decretoContrato { get; set; }
        public string decretoAdjudicacion { get; set; }
        public int? ufiOrgano_Id { get; set; }
        public int? PryAlcance_Id { get; set; }
        public int? PryProyectoCategoria_Id { get; set; }
        public int? PryODS_Id { get; set; }
        public int? PryFinanciacion_Id { get; set; }
        public int? PryPreguntas_Id { get; set; }
        public string CaratulaExpediente { get; set; }
        public string funcionario { get; set; }
        public string cargoFuncionario { get; set; }
        public string responsableTec { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public string financiamiento { get; set; }
        public string fechaNormaAdjudicacion { get; set; }
        public string fechaNormaContrato { get; set; }
        public string manoObra { get; set; }
        public int? tipoMonedaMontoContratado { get; set; }
        public int? PryProyectoDimension_Id { get; set; }
        public int? ufiOrganoReceptor_Id { get; set; }
        public string duenoTerreno { get; set; }
        public string nomenCatastral { get; set; }
        public DateTime? fechaMonto { get; set; }
        public DateTime? fechaRecepcion { get; set; }
        public string numeroLicitacion { get; set; }
        public int? idAdminGeren { get; set; }
        public string telAdminGeren { get; set; }
        public string emailAdminGeren { get; set; }
        public int? idInspector { get; set; }
        public int? idPryProyectoTipoCertificacion { get; set; }
        public string MarcaArquitectura { get; set; }
        public int? CodIpv { get; set; }
        public string departamentoString { get; set; }

        public bool esEntregada { get; set; }

        public bool noVisor { get; set; }
    }
}