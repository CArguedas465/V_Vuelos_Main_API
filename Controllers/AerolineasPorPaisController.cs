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
    public class AerolineasPorPaisController : ApiController
    {
        private V_Vuelos_MainEntities db;
        Crypt c;
        public AerolineasPorPaisController()
        {
            db = new V_Vuelos_MainEntities();
            c = new Crypt();
        }

        // GET: api/AerolineasPorPais
        public List<uspRecuperarAerolineaPorPais_Result> GetAerolinea(string id)
        {
            List<uspRecuperarAerolineaPorPais_Result> resultado = db.uspRecuperarAerolineaPorPais(id).ToList();

            foreach (var aerolinea in resultado)
            {
                aerolinea.nombre = c.desencriptar(aerolinea.nombre);
                aerolinea.NombrePais = c.desencriptar(aerolinea.NombrePais);
            }

            return resultado;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}