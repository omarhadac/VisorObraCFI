using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace VisorObraCFI.DTO
{
	public class ObraGrilla
    {
        public int? IdObra { get; set; }
        public string Nombre { get; set; }
        public string Estado { get; set; }
        public string Dependencia { get; set; }
        public string Empresa { get; set; }
        public string Departamento { get; set; }
        public decimal? Contrato { get; set; }
        public decimal? TotalPagado { get; set; }
        public decimal? TotalObra
        {
            get
            {
                return (TotalAlteraciones ?? 0) + (TotalVariaciones ?? 0) + (MontoContratado ?? 0);
            }
        }

        public decimal? TotalAlteraciones { get; set; }
        public int? idEstado { get; set; }
        public decimal? TotalVariaciones { get; set; }
        public decimal? MontoContratado { get; set; }
        public decimal? Avance { get; set; }
        public DateTime? Inicio { get; set; }
        public DateTime? Fin { get; set; }
        public DateTime? Apertura { get; set; }
        public DateTime? Publicacion { get; set; }
        public int? Financiamiento { get; set; }
        // Propiedades calculadas para formatear los valores
        public string NombreFormatted => Nombre ?? string.Empty;
        public string EstadoFormatted => Estado ?? string.Empty;
        public string EmpresaFormatted => Empresa ?? string.Empty;
        public string DependenciaFormatted => Dependencia ?? string.Empty;
        public string DepartamentoFormatted => Departamento ?? string.Empty;
        public string ContratoFormatted => Contrato.HasValue ? $"${Contrato.Value.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))}" : "$0";
        public string TotalObraFormatted => TotalObra.HasValue ? $"${TotalObra.Value.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))}" : "$0";
        public string TotalPagadoFormatted => TotalPagado.HasValue ? $"${TotalPagado.Value.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))}" : "$0";
        public string AvanceFormatted => Avance.HasValue ? $"{Avance.Value.ToString("N2", CultureInfo.CreateSpecificCulture("es-ES"))}%" : "0.00%";
        public string InicioFormatted => Inicio?.ToString("dd/MM/yyyy") ?? string.Empty;
        public string FinFormatted => Fin?.ToString("dd/MM/yyyy") ?? string.Empty;
        public string PublicacionFormatted => Publicacion?.ToString("dd/MM/yyyy") ?? string.Empty;
        public string AperturaFormatted => Apertura?.ToString("dd/MM/yyyy") ?? string.Empty;
        public string EsResarcimiento => Financiamiento.Value == 9 ? "Si" : "No";

    }
}