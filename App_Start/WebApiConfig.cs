using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace VisorObraCFI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configurar Web API para usar camel case en los nombres de las propiedades JSON
            var jsonFormatter = config.Formatters.JsonFormatter;
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Eliminar el formateador XML
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Rutas de Web API
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
