using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VisorObraCFI.Models;

namespace VisorObraCFI
{
    public class ObraController : ApiController
    {
        private MySqlDbContext db = new MySqlDbContext();

        [Route("api/Obra/BuscarTotalesObras")]
        [System.Web.Http.ActionName("BuscarTotalesObras")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetData()
        {
            var obras = db.vw_looker_obras.ToList();
            return Ok(obras);
        }
    }
}