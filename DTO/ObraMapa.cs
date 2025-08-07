using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace VisorObraCFI.DTO
{
	public class ObraMapa
	{
        public int? IdObra { get; set; }
        public string Nombre { get; set; }
        public string Estado { get; set; }
        public string Dependencia { get; set; }
        public string Departamento { get; set; }
        public string Direccion { get; set; }
        public string Empresa { get; set; }
        public string MesCertificado { get; set; }
        public int? IdOrganismo { get; set; }
        public int? idEstado { get; set; }
        public decimal? Contrato { get; set; }
        public decimal? MontoOficial { get; set; }
        public decimal? TotalPagado { get; set; }
        public decimal? Avance { get; set; }
        public DateTime? Inicio { get; set; }
        public DateTime? Fin { get; set; }
        public decimal? Latitud { get;set; }
        public decimal? Longitud { get; set; }
        public int? Financiamiento { get; set; }
        public decimal? MontoContratado { get; set; }
        public decimal? TotalVariaciones { get; set; }
        public DateTime? Apertura { get; set; }
        public DateTime? Publicacion { get; set; }
        public decimal? TotalObra
        {
            get
            {
                return (TotalAlteraciones ?? 0) + (TotalVariaciones ?? 0) + (MontoContratado ?? 0);
            }
        }

        public decimal? TotalAlteraciones { get; set; }
        public List<ArchivoObraInfo> ListaArchivos { get; set; }
        public List<AvanceGrafico> ChartData { get; set; } 
        // Propiedades calculadas para formatear los valores
        public string NombreFormatted => Nombre ?? string.Empty;
        public string EstadoFormatted => Estado ?? string.Empty;
        public decimal? MontoAdicional { get; set; }
        public decimal? MontoContratacionDirecta { get; set; }
        public decimal? OB_VarPrecio { get; set; }
        public string EsResarcimiento
        {
            get
            {
                if (!Financiamiento.HasValue)
                    return "Sin datos";
                if (Financiamiento.Value == 1)
                    return "Sin Financiamiento";
                if (Financiamiento.Value == 2)
                    return "Provincial";
                if (Financiamiento.Value == 3)
                    return "Nacional";
                if (Financiamiento.Value == 4)
                    return "Internacional";
                if (Financiamiento.Value == 5)
                    return "Nacional/Provincial";
                if (Financiamiento.Value == 6)
                    return "Internacional/Provincial";
                if (Financiamiento.Value == 7)
                    return "Internacional/Nacional";
                if (Financiamiento.Value == 8)
                    return "Internacional/Nacional/Provincial";
                if (Financiamiento.Value == 9)
                    return "Resarcimiento";
                if (Financiamiento.Value == 10)
                    return "Municipal";
                return "Sin datos";
            }
        }
        public decimal? MontoTotal
        {
            get
            {
                if (MontoContratado == null && MontoAdicional == null && MontoContratacionDirecta == null && OB_VarPrecio == null)
                    return null;
                return (MontoContratado ?? 0)
                     + (MontoAdicional ?? 0)
                     + (MontoContratacionDirecta ?? 0)
                     + (OB_VarPrecio ?? 0);
            }
        }
        public string PresupuestoFormatted => MontoOficial.HasValue ? $"${MontoOficial.Value.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))}" : "$0";
        public string ContratoFormatted => MontoTotal.HasValue ? $"${MontoTotal.Value.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))}" : "$0";
        public string DependenciaFormatted => Dependencia ?? string.Empty;
        public string DepartamentoFormatted => Departamento ?? string.Empty;
        public string DireccionFormatted => Direccion ?? string.Empty;
        public string TotalPagadoFormatted => TotalPagado.HasValue ? $"${TotalPagado.Value.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))}" : "$0";
        public string MontoTotalFormatted => TotalPagado.HasValue ? $"${TotalPagado.Value.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))}" : "$0";
        public string AvanceFormatted => Avance.HasValue ? $"{Avance.Value.ToString("N2", CultureInfo.CreateSpecificCulture("es-ES"))}%" : "0.00%";
        public string InicioFormatted => Inicio?.ToString("dd/MM/yyyy") ?? string.Empty;
        public string FinFormatted => Fin?.ToString("dd/MM/yyyy") ?? string.Empty;
        public string LatitudFormatted => Latitud.HasValue ? Latitud.Value.ToString("N6", CultureInfo.InvariantCulture) : "0.000000";
        public string LongitudFormatted => Longitud.HasValue ? Longitud.Value.ToString("N6", CultureInfo.InvariantCulture) : "0.000000";
        public string TotalObraFormatted => TotalObra.HasValue ? $"${TotalObra.Value.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))}" : "$0";
        public string PublicacionFormatted => Publicacion?.ToString("dd/MM/yyyy") ?? string.Empty;
        public string AperturaFormatted => Apertura?.ToString("dd/MM/yyyy") ?? string.Empty;
        public string Icono
        {
            get
            {
                switch (IdOrganismo)
                {
                    case 2:
                        return "/Content/Iconos/infraestructura.png";
                    case 4:
                        return "/Content/Iconos/ipv.png";
                    case 9:
                        return "/Content/Iconos/vialidad.png";
                    case 14:
                        return "/Content/Iconos/aysam.png";
                    case 20:
                        return "/Content/Iconos/irrigacion.png";
                    default:
                        return "/Content/Iconos/aysam.png";
                }
            }
        }
    }
    public class ArchivoObraInfo
    {
        public string Url { get; set; }
        public string FechaImagen { get; set; }
    }
}