using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Windows.Media;
using OfficeOpenXml;
using VisorObraCFI.DTO;
using VisorObraCFI.Models;


namespace VisorObraCFI
{
    public class InformeController : ApiController
    {

        [Route("api/Informe/BuscarDistritos")]
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
                        if ((item.IdDepartamento == 1) || (item.IdDepartamento == 3) || (item.IdDepartamento == 7) || (item.IdDepartamento == 8))
                        {
                            detalleDistrito.NombreDistrito = "Primer Distrito";
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
                        if (detalleDistrito.NombreDistrito != null)
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
                    return Ok(listaAgrupada.OrderBy(x => x.NroDistrito));
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/Informe/BuscarTotal")]
        [System.Web.Http.ActionName("BuscarTotal")]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> BuscarTotal()
        {
            try
            {
                using (var context = new MySqlDbContext())
                {
                    var fechaLimite = new DateTime(2024, 1, 1);

                    var listaObras = await context.vw_looker_obras
                        .Distinct().ToListAsync();

                    int totalEjecucion = 0;
                    int totalLicitacion = 0;
                    int totalEntregadas = 0;
                    int totalParalizadas = 0;

                    foreach (var item in listaObras)
                    {
                        if ((item.IdEstado == 1) && (item.esEntregada == false))
                        {
                            totalEjecucion++;
                        }
                        else if ((item.PryStage_Id == 49) && (item.IdEstado == 6))
                        {
                            totalLicitacion++;
                        }
                        else if ((item.IdEstado == 3) && (item.FechaFin > fechaLimite))
                        {
                            totalEntregadas++;
                        }
                        else if ((item.IdEstado == 11) && (item.FechaFin > fechaLimite))
                        {
                            totalParalizadas++;
                        }
                    }

                    var resultado = new
                    {
                        ObrasEjecucion = totalEjecucion,
                        ObrasLicitacion = totalLicitacion,
                        ObrasEntregadas = totalEntregadas,
                        ObrasParalizadas = totalParalizadas
                    };

                    return Ok(resultado);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/Informe/ExportarTotalExcel2")]
        [System.Web.Http.ActionName("ExportarTotalExcel2")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> ExportarTotalExcel2()
        {
            try
            {
                using (var context = new MySqlDbContext())
                {
                    var fechaLimite = new DateTime(2024, 1, 1);

                    var listaObras = await context.vw_looker_obras
                        .Distinct().ToListAsync();

                    int totalEjecucion = 0;
                    int totalLicitacion = 0;
                    int totalEntregadas = 0;
                    int totalParalizadas = 0;

                    foreach (var item in listaObras)
                    {
                        if ((item.IdEstado == 1) && (item.esEntregada == false))
                        {
                            totalEjecucion++;
                        }
                        else if ((item.PryStage_Id == 49) && (item.IdEstado == 6))
                        {
                            totalLicitacion++;
                        }
                        else if ((item.IdEstado == 3) && (item.FechaFin > fechaLimite))
                        {
                            totalEntregadas++;
                        }
                        else if ((item.IdEstado == 11) && (item.FechaFin > fechaLimite))
                        {
                            totalParalizadas++;
                        }
                    }

                    int totalObras = totalEjecucion + totalLicitacion + totalEntregadas + totalParalizadas;

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage())
                    {
                        // Hoja principal
                        var ws = package.Workbook.Worksheets.Add("Informe Total Obras");

                        // Hoja de datos para el gráfico
                        var wsDatos = package.Workbook.Worksheets.Add("datosGrafico");
                        string[] titulos = { "En Licitación", "En Ejecución", "Entregadas", "Paralizadas" };
                        int[] valores = { totalLicitacion, totalEjecucion, totalEntregadas, totalParalizadas };
                        string[] colores = { "#007bff", "#003366", "#bfa16c", "#d3d3d3" }; // azul, azul oscuro, marrón claro, gris claro

                        wsDatos.Cells["A2"].Value = "Tipo";
                        wsDatos.Cells["B2"].Value = "Cantidad";
                        for (int i = 0; i < 4; i++)
                        {
                            wsDatos.Cells[2 + i, 1].Value = titulos[i];
                            wsDatos.Cells[2 + i, 2].Value = valores[i];
                        }

                        // Título superior: CONSOLIDADO - OBRAS GESTIONADAS AL X
                        var fechaHoy = DateTime.Now;
                        string[] meses = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
                        string fechaFormateada = $"{fechaHoy.Day} de {meses[fechaHoy.Month - 1]} de {fechaHoy.Year}";

                        ws.Cells["A1:L1"].Merge = true;
                        ws.Cells["A1"].Value = $"CONSOLIDADO - OBRAS GESTIONADAS AL {fechaFormateada}";
                        ws.Cells["A1"].Style.Font.Size = 16;
                        ws.Cells["A1"].Style.Font.Bold = true;
                        ws.Cells["A1"].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#003366")); // Azul
                        ws.Cells["A1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;
                        ws.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Row(1).Height = 30;

                        // 1. Título arriba (una sola fila)
                        ws.Cells["A2:L2"].Merge = true;
                        ws.Cells["A2"].Value = $"TOTAL DE OBRAS: {totalObras}";
                        ws.Cells["A2"].Style.Font.Size = 18;
                        ws.Cells["A2"].Style.Font.Bold = true;
                        ws.Cells["A2"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells["A2"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                        ws.Cells["A2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Row(1).Height = 28;

                        // 2. Gráfico de torta (izquierda, columnas A-B)
                        var chart = ws.Drawings.AddChart("graficoTorta", OfficeOpenXml.Drawing.Chart.eChartType.Pie);
                        //chart.Title.Text = "Distribución de Obras";
                        chart.SetPosition(3, 0, 5, 0); // Fila 3, Columna A
                        chart.SetSize(300, 350);
                        var serie = chart.Series.Add("datosGrafico!B2:B5", "datosGrafico!A2:A5");
                        serie.Header = "Cantidad";
                        

                        // 3. Cuadros de datos (derecha, columnas E-H)
                        int colBase = 1; // Columna A
                        int rowBase = 4; // Fila inicial para cuadros

                        for (int i = 0; i < 4; i++)
                        {
                            int row = rowBase + (i % 2) * 6; // dos arriba, dos abajo
                            int col = colBase + (i / 2) * 2; // dos columnas

                            // Encabezado
                            var headerCell = ws.Cells[row, col, row, col + 1];
                            headerCell.Merge = true;
                            headerCell.Value = titulos[i];
                            headerCell.Style.Font.Bold = true;
                            headerCell.Style.Font.Size = 14; // Más grande
                            headerCell.Style.Font.Color.SetColor(System.Drawing.Color.White);
                            headerCell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            headerCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#003366")); // Azul más oscuro
                            headerCell.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick, System.Drawing.ColorTranslator.FromHtml("#003366"));
                            headerCell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Row(row).Height = 28;

                            // Valor
                            var valueCell = ws.Cells[row + 1, col, row + 3, col + 1];
                            valueCell.Merge = true;
                            valueCell.Value = valores[i];
                            valueCell.Style.Font.Size = 16; // Más chico
                            valueCell.Style.Font.Bold = true;
                            valueCell.Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#003366")); // Azul
                            valueCell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            valueCell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            valueCell.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick, System.Drawing.ColorTranslator.FromHtml("#003366"));
                        }

                        //ws.Column(1).Width = 7;
                        //ws.Column(2).Width = 7;
                        //for (int i = 5; i <= 8; i++)
                        //{
                        //    ws.Column(i).Width = 7;
                        //}
                        for (int i = 1; i <= 12; i++)
                        {
                            ws.Column(i).Width = 7;
                        }

                        var stream = new MemoryStream(package.GetAsByteArray());
                        var result = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new ByteArrayContent(stream.ToArray())
                        };
                        result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                        {
                            FileName = "InformeTotalObras.xlsx"
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

        [Route("api/Informe/ExportarTotalExcel")]
        [System.Web.Http.ActionName("ExportarTotalExcel")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> ExportarTotalExcel()
        {
            try
            {
                using (var context = new MySqlDbContext())
                {
                    #region BaseDatos
                    var fechaLimite = new DateTime(2024, 1, 1);

                    var listaObras = await context.vw_looker_obras
                        .Distinct().ToListAsync();

                    int totalEjecucion = 0;
                    int totalLicitacion = 0;
                    int totalEntregadas = 0;
                    int totalParalizadas = 0;

                    foreach (var item in listaObras)
                    {
                        if ((item.IdEstado == 1) && (item.esEntregada == false))
                        {
                            totalEjecucion++;
                        }
                        else if ((item.PryStage_Id == 49) && (item.IdEstado == 6))
                        {
                            totalLicitacion++;
                        }
                        else if ((item.IdEstado == 3) && (item.FechaFin > fechaLimite))
                        {
                            totalEntregadas++;
                        }
                        else if ((item.IdEstado == 11) && (item.FechaFin > fechaLimite))
                        {
                            totalParalizadas++;
                        }
                    }

                    int totalObras = totalEjecucion + totalLicitacion + totalEntregadas + totalParalizadas;

                    #endregion

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage())
                    {
                        // Hoja principal
                        var ws = package.Workbook.Worksheets.Add("Informe Total Obras");

                       
                        // Fondo azul en columnas A a I (filas 1 a 20 por ejemplo)
                        var azul = System.Drawing.ColorTranslator.FromHtml("#003366");
                        for (int fila = 1; fila <= 41; fila++)
                        {
                            for (int col = 1; col <= 9; col++)
                            {
                                ws.Cells[fila, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Cells[fila, col].Style.Fill.BackgroundColor.SetColor(azul);
                            }
                        }
                        // 1. Fondo blanco en columnas J a L (filas 1 a 20)
                        var blanco = System.Drawing.Color.White;
                        for (int fila = 1; fila <= 41; fila++)
                        {
                            for (int col = 10; col <= 12; col++) // J=10, K=11, L=12
                            {
                                ws.Cells[fila, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Cells[fila, col].Style.Fill.BackgroundColor.SetColor(blanco);
                            }
                        }

                        #region PrimeraHoja
                        // Título en fila 4
                        ws.Cells["A7:I7"].Merge = true;
                        ws.Cells["A7"].Value = "Informe Ejecutivo de obras";
                        ws.Cells["A7"].Style.Font.Size = 28;
                        ws.Cells["A7"].Style.Font.Bold = true;
                        ws.Cells["A7"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        ws.Cells["A7"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Row(4).Height = 40;

                        // Fecha en fila 7
                        string[] meses = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
                        var fechaHoy = DateTime.Now;
                        string fechaFormateada = $"{fechaHoy.Day} de {meses[fechaHoy.Month - 1]} de {fechaHoy.Year}";
                        ws.Cells["A16:I16"].Merge = true;
                        ws.Cells["A16"].Value = $"01 de Enero de 2024 al {fechaFormateada}";
                        ws.Cells["A16"].Style.Font.Size = 16;
                        ws.Cells["A16"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        ws.Cells["A16"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Row(7).Height = 28;

                        // Texto en fila 11, columnas A a I
                        ws.Cells["A33:I33"].Merge = true;
                        ws.Cells["A33"].Value = "Infraestructura - IPV - AySAM - DGI";
                        ws.Cells["A33"].Style.Font.Size = 16;
                        ws.Cells["A33"].Style.Font.Bold = true;
                        ws.Cells["A33"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        ws.Cells["A33"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Row(11).Height = 28;

                        // 2. Insertar imagen en J1
                        string rutaImagen = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Iconos/LogoEscudo.png");
                        // O usa una ruta absoluta si la imagen está en otro lugar
                        if (File.Exists(rutaImagen))
                        {
                            var imagen = Image.FromFile(rutaImagen);
                            var picture = ws.Drawings.AddPicture("Imagen1", rutaImagen);
                            //picture.SetPosition(10, 0, 9, 0); // Fila 1 (índice 0), Columna J (índice 9)
                            picture.SetPosition(10, 0, 9, 1); // Fila 1, columna J, 1px de margen izquierdo

                            picture.SetSize(147, 440);
                        }

                        #endregion

                        #region SegundaHoja

                        for (int fila = 42; fila <= 43; fila++)
                        {
                            for (int col = 1; col <= 12; col++)
                            {
                                ws.Cells[fila, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Cells[fila, col].Style.Fill.BackgroundColor.SetColor(azul);
                            }
                        }
                        // 2. Insertar imagen en J1
                        string rutaImagen2 = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Iconos/LogoMzaReducido1.png");
                        // O usa una ruta absoluta si la imagen está en otro lugar
                        if (File.Exists(rutaImagen2))
                        {
                            var imagen1 = Image.FromFile(rutaImagen2);
                            var picture1 = ws.Drawings.AddPicture("Imagen2", rutaImagen2);
                            //picture.SetPosition(10, 0, 9, 0); // Fila 1 (índice 0), Columna J (índice 9)
                            picture1.SetPosition(41, 3, 10, 1); // Fila 1, columna J, 1px de margen izquierdo, 2px margen superior

                            picture1.SetSize(84, 30);
                        }
                        // Hoja de datos para el gráfico
                        var wsDatos = package.Workbook.Worksheets.Add("datosGrafico");
                        string[] titulos = { "En Licitación", "En Ejecución", "Entregadas", "Paralizadas" };
                        int[] valores = { totalLicitacion, totalEjecucion, totalEntregadas, totalParalizadas };
                        string[] colores = { "#007bff", "#003366", "#bfa16c", "#d3d3d3" }; // azul, azul oscuro, marrón claro, gris claro

                        wsDatos.Cells["A2"].Value = "Tipo";
                        wsDatos.Cells["B2"].Value = "Cantidad";
                        for (int i = 0; i < 4; i++)
                        {
                            wsDatos.Cells[2 + i, 1].Value = titulos[i];
                            wsDatos.Cells[2 + i, 2].Value = valores[i];
                        }

                        
                        ws.Row(41).PageBreak = true; // Salto de página después de la fila 42
                        
                        // Título superior: CONSOLIDADO - OBRAS GESTIONADAS AL X
                        ws.Cells["A45:L45"].Merge = true;
                        ws.Cells["A45"].Value = $"CONSOLIDADO - OBRAS GESTIONADAS AL {fechaFormateada}";
                        ws.Cells["A45"].Style.Font.Size = 16;
                        ws.Cells["A45"].Style.Font.Bold = true;
                        ws.Cells["A45"].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#003366")); // Azul
                        ws.Cells["A45"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;
                        ws.Cells["A45"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Row(1).Height = 30;

                        // 1. Título arriba (una sola fila)
                        ws.Cells["A46:L46"].Merge = true;
                        ws.Cells["A46"].Value = $"TOTAL DE OBRAS: {totalObras}";
                        ws.Cells["A46"].Style.Font.Size = 18;
                        ws.Cells["A46"].Style.Font.Bold = true;
                        ws.Cells["A46"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells["A46"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                        ws.Cells["A46"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Row(1).Height = 28;

                        // 2. Gráfico de torta (izquierda, columnas A-B)
                        var chart = ws.Drawings.AddChart("graficoTorta", OfficeOpenXml.Drawing.Chart.eChartType.Pie);
                        //chart.Title.Text = "Distribución de Obras";
                        chart.SetPosition(47, 0, 5, 0); // Fila 3, Columna A
                        chart.SetSize(400, 300);
                        var serie = chart.Series.Add("datosGrafico!B2:B5", "datosGrafico!A2:A5");
                        serie.Header = "Cantidad";


                        // 3. Cuadros de datos (derecha, columnas E-H)
                        int colBase = 1; // Columna A
                        int rowBase = 50; // Fila inicial para cuadros

                        for (int i = 0; i < 4; i++)
                        {
                            int row = rowBase + (i % 2) * 6; // dos arriba, dos abajo
                            int col = colBase + (i / 2) * 2; // dos columnas

                            // Encabezado
                            var headerCell = ws.Cells[row, col, row, col + 1];
                            headerCell.Merge = true;
                            headerCell.Value = titulos[i];
                            headerCell.Style.Font.Bold = true;
                            headerCell.Style.Font.Size = 14; // Más grande
                            headerCell.Style.Font.Color.SetColor(System.Drawing.Color.White);
                            headerCell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            headerCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#003366")); // Azul más oscuro
                            headerCell.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick, System.Drawing.ColorTranslator.FromHtml("#003366"));
                            headerCell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Row(row).Height = 28;

                            // Valor
                            var valueCell = ws.Cells[row + 1, col, row + 3, col + 1];
                            valueCell.Merge = true;
                            valueCell.Value = valores[i];
                            valueCell.Style.Font.Size = 16; // Más chico
                            valueCell.Style.Font.Bold = true;
                            valueCell.Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#003366")); // Azul
                            valueCell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            valueCell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            valueCell.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick, System.Drawing.ColorTranslator.FromHtml("#003366"));
                        }

                        #endregion

                        #region TercerHoja
                        //ws.Row(86).PageBreak = true; // Salto de página después de la fila 84

                        for (int fila = 86; fila <= 87; fila++)
                        {
                            for (int col = 1; col <= 12; col++)
                            {
                                ws.Cells[fila, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Cells[fila, col].Style.Fill.BackgroundColor.SetColor(azul);
                            }
                        }
                        // 2. Insertar imagen en J1
                        string rutaImagen3 = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Iconos/LogoMzaReducido2.png");
                        if (File.Exists(rutaImagen3))
                        {
                            var imagen1 = Image.FromFile(rutaImagen3);
                            var picture1 = ws.Drawings.AddPicture("Imagen3", rutaImagen3);
                            //picture.SetPosition(10, 0, 9, 0); // Fila 1 (índice 0), Columna J (índice 9)
                            picture1.SetPosition(85, 3, 10, 1); // Fila 1, columna J, 1px de margen izquierdo, 2px margen superior

                            picture1.SetSize(84, 30);
                        }

                        // Título superior
                        ws.Cells["A91:L91"].Merge = true;
                        ws.Cells["A91"].Value = $"DATOS INFRAESTRUCTURA";
                        ws.Cells["A91"].Style.Font.Size = 16;
                        ws.Cells["A91"].Style.Font.Bold = true;
                        ws.Cells["A91"].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#003366")); // Azul
                        ws.Cells["A91"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;
                        ws.Cells["A91"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Row(1).Height = 30;

                        // 2. Insertar imagen en J1
                        string rutaImagen4 = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Iconos/Recurso1.png");
                        if (File.Exists(rutaImagen4))
                        {
                            var imagen4 = Image.FromFile(rutaImagen4);
                            var picture4 = ws.Drawings.AddPicture("Imagen4", rutaImagen4);
                            picture4.SetPosition(95, 0, 1, 0); // Fila 1, columna J, 1px de margen izquierdo, 2px margen superior

                            picture4.SetSize(50, 50);
                        }
                        // Título superior
                        ws.Cells["C96"].Value = $"0 M2";
                        ws.Cells["C96"].Style.Font.Size = 12;
                        ws.Cells["C96"].Style.Font.Bold = true;
                        ws.Cells["C96"].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#f7BD56")); // Azul
                        ws.Cells["C96"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;
                        //ws.Cells["D96"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        // Título superior
                        ws.Cells["C97"].Value = $"CONSTRUIDOS EN SALUD";
                        ws.Cells["C97"].Style.Font.Size = 11;
                        ws.Cells["C97"].Style.Font.Bold = true;
                        ws.Cells["C97"].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#003366")); // Azul
                        ws.Cells["C97"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;
                        //ws.Cells["D97"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        // 2. Insertar imagen en J1
                        string rutaImagen5 = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Iconos/Recurso4.png");
                        if (File.Exists(rutaImagen5))
                        {
                            var imagen5 = Image.FromFile(rutaImagen5);
                            var picture5 = ws.Drawings.AddPicture("Imagen5", rutaImagen5);
                            picture5.SetPosition(95, 0, 7, 0); // Fila 1, columna J, 1px de margen izquierdo, 2px margen superior

                            picture5.SetSize(50, 50);
                        }

                        // Título superior
                        ws.Cells["I96"].Value = $"0 M2";
                        ws.Cells["I96"].Style.Font.Size = 12;
                        ws.Cells["I96"].Style.Font.Bold = true;
                        ws.Cells["I96"].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#f7BD56")); // Azul
                        ws.Cells["I96"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;
                        //ws.Cells["D96"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        // Título superior
                        ws.Cells["I97"].Value = $"CONSTRUIDOS EN ESCUELAS";
                        ws.Cells["I97"].Style.Font.Size = 11;
                        ws.Cells["I97"].Style.Font.Bold = true;
                        ws.Cells["I97"].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#003366")); // Azul
                        ws.Cells["I97"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;
                        //ws.Cells["D97"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        // 2. Insertar imagen en J1
                        string rutaImagen6 = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Iconos/Recurso2.png");
                        if (File.Exists(rutaImagen6))
                        {
                            var imagen6 = Image.FromFile(rutaImagen6);
                            var picture6 = ws.Drawings.AddPicture("Imagen6", rutaImagen6);
                            picture6.SetPosition(99, 0, 1, 0); // Fila 1, columna J, 1px de margen izquierdo, 2px margen superior

                            picture6.SetSize(50, 50);
                        }
                        // Título superior
                        ws.Cells["C100"].Value = $"0 KM";
                        ws.Cells["C100"].Style.Font.Size = 12;
                        ws.Cells["C100"].Style.Font.Bold = true;
                        ws.Cells["C100"].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#f7BD56")); // Azul
                        ws.Cells["C100"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;
                        //ws.Cells["D96"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        // Título superior
                        ws.Cells["C101"].Value = $"NUEVAS RUTAS";
                        ws.Cells["C101"].Style.Font.Size = 11;
                        ws.Cells["C101"].Style.Font.Bold = true;
                        ws.Cells["C101"].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#003366")); // Azul
                        ws.Cells["C101"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;
                        //ws.Cells["D97"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        // Título superior
                        ws.Cells["G100"].Value = $"0 KM";
                        ws.Cells["G100"].Style.Font.Size = 12;
                        ws.Cells["G100"].Style.Font.Bold = true;
                        ws.Cells["G100"].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#f7BD56")); // Azul
                        ws.Cells["G100"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;
                        //ws.Cells["D96"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        // Título superior
                        ws.Cells["G101"].Value = $"RUTAS PAVIMENTADAS";
                        ws.Cells["G101"].Style.Font.Size = 11;
                        ws.Cells["G101"].Style.Font.Bold = true;
                        ws.Cells["G101"].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#003366")); // Azul
                        ws.Cells["G101"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;
                        //ws.Cells["D97"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        // Título superior
                        ws.Cells["J100"].Value = $"0 KM";
                        ws.Cells["J100"].Style.Font.Size = 12;
                        ws.Cells["J100"].Style.Font.Bold = true;
                        ws.Cells["J100"].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#f7BD56")); // Azul
                        ws.Cells["J100"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;
                        //ws.Cells["D96"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        // Título superior
                        ws.Cells["J101"].Value = $"RUTAS CONSERVADAS";
                        ws.Cells["J101"].Style.Font.Size = 11;
                        ws.Cells["J101"].Style.Font.Bold = true;
                        ws.Cells["J101"].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#003366")); // Azul
                        ws.Cells["J101"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;
                        //ws.Cells["D97"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        // 2. Insertar imagen en J1
                        string rutaImagen7 = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Iconos/Recurso3.png");
                        if (File.Exists(rutaImagen7))
                        {
                            var imagen6 = Image.FromFile(rutaImagen7);
                            var picture6 = ws.Drawings.AddPicture("Imagen7", rutaImagen7);
                            picture6.SetPosition(104, 0, 1, 0); // Fila 1, columna J, 1px de margen izquierdo, 2px margen superior

                            picture6.SetSize(50, 50);
                        }
                        // Título superior
                        ws.Cells["C105"].Value = $"0 KM";
                        ws.Cells["C105"].Style.Font.Size = 12;
                        ws.Cells["C105"].Style.Font.Bold = true;
                        ws.Cells["C105"].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#f7BD56")); // Azul
                        ws.Cells["C105"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;
                        //ws.Cells["D96"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        // Título superior
                        ws.Cells["C106"].Value = $"IMPERMEABILIZACION";
                        ws.Cells["C106"].Style.Font.Size = 11;
                        ws.Cells["C106"].Style.Font.Bold = true;
                        ws.Cells["C106"].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#003366")); // Azul
                        ws.Cells["C106"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;
                        //ws.Cells["D97"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        // Título superior
                        ws.Cells["C107"].Value = $"DE CANALES";
                        ws.Cells["C107"].Style.Font.Size = 11;
                        ws.Cells["C107"].Style.Font.Bold = true;
                        ws.Cells["C107"].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#003366")); // Azul
                        ws.Cells["C107"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;
                        //ws.Cells["D97"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        // Título superior
                        ws.Cells["G105"].Value = $"0 HA";
                        ws.Cells["G105"].Style.Font.Size = 12;
                        ws.Cells["G105"].Style.Font.Bold = true;
                        ws.Cells["G105"].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#f7BD56")); // Azul
                        ws.Cells["G105"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;
                        //ws.Cells["D96"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        // Título superior
                        ws.Cells["G106"].Value = $"BENEFICIADAS";
                        ws.Cells["G106"].Style.Font.Size = 11;
                        ws.Cells["G106"].Style.Font.Bold = true;
                        ws.Cells["G106"].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#003366")); // Azul
                        ws.Cells["G106"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;
                        //ws.Cells["D97"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        // Título superior
                        ws.Cells["J105"].Value = $"0 USUARIOS";
                        ws.Cells["J105"].Style.Font.Size = 12;
                        ws.Cells["J105"].Style.Font.Bold = true;
                        ws.Cells["J105"].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#f7BD56")); // Azul
                        ws.Cells["J105"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;
                        //ws.Cells["D96"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        // Título superior
                        ws.Cells["J106"].Value = $"BENEFICIADOS";
                        ws.Cells["J106"].Style.Font.Size = 11;
                        ws.Cells["J106"].Style.Font.Bold = true;
                        ws.Cells["J106"].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#003366")); // Azul
                        ws.Cells["J106"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;

                        #endregion

                        #region CuartaHoja
                        for (int fila = 132; fila <= 133; fila++)
                        {
                            for (int col = 1; col <= 12; col++)
                            {
                                ws.Cells[fila, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Cells[fila, col].Style.Fill.BackgroundColor.SetColor(azul);
                            }
                        }
                        // 2. Insertar imagen en J1
                        string rutaImagen8 = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Iconos/LogoMzaReducido3.png");
                        if (File.Exists(rutaImagen8))
                        {
                            var imagen1 = Image.FromFile(rutaImagen8);
                            var picture1 = ws.Drawings.AddPicture("Imagen8", rutaImagen8);
                            //picture.SetPosition(10, 0, 9, 0); // Fila 1 (índice 0), Columna J (índice 9)
                            picture1.SetPosition(131, 3, 10, 1); // Fila 1, columna J, 1px de margen izquierdo, 2px margen superior

                            picture1.SetSize(84, 30);
                        }
                        // 2. Insertar Mapa
                        string rutaImagen9 = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Iconos/MapaMendoza.png");
                        if (File.Exists(rutaImagen9))
                        {
                            var imagen1 = Image.FromFile(rutaImagen9);
                            var picture1 = ws.Drawings.AddPicture("Imagen9", rutaImagen9);
                            //picture.SetPosition(10, 0, 9, 0); // Fila 1 (índice 0), Columna J (índice 9)
                            picture1.SetPosition(137, 3, 2, 1); // Fila 1, columna J, 1px de margen izquierdo, 2px margen superior

                            picture1.SetSize(400, 650);//448
                        }

                        // 2. Insertar Flecha#1
                        string rutaImagen10 = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Iconos/flecha1.png");
                        if (File.Exists(rutaImagen10))
                        {
                            var imagen1 = Image.FromFile(rutaImagen10);
                            var picture1 = ws.Drawings.AddPicture("Imagen10", rutaImagen10);
                            //picture.SetPosition(10, 0, 9, 0); // Fila 1 (índice 0), Columna J (índice 9)
                            picture1.SetPosition(133, 3, 2, 1); // Fila 1, columna J, 1px de margen izquierdo, 2px margen superior

                            picture1.SetSize(130, 130);
                        }
                        var listaDistrito = await BuscarPorDistrito();
                        listaDistrito = listaDistrito.OrderBy(x => x.NroDistrito).ToList();
                        var totalObrasEnEjecucion1 = listaDistrito.Where(x=>x.NroDistrito == 1).Sum(x => x.ObrasEjecucion) ?? 0;
                        var totalObrasEnLicitacion1 = listaDistrito.Where(x => x.NroDistrito == 1).Sum(x => x.ObrasLicitacion) ?? 0;
                        var totalObrasParalizadas1 = listaDistrito.Where(x => x.NroDistrito == 1).Sum(x => x.ObrasParalizadas) ?? 0;
                        var totalObrasEntregadas1 = listaDistrito.Where(x => x.NroDistrito == 1).Sum(x => x.ObrasEntregadas) ?? 0;

                        ws.Cells["A135"].Value = $"PRIMER DISTRITO";
                        ws.Cells["A135"].Style.Font.Size = 9;
                        ws.Cells["A135"].Style.Font.Bold = true;
                        ws.Cells["A135"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;

                        ws.Cells["A136"].Value = $"En ejecución: " + totalObrasEnEjecucion1;
                        ws.Cells["A136"].Style.Font.Size = 10;
                        ws.Cells["A136"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;


                        ws.Cells["A137"].Value = $"En Licitación: " + totalObrasEnLicitacion1;
                        ws.Cells["A137"].Style.Font.Size = 10;
                        ws.Cells["A137"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;

                        ws.Cells["A138"].Value = $"Entregadas: " + totalObrasParalizadas1;
                        ws.Cells["A138"].Style.Font.Size = 10;
                        ws.Cells["A138"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;

                        ws.Cells["A139"].Value = $"Paralizadas: " + totalObrasEntregadas1;
                        ws.Cells["A139"].Style.Font.Size = 10;
                        ws.Cells["A139"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;
                        
                        // 2. Insertar Flecha#1
                        string rutaImagen11 = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Iconos/flecha3.png");
                        if (File.Exists(rutaImagen11))
                        {
                            var imagen1 = Image.FromFile(rutaImagen11);
                            var picture1 = ws.Drawings.AddPicture("Imagen11", rutaImagen11);
                            //picture.SetPosition(10, 0, 9, 0); // Fila 1 (índice 0), Columna J (índice 9)
                            picture1.SetPosition(146, 3, 2, 1); // Fila 1, columna J, 1px de margen izquierdo, 2px margen superior

                            picture1.SetSize(130, 130);
                        }

                        var totalObrasEnEjecucion2 = listaDistrito.Where(x => x.NroDistrito == 2).Sum(x => x.ObrasEjecucion) ?? 0;
                        var totalObrasEnLicitacion2 = listaDistrito.Where(x => x.NroDistrito == 2).Sum(x => x.ObrasLicitacion) ?? 0;
                        var totalObrasParalizadas2 = listaDistrito.Where(x => x.NroDistrito == 2).Sum(x => x.ObrasParalizadas) ?? 0;
                        var totalObrasEntregadas2 = listaDistrito.Where(x => x.NroDistrito == 2).Sum(x => x.ObrasEntregadas) ?? 0;

                        ws.Cells["A148"].Value = $"SEGUNDO DISTRITO";
                        ws.Cells["A148"].Style.Font.Size = 9;
                        ws.Cells["A148"].Style.Font.Bold = true;
                        ws.Cells["A148"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;

                        ws.Cells["A149"].Value = $"En ejecución: " + totalObrasEnEjecucion2;
                        ws.Cells["A149"].Style.Font.Size = 10;
                        ws.Cells["A149"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;


                        ws.Cells["A150"].Value = $"En Licitación: " + totalObrasEnLicitacion2;
                        ws.Cells["A150"].Style.Font.Size = 10;
                        ws.Cells["A150"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;

                        ws.Cells["A151"].Value = $"Entregadas: " + totalObrasParalizadas2;
                        ws.Cells["A151"].Style.Font.Size = 10;
                        ws.Cells["A151"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;

                        ws.Cells["A152"].Value = $"Paralizadas: " + totalObrasEntregadas2;
                        ws.Cells["A152"].Style.Font.Size = 10;
                        ws.Cells["A152"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;

                        // 2. Insertar Flecha#1
                        string rutaImagen12 = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Iconos/flecha2.png");
                        if (File.Exists(rutaImagen12))
                        {
                            var imagen1 = Image.FromFile(rutaImagen12);
                            var picture1 = ws.Drawings.AddPicture("Imagen12", rutaImagen12);
                            picture1.SetPosition(142, 3, 7, 1); // Fila 1, columna J, 1px de margen izquierdo, 2px margen superior

                            picture1.SetSize(130, 130);
                        }

                        var totalObrasEnEjecucion3 = listaDistrito.Where(x => x.NroDistrito == 3).Sum(x => x.ObrasEjecucion) ?? 0;
                        var totalObrasEnLicitacion3 = listaDistrito.Where(x => x.NroDistrito == 3).Sum(x => x.ObrasLicitacion) ?? 0;
                        var totalObrasParalizadas3 = listaDistrito.Where(x => x.NroDistrito == 3).Sum(x => x.ObrasParalizadas) ?? 0;
                        var totalObrasEntregadas3 = listaDistrito.Where(x => x.NroDistrito == 3).Sum(x => x.ObrasEntregadas) ?? 0;

                        ws.Cells["K144"].Value = $"TERCER DISTRITO";
                        ws.Cells["K144"].Style.Font.Size = 9;
                        ws.Cells["K144"].Style.Font.Bold = true;
                        ws.Cells["K144"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;

                        ws.Cells["K145"].Value = $"En ejecución: " + totalObrasEnEjecucion3;
                        ws.Cells["K145"].Style.Font.Size = 10;
                        ws.Cells["K145"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;


                        ws.Cells["K146"].Value = $"En Licitación: " + totalObrasEnLicitacion3;
                        ws.Cells["K146"].Style.Font.Size = 10;
                        ws.Cells["K146"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;

                        ws.Cells["K147"].Value = $"Entregadas: " + totalObrasParalizadas3;
                        ws.Cells["K147"].Style.Font.Size = 10;
                        ws.Cells["K147"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;

                        ws.Cells["K148"].Value = $"Paralizadas: " + totalObrasEntregadas3;
                        ws.Cells["K148"].Style.Font.Size = 10;
                        ws.Cells["K148"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;

                        // 2. Insertar Flecha#1
                        string rutaImagen13 = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Iconos/flecha4.png");
                        if (File.Exists(rutaImagen13))
                        {
                            var imagen1 = Image.FromFile(rutaImagen13);
                            var picture1 = ws.Drawings.AddPicture("Imagen13", rutaImagen13);
                            picture1.SetPosition(150, 3, 7, 1); // Fila 1, columna J, 1px de margen izquierdo, 2px margen superior

                            picture1.SetSize(130, 130);
                        }

                        var totalObrasEnEjecucion4 = listaDistrito.Where(x => x.NroDistrito == 4).Sum(x => x.ObrasEjecucion) ?? 0;
                        var totalObrasEnLicitacion4 = listaDistrito.Where(x => x.NroDistrito == 4).Sum(x => x.ObrasLicitacion) ?? 0;
                        var totalObrasParalizadas4 = listaDistrito.Where(x => x.NroDistrito == 4).Sum(x => x.ObrasParalizadas) ?? 0;
                        var totalObrasEntregadas4 = listaDistrito.Where(x => x.NroDistrito == 4).Sum(x => x.ObrasEntregadas) ?? 0;

                        ws.Cells["K153"].Value = $"CUARTO DISTRITO";
                        ws.Cells["K153"].Style.Font.Size = 9;
                        ws.Cells["K153"].Style.Font.Bold = true;
                        ws.Cells["K153"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;

                        ws.Cells["K154"].Value = $"En ejecución: " + totalObrasEnEjecucion3;
                        ws.Cells["K154"].Style.Font.Size = 10;
                        ws.Cells["K154"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;


                        ws.Cells["K155"].Value = $"En Licitación: " + totalObrasEnLicitacion3;
                        ws.Cells["K155"].Style.Font.Size = 10;
                        ws.Cells["K155"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;

                        ws.Cells["K156"].Value = $"Entregadas: " + totalObrasParalizadas3;
                        ws.Cells["K156"].Style.Font.Size = 10;
                        ws.Cells["K156"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;

                        ws.Cells["K157"].Value = $"Paralizadas: " + totalObrasEntregadas3;
                        ws.Cells["K157"].Style.Font.Size = 10;
                        ws.Cells["K157"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;

                        #endregion

                        #region QuintaHoja
                        for (int fila = 179; fila <= 225; fila++)
                        {
                            for (int col = 1; col <= 12; col++)
                            {
                                ws.Cells[fila, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Cells[fila, col].Style.Fill.BackgroundColor.SetColor(azul);
                            }
                        }

                        // Título superior
                        ws.Cells["A195:L195"].Merge = true;
                        ws.Cells["A195"].Value = $"Subsecretaria de Infraestructura ";
                        ws.Cells["A195"].Style.Font.Size = 28;
                        ws.Cells["A195"].Style.Font.Bold = true;
                        ws.Cells["A195"].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF")); // Azul
                        ws.Cells["A195"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;
                        ws.Cells["A195"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        // Título superior
                        ws.Cells["A196:L196"].Merge = true;
                        ws.Cells["A196"].Value = $"y Desarrollo Territorial";
                        ws.Cells["A196"].Style.Font.Size = 28;
                        ws.Cells["A196"].Style.Font.Bold = true;
                        ws.Cells["A196"].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF")); // Azul
                        ws.Cells["A196"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;
                        ws.Cells["A196"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        #endregion

                        #region BuscaBDOrganismo
                        var listaOrganismo = await BuscarTotalesPorOrganismo(listaObras);
                        #endregion

                        #region SectaHoja

                        for (int fila = 226; fila <= 227; fila++)
                        {
                            for (int col = 1; col <= 12; col++)
                            {
                                ws.Cells[fila, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Cells[fila, col].Style.Fill.BackgroundColor.SetColor(azul);
                            }
                        }
                        // 2. Insertar imagen en J1
                        string rutaImage14 = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Iconos/LogoMzaReducido4.png");
                        if (File.Exists(rutaImage14))
                        {
                            var imagen1 = Image.FromFile(rutaImage14);
                            var picture1 = ws.Drawings.AddPicture("Imagen14", rutaImage14);
                            //picture.SetPosition(10, 0, 9, 0); // Fila 1 (índice 0), Columna J (índice 9)
                            picture1.SetPosition(225, 3, 10, 1); // Fila 1, columna J, 1px de margen izquierdo, 2px margen superior

                            picture1.SetSize(84, 30);
                        }

                        totalEjecucion = 0;
                        totalLicitacion = 0;
                        totalEntregadas = 0;
                        totalParalizadas = 0;
                        totalObras = 0;
                        totalEjecucion = listaOrganismo.Where(x=>x.Organismo.Contains("Infraestr")).Sum(x => x.CantidadObraEjecucion) ?? 0;
                        totalLicitacion = listaOrganismo.Where(x => x.Organismo.Contains("Infraestr")).Sum(x => x.CantidadObraLicitacion) ?? 0;
                        totalEntregadas = listaOrganismo.Where(x => x.Organismo.Contains("Infraestr")).Sum(x => x.CantidadObraFinalizada) ?? 0;
                        totalParalizadas = listaOrganismo.Where(x => x.Organismo.Contains("Infraestr")).Sum(x => x.CantidadObraEjecucion) ?? 0;
                        totalObras = totalEjecucion + totalLicitacion + totalEntregadas + totalParalizadas; 

                        // Hoja de datos para el gráfico
                        var wsDatos2 = package.Workbook.Worksheets.Add("datosGrafico2");
                        string[] titulos2 = { "En Licitación", "En Ejecución", "Entregadas", "Paralizadas" };
                        int[] valores2 = { totalLicitacion, totalEjecucion, totalEntregadas, totalParalizadas };
                        string[] colores2 = { "#007bff", "#003366", "#bfa16c", "#d3d3d3" }; // azul, azul oscuro, marrón claro, gris claro

                        wsDatos.Cells["A230"].Value = "Tipo";
                        wsDatos.Cells["B230"].Value = "Cantidad";
                        for (int i = 0; i < 4; i++)
                        {
                            wsDatos.Cells[2 + i, 1].Value = titulos[i];
                            wsDatos.Cells[2 + i, 2].Value = valores[i];
                        }


                        ws.Row(41).PageBreak = true; // Salto de página después de la fila 42

                        // Título superior: CONSOLIDADO - OBRAS GESTIONADAS AL X
                        ws.Cells["A245:L245"].Merge = true;
                        ws.Cells["A245"].Value = $"SUBSECRETARIA DE INFRAESTRUCTURA";
                        ws.Cells["A245"].Style.Font.Size = 16;
                        ws.Cells["A245"].Style.Font.Bold = true;
                        ws.Cells["A245"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        ws.Cells["A245"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;
                        ws.Cells["A245"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;                        

                        ws.Cells["A246:L246"].Merge = true;
                        ws.Cells["A246"].Value = $"Y DESARROLLO TERRITORIAL";
                        ws.Cells["A246"].Style.Font.Size = 16;
                        ws.Cells["A246"].Style.Font.Bold = true;
                        ws.Cells["A245"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        ws.Cells["A246"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.None;
                        ws.Cells["A246"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        

                        // 1. Título arriba (una sola fila)
                        ws.Cells["A247:L247"].Merge = true;
                        ws.Cells["A247"].Value = $"TOTAL DE OBRAS: {totalObras}";
                        ws.Cells["A247"].Style.Font.Size = 18;
                        ws.Cells["A247"].Style.Font.Bold = true;
                        ws.Cells["A247"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells["A247"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                        ws.Cells["A247"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        // 2. Gráfico de torta (izquierda, columnas A-B)
                        var chart2 = ws.Drawings.AddChart("graficoTorta2", OfficeOpenXml.Drawing.Chart.eChartType.Pie);
                        //chart.Title.Text = "Distribución de Obras";
                        chart2.SetPosition(247, 0, 5, 0); // Fila 3, Columna A
                        chart2.SetSize(400, 300);
                        var serie2 = chart2.Series.Add("datosGrafico2!B2:B5", "datosGrafico2!A2:A5");
                        serie2.Header = "Cantidad";


                        // 3. Cuadros de datos (derecha, columnas E-H)
                        int colBase2 = 1; // Columna A
                        int rowBase2 = 250; // Fila inicial para cuadros

                        for (int i = 0; i < 4; i++)
                        {
                            int row = rowBase2 + (i % 2) * 6; // dos arriba, dos abajo
                            int col = colBase2 + (i / 2) * 2; // dos columnas

                            // Encabezado
                            var headerCell = ws.Cells[row, col, row, col + 1];
                            headerCell.Merge = true;
                            headerCell.Value = titulos[i];
                            headerCell.Style.Font.Bold = true;
                            headerCell.Style.Font.Size = 14; // Más grande
                            headerCell.Style.Font.Color.SetColor(System.Drawing.Color.White);
                            headerCell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            headerCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#003366")); // Azul más oscuro
                            headerCell.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick, System.Drawing.ColorTranslator.FromHtml("#003366"));
                            headerCell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Row(row).Height = 28;

                            // Valor
                            var valueCell = ws.Cells[row + 1, col, row + 3, col + 1];
                            valueCell.Merge = true;
                            valueCell.Value = valores[i];
                            valueCell.Style.Font.Size = 16; // Más chico
                            valueCell.Style.Font.Bold = true;
                            valueCell.Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#003366")); // Azul
                            valueCell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            valueCell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            valueCell.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick, System.Drawing.ColorTranslator.FromHtml("#003366"));
                        }

                        #endregion
                        
                        for (int i = 1; i <= 12; i++)
                        {
                            ws.Column(i).Width = 7;
                        }

                        var stream = new MemoryStream(package.GetAsByteArray());
                        var result = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new ByteArrayContent(stream.ToArray())
                        };
                        result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                        {
                            FileName = "InformeTotalObras.xlsx"
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
        private async Task<List<DetalleDistrito>> BuscarPorDistrito()
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
                        if ((item.IdDepartamento == 1) || (item.IdDepartamento == 3) || (item.IdDepartamento == 7) || (item.IdDepartamento == 8))
                        {
                            detalleDistrito.NombreDistrito = "Primer Distrito";
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
                        if (detalleDistrito.NombreDistrito != null)
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
                    return listaAgrupada;
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                return null;
            }
        }
        private async Task<List<ContadorObra>> BuscarTotalesPorOrganismo(List<vw_looker_obras> listaOriginal)
        {
            var lista = new List<ContadorObra>();

            try
            {

                var listaEjecucion = listaOriginal
                    .Where(x => x.IdEstado == 1 && x.esEntregada == false && (x.OrganismoId == 2 || x.OrganismoId == 4
                        || x.OrganismoId == 9 || x.OrganismoId == 14 || x.OrganismoId == 20))
                    .Distinct().ToList();

                var listaLicitacion = listaOriginal
                    .Where(x => x.PryStage_Id == 49 && (x.IdEstado == 6 || x.IdEstado == 7 || x.IdEstado == 8 || x.IdEstado == 14 || x.IdEstado == 15) && (x.OrganismoId == 2
                        || x.OrganismoId == 4 || x.OrganismoId == 9 || x.OrganismoId == 14 || x.OrganismoId == 20))
                    .Distinct().ToList();

                var listaParalizada = listaOriginal
                    .Where(x => x.PryStage_Id == 49 && (x.IdEstado == 6 || x.IdEstado == 7 || x.IdEstado == 8 || x.IdEstado == 14 || x.IdEstado == 15) && (x.OrganismoId == 2
                        || x.OrganismoId == 4 || x.OrganismoId == 9 || x.OrganismoId == 14 || x.OrganismoId == 20))
                    .Distinct().ToList();

                var fechaLimite = new DateTime(2024, 1, 1);

                var listaFinalizadas = listaOriginal    
                    .Where(x =>
                        (
                            (
                                x.PryStage_Id == 48 &&
                                x.IdEstado == 18 &&
                                (
                                    ((x.FechaFinActualizada ?? DateTime.MinValue) > (x.FechaFin ?? DateTime.MinValue)
                                        ? x.FechaFinActualizada
                                        : x.FechaFin) >= fechaLimite
                                )
                            )
                            ||
                            (
                                x.PryStage_Id == 160 &&
                                x.IdEstado == 3 &&
                                (
                                    ((x.FechaFinActualizada ?? DateTime.MinValue) > (x.FechaFin ?? DateTime.MinValue)
                                        ? x.FechaFinActualizada
                                        : x.FechaFin) >= fechaLimite
                                )
                            )
                            ||
                            x.esEntregada == true
                        )
                        && (x.OrganismoId == 2 || x.OrganismoId == 4 || x.OrganismoId == 9 || x.OrganismoId == 14 || x.OrganismoId == 20)
                    )
                    .Distinct()
                    .ToList();


                var listaEjecucionAySAM = listaOriginal
                    .Where(x => x.IdEstado == 1 && x.esEntregada == false && x.OrganismoId == 14)
                    .Distinct().ToList();
                var listaLicitacionAySAM = listaLicitacion.Where(x => x.OrganismoId == 14).Distinct().ToList();

                var listaEjecucionIPV = listaOriginal
                    .Where(x => x.IdEstado == 1 && x.OrganismoId == 4)
                    .Distinct().ToList();
                var listaLicitacionIPV = listaLicitacion.Where(x => x.OrganismoId == 4).Distinct().ToList();
                var listaParalizadaIPV= listaParalizada.Where(x => x.OrganismoId == 4).Distinct().ToList();

                var listaEjecucionVialidad = listaOriginal.Where(x => x.IdEstado == 1 && x.esEntregada == false && x.OrganismoId == 9)
                    .Distinct().ToList();
                var listaLicitacionVialidad = listaLicitacion.Where(x => x.OrganismoId == 9).Distinct().ToList();
                var listaParalizadaVialidad = listaParalizada.Where(x => x.OrganismoId == 9).Distinct().ToList();

                var listaEjecucionInfra = listaOriginal.Where(x => x.IdEstado == 1 && x.esEntregada == false && x.OrganismoId == 2)
                    .Distinct().ToList();
                var listaLicitacionInfra = listaLicitacion.Where(x => x.OrganismoId == 2).Distinct().ToList();
                var listaParalizadaInfra = listaParalizada.Where(x=>x.OrganismoId == 2).Distinct().ToList();

                var listaEjecucionIrrigacion = listaOriginal.Where(x => x.IdEstado == 1 && x.esEntregada == false && x.OrganismoId == 20)
                        .Distinct().ToList();
                var listaLicitacionIrrigacion = listaLicitacion.Where(x => x.OrganismoId == 20).Distinct().ToList();
                var listaParalizadaIrrigacion = listaParalizada.Where(x => x.OrganismoId == 20).Distinct().ToList();

                var unItemAysam = new ContadorObra();
                unItemAysam.CantidadObraEjecucion = listaEjecucionAySAM.Count;
                unItemAysam.MontoObraEjecucion = Convert.ToInt64(listaEjecucionAySAM.Sum(x => (decimal?)x.MontoContratado) ?? 0);
                unItemAysam.CantidadObraLicitacion = listaLicitacionAySAM.Count;
                unItemAysam.MontoObraLicitacion = Convert.ToInt64(listaLicitacionAySAM.Sum(x => (decimal?)x.MontoOficial) ?? 0);
                unItemAysam.IdOrganismo = 14;
                unItemAysam.Organismo = "AySAM";
                unItemAysam.CantidadObraFinalizada = listaFinalizadas.Where(x => x.OrganismoId == 14).Count();
                lista.Add(unItemAysam);

                var unItemIPV = new ContadorObra();
                unItemIPV.CantidadObraEjecucion = listaEjecucionIPV.Count;
                unItemIPV.MontoObraEjecucion = Convert.ToInt64(listaEjecucionIPV.Sum(x => (decimal?)x.MontoContratado) ?? 0);
                unItemIPV.CantidadObraLicitacion = listaLicitacionIPV.Count;
                unItemIPV.MontoObraLicitacion = Convert.ToInt64(listaLicitacionIPV.Sum(x => (decimal?)x.MontoOficial) ?? 0);
                unItemIPV.CantidadObraFinalizada = listaFinalizadas.Where(x => x.OrganismoId == 4).Count();
                unItemIPV.IdOrganismo = 4;
                unItemIPV.Organismo = "IPV";
                lista.Add(unItemIPV);

                var unItemVialidad = new ContadorObra();
                unItemVialidad.CantidadObraEjecucion = listaEjecucionVialidad.Count;
                unItemVialidad.MontoObraEjecucion = Convert.ToInt64(listaEjecucionVialidad.Sum(x => (decimal?)x.MontoContratado) ?? 0);
                unItemVialidad.CantidadObraLicitacion = listaLicitacionVialidad.Count;
                unItemVialidad.MontoObraLicitacion = Convert.ToInt64(listaLicitacionVialidad.Sum(x => (decimal?)x.MontoOficial) ?? 0);
                unItemVialidad.IdOrganismo = 9;
                unItemVialidad.CantidadObraFinalizada = listaFinalizadas.Where(x => x.OrganismoId == 9).Count();
                unItemVialidad.Organismo = "Vialidad";
                lista.Add(unItemVialidad);

                var unItemInfra = new ContadorObra();
                unItemInfra.CantidadObraEjecucion = listaEjecucionInfra.Count;
                unItemInfra.MontoObraEjecucion = Convert.ToInt64(listaEjecucionInfra.Sum(x => (decimal?)x.MontoContratado) ?? 0);
                unItemInfra.CantidadObraLicitacion = listaLicitacionInfra.Count;
                unItemInfra.MontoObraLicitacion = Convert.ToInt64(listaLicitacionInfra.Sum(x => (decimal?)x.MontoOficial) ?? 0);
                unItemInfra.IdOrganismo = 2;
                unItemInfra.CantidadObraFinalizada = listaFinalizadas.Where(x => x.OrganismoId == 2).Count();
                unItemInfra.Organismo = "Infraestructura";
                lista.Add(unItemInfra);

                var unItemIrrig = new ContadorObra();
                unItemIrrig.CantidadObraEjecucion = listaEjecucionIrrigacion.Count;
                unItemIrrig.MontoObraEjecucion = Convert.ToInt64(listaEjecucionIrrigacion.Sum(x => (decimal?)x.MontoContratado) ?? 0);
                unItemIrrig.CantidadObraLicitacion = listaLicitacionIrrigacion.Count;
                unItemIrrig.MontoObraLicitacion = Convert.ToInt64(listaLicitacionIrrigacion.Sum(x => (decimal?)x.MontoOficial) ?? 0);
                unItemIrrig.IdOrganismo = 20;
                unItemIrrig.CantidadObraFinalizada = listaFinalizadas.Where(x => x.OrganismoId == 20).Count();
                unItemIrrig.Organismo = "Irrigación";
                lista.Add(unItemIrrig);                
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                if (lista.Count == 0)
                {
                    lista.Add(new ContadorObra
                    {
                        CantidadObraEjecucion = 0,
                        MontoObraEjecucion = 0,
                        CantidadObraLicitacion = 0,
                        MontoObraLicitacion = 0,
                        IdOrganismo = 0,
                        Organismo = "Error",
                        CantidadObraFinalizada = 0
                    });
                }
            }
            return lista;
        }
    }
}