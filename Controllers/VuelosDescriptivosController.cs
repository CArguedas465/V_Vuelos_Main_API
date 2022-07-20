using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using V_Vuelos_Main_API.Crypto;
using V_Vuelos_Main_API.Models;

namespace V_Vuelos_Main_API.Controllers
{
    public class VuelosDescriptivosController : ApiController
    {
        private V_Vuelos_Main_NotEncryptedEntities db;
        Crypt c;
        public VuelosDescriptivosController()
        {
            db = new V_Vuelos_Main_NotEncryptedEntities();
            c = new Crypt();
        }

        // GET: api/VuelosDescriptivos
        public  List<uspRecuperarVuelos_Result> GetVuelo()
        {
            List<uspRecuperarVuelos_Result> resultado = db.uspRecuperarVuelos().ToList();

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