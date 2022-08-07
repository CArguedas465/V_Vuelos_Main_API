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
    public class VuelosEntrantesSalientesController : ApiController
    {
        private V_Vuelos_MainEntities db;
        Crypt c;
        public VuelosEntrantesSalientesController()
        {
            db = new V_Vuelos_MainEntities();
            c = new Crypt();
        }

        // GET: api/VuelosDescriptivos/1 o 0
        public List<uspRecuperarVuelos_EntranteSaliente_Result> GetVuelo(decimal id)
        {
            bool? saliente = id == 1 ? true : false;

            List<uspRecuperarVuelos_EntranteSaliente_Result> resultado = db.uspRecuperarVuelos_EntranteSaliente(saliente).ToList();

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
            }

            return resultado;
        }
    }
}
