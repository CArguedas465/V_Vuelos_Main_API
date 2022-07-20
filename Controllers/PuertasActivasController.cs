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
    public class PuertasActivasController : ApiController
    {
        private V_Vuelos_Main_NotEncryptedEntities db;
        Crypt c;
        public PuertasActivasController()
        {
            db = new V_Vuelos_Main_NotEncryptedEntities();
            c = new Crypt();
        }

        // GET: api/PuertasActivas
        public List<uspRecuperarPuertasActivas_Result> GetPuertasActivas()
        {
            List<uspRecuperarPuertasActivas_Result> resultado = db.uspRecuperarPuertasActivas().ToList();

            foreach (var puertaActiva in resultado)
            {
                puertaActiva.numero = c.desencriptar(puertaActiva.numero);
                puertaActiva.DescripcionEstado = c.desencriptar(puertaActiva.DescripcionEstado);
            }

            return resultado;
        }
    }
}