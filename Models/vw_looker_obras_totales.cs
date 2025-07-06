using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VisorObraCFI.Models
{
	public class vw_looker_obras_totales
	{
        [Key]
        public int PryProyecto_Id { get; set; }
        public decimal? TotalAlteraciones { get; set; }
        public decimal? TotalVariaciones { get; set; }
        public decimal? MontoContratado { get; set; }

    }
}