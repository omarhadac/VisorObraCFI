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
        public int? IdOrganismo { get; set; }
        public decimal? Contrato { get; set; }
        public decimal? TotalPagado { get; set; }
        public decimal? Avance { get; set; }
        public DateTime? Inicio { get; set; }
        public DateTime? Fin { get; set; }
        public decimal? Latitud { get;set; }
        public decimal? Longitud { get; set; }
        public List<string> ListaArchivos { get; set; }
        public List<AvanceGrafico> ChartData { get; set; } 
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
        public string LatitudFormatted => Latitud.HasValue ? Latitud.Value.ToString("N6", CultureInfo.InvariantCulture) : "0.000000";
        public string LongitudFormatted => Longitud.HasValue ? Longitud.Value.ToString("N6", CultureInfo.InvariantCulture) : "0.000000";
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
                    default:
                        return "/Content/Iconos/aysam.png";
                }
            }
        }
    }
}