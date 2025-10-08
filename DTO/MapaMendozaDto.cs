using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisorObraCFI.DTO
{
	public class MapaMendozaDto
	{
		public List<DetalleDistrito> ListaPrimerDistrito;
        public List<DetalleDistrito> ListaSegundoDistrito;
        public List<DetalleDistrito> ListaTercerDistrito;
        public List<DetalleDistrito> ListaCuartoDistrito;
        public List<DetalleDistrito> ListaTotales;
    }
	public class DetalleDistrito
	{
		public string NombreDistrito { get; set; }
		public int? ObrasLicitacion { get; set; }
        public int? ObrasEjecucion { get; set; }
		public int? ObrasParalizadas { get;set; }
        public int? ObrasEntregadas { get; set; }
		public int? NroDistrito { get; set; }
    }
}