using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using OfficeOpenXml;
using VisorObraCFI.DTO;
using VisorObraCFI.Models;

namespace VisorObraCFI
{
    public class MapaMendozaController : ApiController
    {

        [Route("api/MapaMendoza/BuscarDistritos")]
        [System.Web.Http.ActionName("BuscarDistritos")]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> BuscarDistritos()
        {
            try
            {
                using (var context = new MySqlDbContext())
                {
                    var fechaLimite = new DateTime(2024, 1, 1);

                    var listaObras = await context.vw_looker_obras
                        .Distinct().ToListAsync();

                    var listaLicitacion = await context.vw_looker_obras
                        .Where(x => x.PryStage_Id == 49)
                        .Distinct().ToListAsync();
                    List<DetalleDistrito> listaDetalle = new List<DetalleDistrito>();
                    foreach (var item in listaObras)
                    {
                        DetalleDistrito detalleDistrito = new DetalleDistrito();
                        if((item.IdDepartamento == 1)|| (item.IdDepartamento == 3)|| (item.IdDepartamento == 7)|| (item.IdDepartamento == 8))
                        {
                            detalleDistrito.NombreDistrito= "Primer Distrito";
                            detalleDistrito.NroDistrito = 1;
                        }
                        if ((item.IdDepartamento == 5) || (item.IdDepartamento == 6) || (item.IdDepartamento == 10) || (item.IdDepartamento == 12) || (item.IdDepartamento == 14) || (item.IdDepartamento == 16))
                        {
                            detalleDistrito.NombreDistrito = "Segundo Distrito";
                            detalleDistrito.NroDistrito = 2;
                        }
                        if ((item.IdDepartamento == 2) || (item.IdDepartamento == 9) || (item.IdDepartamento == 13) || (item.IdDepartamento == 17) || (item.IdDepartamento == 18))
                        {
                            detalleDistrito.NombreDistrito = "Tercer Distrito";
                            detalleDistrito.NroDistrito = 3;
                        }
                        if ((item.IdDepartamento == 4) || (item.IdDepartamento == 11) || (item.IdDepartamento == 15))
                        {
                            detalleDistrito.NombreDistrito = "Cuarto Distrito";
                            detalleDistrito.NroDistrito = 4;
                        }
                        if ((item.IdEstado == 1) && (item.esEntregada == false))
                        {
                            if (detalleDistrito.ObrasEjecucion.HasValue)
                            {
                                detalleDistrito.ObrasEjecucion = detalleDistrito.ObrasEjecucion + 1;
                            }
                            else
                            {
                                detalleDistrito.ObrasEjecucion = 1;
                            }
                        }
                        else
                        {
                            if ((item.PryStage_Id == 49) && (item.IdEstado == 6))
                            {
                                if (detalleDistrito.ObrasLicitacion.HasValue)
                                {
                                    detalleDistrito.ObrasLicitacion = detalleDistrito.ObrasLicitacion + 1;
                                }
                                else
                                {
                                    detalleDistrito.ObrasLicitacion = 1;
                                }
                            }
                            else
                            {
                                if ((item.IdEstado == 3) && (item.FechaFin > fechaLimite))
                                {
                                    if (detalleDistrito.ObrasEntregadas.HasValue)
                                    {
                                        detalleDistrito.ObrasEntregadas = detalleDistrito.ObrasEntregadas + 1;
                                    }
                                    else
                                    {
                                        detalleDistrito.ObrasEntregadas = 1;
                                    }
                                }
                                else
                                {
                                    if ((item.IdEstado == 11) && (item.FechaFin > fechaLimite))
                                    {
                                        if (detalleDistrito.ObrasParalizadas.HasValue)
                                        {
                                            detalleDistrito.ObrasParalizadas = detalleDistrito.ObrasParalizadas + 1;
                                        }
                                        else
                                        {
                                            detalleDistrito.ObrasParalizadas = 1;
                                        }
                                    }
                                }
                            }
                        }
                        if(detalleDistrito.NombreDistrito != null)
                        {
                            listaDetalle.Add(detalleDistrito);
                        } 
                    }
                    var listaAgrupada = listaDetalle
                        .GroupBy(x => x.NroDistrito)
                        .Select(g => new DetalleDistrito
                        {
                            NombreDistrito = g.First().NombreDistrito,
                            NroDistrito = g.Key,
                            ObrasEjecucion = g.Sum(x => x.ObrasEjecucion) ?? 0,
                            ObrasLicitacion = g.Sum(x => x.ObrasLicitacion) ?? 0,
                            ObrasEntregadas = g.Sum(x => x.ObrasEntregadas) ?? 0,
                            ObrasParalizadas = g.Sum(x => x.ObrasParalizadas) ?? 0
                        })
                        .ToList();
                    return Ok(listaAgrupada.OrderBy(x=>x.NroDistrito));
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}