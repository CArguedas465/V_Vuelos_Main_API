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
    public class PuertasController : ApiController
    {
        private V_Vuelos_MainEntities db;
        Crypt c;
        public PuertasController()
        {
            db = new V_Vuelos_MainEntities();
            c = new Crypt();
        }

        // GET: api/Puertas
        public IEnumerable<Puerta> GetPuerta()
        {
            IEnumerable<Puerta> resultado = db.Puerta;

            foreach (var puerta in resultado)
            {
                puerta.numero = c.desencriptar(puerta.numero);
            }

            return resultado;
        }

        // GET: api/Puertas/5
        [ResponseType(typeof(Puerta))]
        public IHttpActionResult GetPuerta(string id)
        {
            Puerta puerta = db.Puerta.Find(id);
            if (puerta == null)
            {
                return NotFound();
            }

            puerta.numero = c.desencriptar(puerta.numero);

            return Ok(puerta);
        }

        // PUT: api/Puertas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPuerta(string id, Puerta puerta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != puerta.id)
            {
                return BadRequest();
            }

            puerta.numero = c.encriptar(puerta.numero);
            db.Entry(puerta).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PuertaExists(id))
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

        // POST: api/Puertas
        [ResponseType(typeof(Puerta))]
        public IHttpActionResult PostPuerta(Puerta puerta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Consecutivo consecutivo = db.Consecutivo.Find(3);

            puerta.id = c.desencriptar(consecutivo.prefijo) + "-" + c.desencriptar(consecutivo.valor);
            puerta.numero = c.encriptar(puerta.numero);

            int valor = Convert.ToInt32(c.desencriptar(consecutivo.valor));
            valor++;
            consecutivo.valor = c.encriptar(valor.ToString());
            db.Puerta.Add(puerta);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PuertaExists(puerta.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = puerta.id }, puerta);
        }

        // DELETE: api/Puertas/5
        [ResponseType(typeof(Puerta))]
        public IHttpActionResult DeletePuerta(string id)
        {
            Puerta puerta = db.Puerta.Find(id);
            if (puerta == null)
            {
                return NotFound();
            }

            db.Puerta.Remove(puerta);
            db.SaveChanges();

            return Ok(puerta);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PuertaExists(string id)
        {
            return db.Puerta.Count(e => e.id == id) > 0;
        }
    }
}