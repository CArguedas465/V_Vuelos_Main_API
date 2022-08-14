using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using V_Vuelos_Main_API.Crypto;
using V_Vuelos_Main_API.Models;

namespace V_Vuelos_Main_API.Controllers
{
    public class BusquedaVuelosController : ApiController
    {
        private V_Vuelos_MainEntities db;
        Crypt c;
        public BusquedaVuelosController()
        {
            db = new V_Vuelos_MainEntities();
            c = new Crypt();
        }

        // POST: api/BusquedaVuelos
        public IHttpActionResult Post(BusquedaVuelos busqueda)
        {
            List<uspRecuperarVuelos_Result> vuelos = db.uspRecuperarVuelos().ToList();
            List<uspRecuperarVuelos_Result> resultado = new List<uspRecuperarVuelos_Result>();

            DateTime? departure_date;

            if (busqueda.fecha_inicio != null)
            {
                departure_date = DateTime.ParseExact(busqueda.fecha_inicio, "yyyy'-'MM'-'dd", null);
            } else
            {
                departure_date = null;
            }
            

            foreach(var vuelo in vuelos)
            {
                DateTime flight_departure_date = DateTime.Parse(c.desencriptar(vuelo.fecha_partida));

                if (busqueda.pais != null && busqueda.fecha_inicio != null)
                {
                    if (busqueda.pais == vuelo.codigo_pais_parte && departure_date == flight_departure_date)
                    {
                        resultado.Add(vuelo);
                    } else
                    {
                        continue;
                    }
                } else if (busqueda.pais != null)
                {
                    if (busqueda.pais == vuelo.codigo_pais_parte)
                    {
                        resultado.Add(vuelo);
                    } else
                    {
                        continue;
                    }
                } else if (busqueda.fecha_inicio != null)
                {
                    if (departure_date == flight_departure_date)
                    {
                        resultado.Add(vuelo);
                    }
                    else
                    {
                        continue;
                    }
                } else
                {
                    return Ok(new {error = "No se pasaron argumentos para la búsqueda."});
                }

                
            }

            foreach (var vuelo in resultado)
            {
                vuelo.fecha_partida = c.desencriptar(vuelo.fecha_partida);
                vuelo.hora_partida = c.desencriptar(vuelo.hora_partida);
                vuelo.fecha_llegada = c.desencriptar(vuelo.fecha_llegada);
                vuelo.hora_llegada = c.desencriptar(vuelo.hora_llegada);
                vuelo.nombre_aerolinea = c.desencriptar(vuelo.nombre_aerolinea);
                vuelo.numero_puerta = c.desencriptar(vuelo.numero_puerta);
                vuelo.descripcion_estado = c.desencriptar(vuelo.descripcion_estado);
                vuelo.nombre_pais_parte = c.desencriptar(vuelo.nombre_pais_parte);
                vuelo.nombre_pais_llega = c.desencriptar(vuelo.nombre_pais_llega);
                vuelo.precio = c.desencriptar(vuelo.precio);

            }

            return Ok(resultado);
        }
    }
}
