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
    public class AerolineasController : ApiController
    {
        private V_Vuelos_MainEntities db;
        Crypt c;
        public AerolineasController()
        {
            db = new V_Vuelos_MainEntities();
            c = new Crypt();
        }

        // GET: api/Aerolineas
        public IEnumerable<Aerolinea> GetAerolinea()
        {
            IEnumerable<Aerolinea> resultado = db.Aerolinea;

            foreach (var aerolinea in resultado)
            {
                aerolinea.nombre = c.desencriptar(aerolinea.nombre);
            }

            return resultado;
        }

        // GET: api/Aerolineas/5
        [ResponseType(typeof(Aerolinea))]
        public IHttpActionResult GetAerolinea(string id)
        {
            Aerolinea aerolinea = db.Aerolinea.Find(id);
            if (aerolinea == null)
            {
                return NotFound();
            }

            aerolinea.nombre = c.desencriptar(aerolinea.nombre);

            return Ok(aerolinea);
        }

        // PUT: api/Aerolineas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAerolinea(string id, Aerolinea aerolinea)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != aerolinea.id)
            {
                return BadRequest();
            }

            aerolinea.nombre = c.encriptar(aerolinea.nombre);
            db.Entry(aerolinea).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AerolineaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Aerolineas
        [ResponseType(typeof(Aerolinea))]
        public IHttpActionResult PostAerolinea(Aerolinea aerolinea)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Consecutivo consecutivo = db.Consecutivo.Find(2);

            aerolinea.id = c.desencriptar(consecutivo.prefijo) + "-" + c.desencriptar(consecutivo.valor);
            aerolinea.nombre = c.encriptar(aerolinea.nombre);

            int valor = Convert.ToInt32(c.desencriptar(consecutivo.valor));
            valor++;
            consecutivo.valor = c.encriptar(valor.ToString());

            db.Aerolinea.Add(aerolinea);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AerolineaExists(aerolinea.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(CreatedAtRoute("DefaultApi", new { id = aerolinea.id }, aerolinea));
        }

        // DELETE: api/Aerolineas/5
        [ResponseType(typeof(Aerolinea))]
        public IHttpActionResult DeleteAerolinea(string id)
        {
            Aerolinea aerolinea = db.Aerolinea.Find(id);
            if (aerolinea == null)
            {
                return NotFound();
            }

            db.Aerolinea.Remove(aerolinea);
            db.SaveChanges();

            return Ok(aerolinea);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AerolineaExists(string id)
        {
            return db.Aerolinea.Count(e => e.id == id) > 0;
        }
    }
}