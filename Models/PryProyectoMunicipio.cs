using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VisorObraCFI.Models
{
    [Table("PryProyectoMunicipio")]
    public class PryProyectoMunicipio
    {
        public int Id { get; set; }
        public int PryProyecto_Id { get; set; }

        public int PryMunicipio_Id { get; set; }
    }
}