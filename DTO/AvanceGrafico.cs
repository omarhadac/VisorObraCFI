using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace VisorObraCFI.DTO
{
	public class AvanceGrafico
	{		
        public DateTime Mes { get; set; }
        public decimal? AvanceReal { get; set; }
        public decimal? AvanceTeorico { get; set; }
        public string MesFormatted => Mes.ToString("MM-yy", CultureInfo.CreateSpecificCulture("es-ES"));
    }
}