﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VisorObraCFI.Models
{
    [Table("GrlDepartament")]
    public class GrlDepartament
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}