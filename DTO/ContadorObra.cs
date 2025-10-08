using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VisorObraCFI.DTO
{
    public class ContadorObra
    {
        public int? IdOrganismo { get; set; }
        public string Organismo { get; set; }
        public int? CantidadObraEjecucion { get; set; }
        public long? MontoObraEjecucion { get; set; }
        public int? CantidadObraLicitacion { get; set; }
        public long? MontoObraLicitacion { get; set; }
        public int? CantidadObraFinalizada { get; set; }
        public int? CantidadObraParalizada { get; set; }
    }
}