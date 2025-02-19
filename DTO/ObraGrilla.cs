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
        public string Departamento { get; set; }
        public decimal? Contrato { get; set; }
        public decimal? TotalPagado { get; set; }
        public decimal? Avance { get; set; }
        public DateTime? Inicio { get; set; }
        public DateTime? Fin { get; set; }
        
        // Propiedades calculadas para formatear los valores
        public string NombreFormatted => Nombre ?? string.Empty;
        public string EstadoFormatted => Estado ?? string.Empty;
        public string DependenciaFormatted => Dependencia ?? string.Empty;
        public string DepartamentoFormatted => Departamento ?? string.Empty;
        public string ContratoFormatted => Contrato.HasValue ? $"${Contrato.Value.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))}" : "$0";
        public string TotalPagadoFormatted => TotalPagado.HasValue ? $"${TotalPagado.Value.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))}" : "$0";
        public string AvanceFormatted => Avance.HasValue ? $"{Avance.Value.ToString("N2", CultureInfo.CreateSpecificCulture("es-ES"))}%" : "0.00%";
        public string InicioFormatted => Inicio?.ToString("dd/MM/yyyy") ?? string.Empty;
        public string FinFormatted => Fin?.ToString("dd/MM/yyyy") ?? string.Empty;

    }
}