using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VisorObraCFI.DTO;
using VisorObraCFI.Models;

namespace VisorObraCFI
{
    public class ObraController : ApiController
    {
       

        [Route("api/Obra/BuscarTotalesObras")]
        [System.Web.Http.ActionName("BuscarTotalesObras")]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> BuscarTotalesObras()
        {
            try
            {
                using (var context = new MySqlDbContext())
                {
                    var listaEjecucion = await context.vw_looker_obras
                        .Where(x => x.IdEstado == 1 && (x.OrganismoId == 2 || x.OrganismoId == 4 || x.OrganismoId == 9 || x.OrganismoId == 14))
                        .Distinct().ToListAsync();

                    var listaLicitacion = await context.vw_looker_obras
                        .Where(x => x.PryStage_Id == 49 && x.IdEstado != 5 && (x.OrganismoId == 2 || x.OrganismoId == 4 || x.OrganismoId == 9 || x.OrganismoId == 14))
                        .Distinct().ToListAsync();

                    var tmp = new ContadorObra
                    {
                        CantidadObraEjecucion = listaEjecucion.Count,
                        MontoObraEjecucion = Convert.ToInt64(listaEjecucion.Sum(x => (decimal?)x.MontoContratado) ?? 0),
                        CantidadObraLicitacion = listaLicitacion.Count,
                        MontoObraLicitacion = Convert.ToInt64(listaLicitacion.Sum(x => (decimal?)x.MontoContratado) ?? 0)
                    };

                    return Ok(tmp);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/Obra/BuscarTotalesOrganismo")]
        [System.Web.Http.ActionName("BuscarTotalesOrganismo")]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> BuscarTotalesOrganismo()
        {
            try
            {
                using (var context = new MySqlDbContext())
                {
                    var listaEjecucion = await context.vw_looker_obras
                        .Where(x => x.IdEstado == 1)
                        .ToListAsync();

                    var listaLicitacion = await context.vw_looker_obras
                        .Where(x => x.PryStage_Id == 49 && x.IdEstado != 5)
                        .ToListAsync();

                    var lista = new List<ContadorObra>();

                    var listaEjecucionAySAM = listaEjecucion.Where(x => x.OrganismoId == 14).Distinct().ToList();
                    var listaLicitacionAySAM = listaLicitacion.Where(x => x.OrganismoId == 14).Distinct().ToList();

                    var listaEjecucionIPV = listaEjecucion.Where(x => x.OrganismoId == 4).Distinct().ToList();
                    var listaLicitacionIPV = listaLicitacion.Where(x => x.OrganismoId == 4).Distinct().ToList();

                    var listaEjecucionVialidad = listaEjecucion.Where(x => x.OrganismoId == 9).Distinct().ToList();
                    var listaLicitacionVialidad = listaLicitacion.Where(x => x.OrganismoId == 9).Distinct().ToList();

                    var listaEjecucionInfra = listaEjecucion.Where(x => x.OrganismoId == 2).Distinct().ToList();
                    var listaLicitacionInfra = listaLicitacion.Where(x => x.OrganismoId == 2).Distinct().ToList();

                    var unItemAysam = new ContadorObra();
                    unItemAysam.CantidadObraEjecucion = listaEjecucionAySAM.Count;
                    unItemAysam.MontoObraEjecucion = Convert.ToInt64(listaEjecucionAySAM.Sum(x => (decimal?)x.MontoContratado) ?? 0);
                    unItemAysam.CantidadObraLicitacion = listaLicitacionAySAM.Count;
                    unItemAysam.MontoObraLicitacion = Convert.ToInt64(listaLicitacionAySAM.Sum(x => (decimal?)x.MontoContratado) ?? 0);
                    unItemAysam.IdOrganismo = 14;
                    unItemAysam.Organismo = "AySAM";
                    lista.Add(unItemAysam);

                    var unItemIPV = new ContadorObra();
                    unItemIPV.CantidadObraEjecucion = listaEjecucionIPV.Count;
                    unItemIPV.MontoObraEjecucion = Convert.ToInt64(listaEjecucionIPV.Sum(x => (decimal?)x.MontoContratado) ?? 0);
                    unItemIPV.CantidadObraLicitacion = listaLicitacionIPV.Count;
                    unItemIPV.MontoObraLicitacion = Convert.ToInt64(listaLicitacionIPV.Sum(x => (decimal?)x.MontoContratado) ?? 0);
                    unItemIPV.IdOrganismo = 4;
                    unItemIPV.Organismo = "IPV";
                    lista.Add(unItemIPV);

                    var unItemVialidad = new ContadorObra();
                    unItemVialidad.CantidadObraEjecucion = listaEjecucionVialidad.Count;
                    unItemVialidad.MontoObraEjecucion = Convert.ToInt64(listaEjecucionVialidad.Sum(x => (decimal?)x.MontoContratado) ?? 0);
                    unItemVialidad.CantidadObraLicitacion = listaLicitacionVialidad.Count;
                    unItemVialidad.MontoObraLicitacion = Convert.ToInt64(listaLicitacionVialidad.Sum(x => (decimal?)x.MontoContratado) ?? 0);
                    unItemVialidad.IdOrganismo = 9;
                    unItemVialidad.Organismo = "Vialidad";
                    lista.Add(unItemVialidad);

                    var unItemInfra = new ContadorObra();
                    unItemInfra.CantidadObraEjecucion = listaEjecucionInfra.Count;
                    unItemInfra.MontoObraEjecucion = Convert.ToInt64(listaEjecucionInfra.Sum(x => (decimal?)x.MontoContratado) ?? 0);
                    unItemInfra.CantidadObraLicitacion = listaLicitacionInfra.Count;
                    unItemInfra.MontoObraLicitacion = Convert.ToInt64(listaLicitacionInfra.Sum(x => (decimal?)x.MontoContratado) ?? 0);
                    unItemInfra.IdOrganismo = 2;
                    unItemInfra.Organismo = "Infraestructura";
                    lista.Add(unItemInfra);

                    return Ok(lista);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return InternalServerError(ex.InnerException);
                }
                return InternalServerError(ex);
            }
        }

        [Route("api/Obra/BuscarObrasFiltradas")]
        [System.Web.Http.ActionName("BuscarObrasFiltradas")]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> BuscarObrasFiltradas(string nombreObra, int? selectDepartamento, int? selectOrganismo)
        {
            List<ObraGrilla> listaObra = new List<ObraGrilla>();
            try
            {
                using (var context = new MySqlDbContext())
                {
                    IQueryable<vw_looker_obras> tmp = context.vw_looker_obras.Where(x=>x.IdEstado == 1);
                    if (!(string.IsNullOrEmpty(nombreObra)))
                    {
                        tmp = tmp.Where(x=>x.Nombre.Contains(nombreObra));
                    }
                    if (selectDepartamento.HasValue && selectDepartamento.Value != 0)
                    {
                        tmp = tmp.Where(x => x.IdDepartamento == selectDepartamento.Value);
                    }

                    if (selectOrganismo.HasValue && selectOrganismo.Value != 0)
                    {
                        tmp = tmp.Where(x => x.OrganismoId == selectOrganismo.Value);
                    }

                    var obrasFiltradas = await tmp.ToListAsync();

                    listaObra = obrasFiltradas.Select(x => new ObraGrilla
                    {
                        IdObra = x.PryProyecto_Id,
                        Nombre = x.Nombre,
                        Estado = x.Estado,
                        Dependencia = x.Dependencia,
                        Departamento = x.Departamento,
                        Contrato = x.MontoContratado,
                        TotalPagado = x.OB_MontoPagado,
                        Avance = x.OB_AvanceReal,
                        Inicio = x.FechaInicio,
                        Fin = x.FechaFin
                    }).ToList();

                    return Ok(listaObra);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return InternalServerError(ex.InnerException);
                }
                return InternalServerError(ex);
            }
        }
        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }
    }
}