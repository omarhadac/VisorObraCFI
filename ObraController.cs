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
                        .Where(x => x.IdEstado == 1 && (x.OrganismoId == 2 || x.OrganismoId == 4
                            || x.OrganismoId == 9 || x.OrganismoId == 14 || x.OrganismoId == 20))
                        .Distinct().ToListAsync();

                    var listaLicitacion = await context.vw_looker_obras
                        .Where(x => x.PryStage_Id == 49 && (x.IdEstado == 6 || x.IdEstado == 7 || x.IdEstado == 8 || x.IdEstado == 14 || x.IdEstado == 15) && (x.OrganismoId == 2
                            || x.OrganismoId == 4 || x.OrganismoId == 9 || x.OrganismoId == 14 || x.OrganismoId == 20))
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
                        .Where(x => x.IdEstado == 1 && (x.OrganismoId == 2 || x.OrganismoId == 4
                            || x.OrganismoId == 9 || x.OrganismoId == 14 || x.OrganismoId == 20))
                        .Distinct().ToListAsync();

                    var listaLicitacion = await context.vw_looker_obras
                        .Where(x => x.PryStage_Id == 49 && (x.IdEstado == 6 || x.IdEstado == 7 || x.IdEstado == 8 || x.IdEstado == 14 || x.IdEstado == 15) && (x.OrganismoId == 2
                            || x.OrganismoId == 4 || x.OrganismoId == 9 || x.OrganismoId == 14 || x.OrganismoId == 20))
                        .Distinct().ToListAsync();

                    var lista = new List<ContadorObra>();

                    var listaEjecucionAySAM = await context.vw_looker_obras
                        .Where(x => x.IdEstado == 1 && x.OrganismoId == 14)
                        .Distinct().ToListAsync();
                    var listaLicitacionAySAM = listaLicitacion.Where(x => x.OrganismoId == 14).Distinct().ToList();

                    var listaEjecucionIPV = await context.vw_looker_obras
                        .Where(x => x.IdEstado == 1 && x.OrganismoId == 4)
                        .Distinct().ToListAsync();
                    var listaLicitacionIPV = listaLicitacion.Where(x => x.OrganismoId == 4).Distinct().ToList();

                    var listaEjecucionVialidad = await context.vw_looker_obras.Where(x => x.IdEstado == 1 && x.OrganismoId == 9)
                        .Distinct().ToListAsync();
                    var listaLicitacionVialidad = listaLicitacion.Where(x => x.OrganismoId == 9).Distinct().ToList();

                    var listaEjecucionInfra = await context.vw_looker_obras.Where(x => x.IdEstado == 1 && x.OrganismoId == 2)
                        .Distinct().ToListAsync();
                    var listaLicitacionInfra = listaLicitacion.Where(x => x.OrganismoId == 2).Distinct().ToList();

                    var listaEjecucionIrrigacion = await context.vw_looker_obras.Where(x => x.IdEstado == 1 && x.OrganismoId == 20)
                            .Distinct().ToListAsync();
                    var listaLicitacionIrrigacion = listaLicitacion.Where(x => x.OrganismoId == 20).Distinct().ToList();

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

                    var unItemIrrig = new ContadorObra();
                    unItemIrrig.CantidadObraEjecucion = listaEjecucionIrrigacion.Count;
                    unItemIrrig.MontoObraEjecucion = Convert.ToInt64(listaEjecucionIrrigacion.Sum(x => (decimal?)x.MontoContratado) ?? 0);
                    unItemIrrig.CantidadObraLicitacion = listaLicitacionIrrigacion.Count;
                    unItemIrrig.MontoObraLicitacion = Convert.ToInt64(listaLicitacionIrrigacion.Sum(x => (decimal?)x.MontoContratado) ?? 0);
                    unItemIrrig.IdOrganismo = 20;
                    unItemIrrig.Organismo = "Irrigación";
                    lista.Add(unItemIrrig);

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
        public async Task<IHttpActionResult> BuscarObrasFiltradas(string nombreObra, int? selectDepartamento,
            int? selectOrganismo, int? selectEstado, int page = 1, int pageSize = 10)
        {
            List<ObraGrilla> listaObra = new List<ObraGrilla>();
            try
            {
                using (var context = new MySqlDbContext())
                {
                    context.Database.CommandTimeout = 360;
                    IQueryable<vw_looker_obras> tmp = new List<vw_looker_obras>().AsQueryable();
                    if (selectEstado == 1)
                    {
                        tmp = context.vw_looker_obras.Where(x => x.IdEstado == 1);
                    }
                    else
                    {
                        tmp = context.vw_looker_obras.Where(x => x.PryStage_Id == 49 && (x.IdEstado == 6 || x.IdEstado == 7 || x.IdEstado == 8 || x.IdEstado == 14 || x.IdEstado == 15));
                    }

                    if (!string.IsNullOrEmpty(nombreObra))
                    {
                        tmp = tmp.Where(x => x.Nombre.Contains(nombreObra));
                    }

                    if (selectDepartamento.HasValue && selectDepartamento.Value != 0)
                    {
                        tmp = tmp.Where(x => x.IdDepartamento == selectDepartamento.Value);
                    }

                    if (selectOrganismo.HasValue && selectOrganismo.Value != 0)
                    {
                        tmp = tmp.Where(x => x.OrganismoId == selectOrganismo.Value);
                    }

                    //var query = from obra in tmp
                    //            join licitacion in context.LicProyectoFecha
                    //                .GroupBy(l => l.idProyecto)
                    //                .Select(g => g.OrderByDescending(l => l.idLicProyectoFecha).FirstOrDefault())
                    //                on obra.PryProyecto_Id equals licitacion.idProyecto into obraLicitacion
                    //            from licitacion in obraLicitacion.DefaultIfEmpty()
                    //            join proyecto in context.PryProyecto
                    //                on obra.PryProyecto_Id equals proyecto.Id
                    //            join totales in context.vw_looker_obras_totales
                    //                on obra.PryProyecto_Id equals totales.PryProyecto_Id
                    //            select new { obra, licitacion, proyecto, totales };

                    var query = from obra in tmp
                                join licitacion in context.LicProyectoFecha
                                    .GroupBy(l => l.idProyecto)
                                    .Select(g => g.OrderByDescending(l => l.idLicProyectoFecha).FirstOrDefault())
                                    on obra.PryProyecto_Id equals licitacion.idProyecto into obraLicitacion
                                from licitacion in obraLicitacion.DefaultIfEmpty()
                                join proyecto in context.PryProyecto
                                    on obra.PryProyecto_Id equals proyecto.Id
                                select new { obra, licitacion, proyecto };

                    var totalItems = await query.CountAsync();
                    var obrasFiltradas = await query
                        .OrderBy(x => x.obra.PryProyecto_Id)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();

                    listaObra = obrasFiltradas.Select(x => new ObraGrilla
                    {
                        IdObra = x.obra.PryProyecto_Id,
                        Nombre = x.obra.Nombre,
                        Estado = x.obra.Estado,
                        idEstado = x.obra.IdEstado,
                        Dependencia = x.obra.Dependencia,
                        Departamento = x.obra.departamentoString,
                        Contrato = x.obra.MontoContratado,
                        TotalPagado = x.obra.OB_MontoPagado,
                        //TotalAlteraciones = x.totales.TotalAlteraciones,
                        //TotalVariaciones = x.totales.TotalVariaciones,
                        MontoContratado = x.obra.MontoContratado,
                        Empresa = x.obra.Empresa,
                        Avance = x.obra.OB_AcumuladoMensual,
                        MontoAdicional = x.obra.MontoAdicional,
                        MontoContratacionDirecta = x.obra.MontoContratacionDirecta,
                        OB_VarPrecio = x.obra.OB_VarPrecio,
                        Inicio = x.obra.FechaInicio,
                        Apertura = x.licitacion?.fechaApertura,
                        Publicacion = x.licitacion?.fechaPublicacion,
                        Fin = x.obra.FechaFinActualizada ?? x.obra.FechaFin,
                        Financiamiento = x.proyecto.PryFinanciacion_Id
                    }).ToList();

                    var result = new
                    {
                        TotalItems = totalItems,
                        Items = listaObra
                    };

                    return Ok(result);
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

        [Route("api/Obra/BuscarObrasMapa")]
        [System.Web.Http.ActionName("BuscarObrasMapa")]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> BuscarObrasMapa(string nombreObra, int? selectDepartamento, int? selectOrganismo, int? selectEstado)
        {
            List<ObraMapa> listaObra = new List<ObraMapa>();
            try
            {
                using (var context = new MySqlDbContext())
                {
                    context.Database.CommandTimeout = 360;
                    IQueryable<vw_looker_obras> tmp = context.vw_looker_obras
                        .Where(x => x.Latitud.HasValue && x.Longitud.HasValue);

                    if (selectEstado == 1)
                    {
                        tmp = context.vw_looker_obras.Where(x => x.IdEstado == 1);
                    }
                    else
                    {
                        tmp = context.vw_looker_obras.Where(x => x.PryStage_Id == 49 && (x.IdEstado == 6 || x.IdEstado == 7 || x.IdEstado == 8 || x.IdEstado == 14 || x.IdEstado == 15));
                    }
                    if (!(string.IsNullOrEmpty(nombreObra)))
                    {
                        tmp = tmp.Where(x => x.Nombre.Contains(nombreObra));
                    }
                    if (selectDepartamento.HasValue && selectDepartamento.Value != 0)
                    {
                        tmp = tmp.Where(x => x.IdDepartamento == selectDepartamento.Value);
                    }

                    if (selectOrganismo.HasValue && selectOrganismo.Value != 0)
                    {
                        tmp = tmp.Where(x => x.OrganismoId == selectOrganismo.Value);
                    }

                    // Realiza el join con PryProyecto
                    //var query = from obra in tmp
                    //            join proyecto in context.PryProyecto
                    //                on obra.PryProyecto_Id equals proyecto.Id
                    //            join totales in context.vw_looker_obras_totales
                    //                on obra.PryProyecto_Id equals totales.PryProyecto_Id
                    //            select new { obra, proyecto, totales };

                    var query = from obra in tmp
                                join licitacion in context.LicProyectoFecha
                                    .GroupBy(l => l.idProyecto)
                                    .Select(g => g.OrderByDescending(l => l.idLicProyectoFecha).FirstOrDefault())
                                    on obra.PryProyecto_Id equals licitacion.idProyecto into obraLicitacion
                                from licitacion in obraLicitacion.DefaultIfEmpty()
                                join proyecto in context.PryProyecto
                                    on obra.PryProyecto_Id equals proyecto.Id
                                select new { obra, licitacion, proyecto };

                    var obrasFiltradas = await query.ToListAsync();

                    listaObra = obrasFiltradas.Select(x => {
                        // Traer los avances a memoria
                        var avances = context.PryAvance
                            .Where(a => a.PryProyectoPlanificacion_Id == x.obra.PryProyectoPlanificacion_Id && a.Eliminado == false
                                && a.PryCertificacionesMensuales_Id != null && a.AvanceReal > 0)
                            .ToList()
                            .GroupBy(a => a.Mes)
                            .Select(g => g.OrderByDescending(a => a.AvanceReal ?? 0).FirstOrDefault())
                            .OrderBy(a => a.Mes)
                            .Select(a => new AvanceGrafico
                            {
                                Mes = a.Mes,
                                AvanceReal = a.AvanceReal,
                                AvanceTeorico = a.AvanceTeorico
                            })
                            .ToList();

                        return new ObraMapa
                        {
                            IdObra = x.obra.PryProyecto_Id,
                            Nombre = x.obra.Nombre,
                            Estado = x.obra.Estado,
                            Dependencia = x.obra.Dependencia,
                            Departamento = x.obra.Departamento,
                            idEstado = x.obra.IdEstado,
                            IdOrganismo = x.obra.OrganismoId,
                            Contrato = x.obra.MontoContratado,
                            TotalPagado = x.obra.OB_MontoPagado,
                            Avance = x.obra.OB_AcumuladoMensual,
                            Inicio = x.obra.FechaInicio,
                            Apertura = x.licitacion?.fechaApertura,
                            Publicacion = x.licitacion?.fechaPublicacion,
                            Financiamiento = x.proyecto.PryFinanciacion_Id,
                            Fin = x.obra.FechaFinActualizada ?? x.obra.FechaFin,
                            Latitud = x.obra.Latitud,
                            Longitud = x.obra.Longitud,
                            Direccion = x.obra.Domicilio,
                            MontoAdicional = x.obra.MontoAdicional,
                            MontoContratacionDirecta = x.obra.MontoContratacionDirecta,
                            OB_VarPrecio = x.obra.OB_VarPrecio,
                            Empresa = x.obra.Empresa ?? "",
                            //TotalAlteraciones = x.totales.TotalAlteraciones,
                            //TotalVariaciones = x.totales.TotalVariaciones,
                            MontoContratado = x.obra.MontoContratado,
                            MesCertificado = x.obra.OB_UltMesCert.HasValue
                                ? x.obra.OB_UltMesCert.Value.ToString("MMMM yyyy", new System.Globalization.CultureInfo("es-ES"))
                                : string.Empty,
                            ListaArchivos = context.PryArchivosObra
                                .Where(a => a.IdProyecto == x.obra.PryProyecto_Id && a.IdTipoArchivo == 1 && a.Estado == 1)
                                .OrderByDescending(a => a.FechaCarga)
                                .Take(5)
                                .ToList()
                                .Select(a => new ArchivoObraInfo
                                {
                                    Url = a.Url,
                                    FechaImagen = a.FechaImagen.HasValue ? a.FechaImagen.Value.ToString("dd/MM/yyyy") : "No hay fecha"
                                })
                                .ToList(),
                            ChartData = avances
                        };
                    }).ToList();


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
            return Ok(listaObra);
        }

        [Route("api/Obra/ExportarObrasFiltradas")]
        [System.Web.Http.ActionName("ExportarObrasFiltradas")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> ExportarObrasFiltradas(string nombreObra, int? selectDepartamento, int? selectOrganismo, int? selectEstado)
        {
            try
            {
                using (var context = new MySqlDbContext())
                {
                    context.Database.CommandTimeout = 360;
                    IQueryable<vw_looker_obras> tmp = new List<vw_looker_obras>().AsQueryable();
                    if (selectEstado == 1)
                    {
                        tmp = context.vw_looker_obras.Where(x => x.IdEstado == 1);
                    }
                    else
                    {
                        tmp = context.vw_looker_obras.Where(x => x.PryStage_Id == 49 && (x.IdEstado == 6 || x.IdEstado == 7 || x.IdEstado == 8 || x.IdEstado == 14 || x.IdEstado == 15));
                    }

                    if (!string.IsNullOrEmpty(nombreObra))
                    {
                        tmp = tmp.Where(x => x.Nombre.Contains(nombreObra));
                    }

                    if (selectDepartamento.HasValue && selectDepartamento.Value != 0)
                    {
                        tmp = tmp.Where(x => x.IdDepartamento == selectDepartamento.Value);
                    }

                    if (selectOrganismo.HasValue && selectOrganismo.Value != 0)
                    {
                        tmp = tmp.Where(x => x.OrganismoId == selectOrganismo.Value);
                    }

                    var query = from obra in tmp
                                join licitacion in context.LicProyectoFecha
                                    .GroupBy(l => l.idProyecto)
                                    .Select(g => g.OrderByDescending(l => l.idLicProyectoFecha).FirstOrDefault())
                                    on obra.PryProyecto_Id equals licitacion.idProyecto into obraLicitacion
                                from licitacion in obraLicitacion.DefaultIfEmpty()
                                select new { obra, licitacion };

                    var obrasFiltradas = await query
                        .OrderBy(x => x.obra.PryProyecto_Id)
                        .ToListAsync();

                    var listaObra = obrasFiltradas.Select(x => new ObraGrilla
                    {
                        IdObra = x.obra.PryProyecto_Id,
                        Nombre = x.obra.Nombre,
                        Estado = x.obra.Estado,
                        Dependencia = x.obra.Dependencia,
                        Departamento = x.obra.Departamento,
                        Contrato = x.obra.MontoContratado,
                        TotalPagado = x.obra.OB_MontoPagado,
                        MontoAdicional = x.obra.MontoAdicional,
                        MontoContratacionDirecta = x.obra.MontoContratacionDirecta,
                        OB_VarPrecio = x.obra.OB_VarPrecio,
                        Empresa = x.obra.Empresa,
                        Avance = x.obra.OB_AcumuladoMensual,
                        Inicio = x.obra.FechaInicio,
                        Apertura = x.licitacion?.fechaApertura,
                        Publicacion = x.licitacion?.fechaPublicacion,
                        Fin = x.obra.FechaFinActualizada ?? x.obra.FechaFin,
                    }).ToList();

                    // Establecer la propiedad LicenseContext
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    using (var package = new ExcelPackage())
                    {
                        var worksheet = package.Workbook.Worksheets.Add("Obras Filtradas");
                        worksheet.Cells["A1"].Value = "ID Obra";
                        worksheet.Cells["B1"].Value = "Nombre";
                        worksheet.Cells["C1"].Value = "Estado";
                        worksheet.Cells["D1"].Value = "Dependencia";
                        worksheet.Cells["E1"].Value = "Departamento";
                        worksheet.Cells["F1"].Value = "Contrato";
                        worksheet.Cells["G1"].Value = "Financiamiento";
                        worksheet.Cells["H1"].Value = "Empresa";
                        worksheet.Cells["I1"].Value = "Avance";
                        worksheet.Cells["J1"].Value = "Inicio";
                        worksheet.Cells["K1"].Value = "Apertura";
                        worksheet.Cells["L1"].Value = "Publicación";
                        worksheet.Cells["M1"].Value = "Fin";

                        for (int i = 0; i < listaObra.Count; i++)
                        {
                            var obra = listaObra[i];
                            worksheet.Cells[i + 2, 1].Value = obra.IdObra;
                            worksheet.Cells[i + 2, 2].Value = obra.Nombre;
                            worksheet.Cells[i + 2, 3].Value = obra.Estado;
                            worksheet.Cells[i + 2, 4].Value = obra.Dependencia;
                            worksheet.Cells[i + 2, 5].Value = obra.Departamento;
                            worksheet.Cells[i + 2, 6].Value = obra.ContratoFormatted;
                            worksheet.Cells[i + 2, 7].Value = obra.EsResarcimiento;
                            worksheet.Cells[i + 2, 8].Value = obra.Empresa;
                            worksheet.Cells[i + 2, 9].Value = obra.Avance;
                            worksheet.Cells[i + 2, 10].Value = obra.Inicio?.ToString("dd/MM/yyyy");
                            worksheet.Cells[i + 2, 11].Value = obra.Apertura?.ToString("dd/MM/yyyy");
                            worksheet.Cells[i + 2, 12].Value = obra.Publicacion?.ToString("dd/MM/yyyy");
                            worksheet.Cells[i + 2, 13].Value = obra.Fin?.ToString("dd/MM/yyyy");
                        }

                        var stream = new MemoryStream(package.GetAsByteArray());
                        var result = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new ByteArrayContent(stream.ToArray())
                        };
                        result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                        {
                            FileName = "ObrasFiltradas.xlsx"
                        };
                        result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}