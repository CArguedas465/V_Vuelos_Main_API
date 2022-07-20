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
    public class ConsecutivosController : ApiController
    {
        private V_Vuelos_Main_NotEncryptedEntities db;
        Crypt c;
        public ConsecutivosController()
        {
            db = new V_Vuelos_Main_NotEncryptedEntities();
            c = new Crypt();
        }

        // GET: api/Consecutivos
        public IEnumerable<Consecutivo> GetConsecutivo()
        {
            IEnumerable<Consecutivo> resultado = db.Consecutivo;

            foreach (var consecutivo in resultado)
            {
                consecutivo.valor = c.desencriptar(consecutivo.valor);
                consecutivo.descripcion = c.desencriptar(consecutivo.descripcion);
                consecutivo.prefijo = c.desencriptar(consecutivo.prefijo);

                if (consecutivo.rango_inicial != null)
                {
                    consecutivo.rango_inicial = c.desencriptar(consecutivo.rango_inicial);
                }
                if (consecutivo.rango_final != null)
                {
                    consecutivo.rango_final = c.desencriptar(consecutivo.rango_final);
                }
            }

            return resultado;
        }

        // GET: api/Consecutivos/5
        [ResponseType(typeof(Consecutivo))]
        public IHttpActionResult GetConsecutivo(decimal id)
        {
            Consecutivo consecutivo = db.Consecutivo.Find(id);
            if (consecutivo == null)
            {
                return NotFound();
            }

            consecutivo.valor = c.desencriptar(consecutivo.valor);
            consecutivo.descripcion = c.desencriptar(consecutivo.descripcion);
            consecutivo.prefijo = c.desencriptar(consecutivo.prefijo);

            if (consecutivo.rango_inicial != null)
            {
                consecutivo.rango_inicial = c.desencriptar(consecutivo.rango_inicial);
            }
            if (consecutivo.rango_final != null)
            {
                consecutivo.rango_final = c.desencriptar(consecutivo.rango_final);
            }

            return Ok(consecutivo);
        }

        // PUT: api/Consecutivos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutConsecutivo(decimal id, Consecutivo consecutivo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != consecutivo.id)
            {
                return BadRequest();
            }

            consecutivo.valor = c.encriptar(consecutivo.valor);
            consecutivo.descripcion = c.encriptar(consecutivo.descripcion);
            consecutivo.prefijo = c.encriptar(consecutivo.prefijo);
            if (consecutivo.rango_inicial != null)
            {
                consecutivo.rango_inicial = c.encriptar(consecutivo.rango_inicial);
            }
            if (consecutivo.rango_final != null)
            {
                consecutivo.rango_final = c.encriptar(consecutivo.rango_final);
            }
            db.Entry(consecutivo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConsecutivoExists(id))
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

        // POST: api/Consecutivos
        [ResponseType(typeof(Consecutivo))]
        public IHttpActionResult PostConsecutivo(Consecutivo consecutivo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            consecutivo.valor = c.encriptar(consecutivo.valor);
            consecutivo.descripcion = c.encriptar(consecutivo.descripcion);
            consecutivo.prefijo = c.encriptar(consecutivo.prefijo);

            if (consecutivo.rango_inicial != null)
            {
                consecutivo.rango_inicial = c.encriptar(consecutivo.rango_inicial);
            }
            if (consecutivo.rango_final != null)
            {
                consecutivo.rango_final = c.encriptar(consecutivo.rango_final);
            }
            
            

            db.Consecutivo.Add(consecutivo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = consecutivo.id }, consecutivo);
        }

        // DELETE: api/Consecutivos/5
        [ResponseType(typeof(Consecutivo))]
        public IHttpActionResult DeleteConsecutivo(decimal id)
        {
            Consecutivo consecutivo = db.Consecutivo.Find(id);
            if (consecutivo == null)
            {
                return NotFound();
            }

            db.Consecutivo.Remove(consecutivo);
            db.SaveChanges();

            return Ok(consecutivo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ConsecutivoExists(decimal id)
        {
            return db.Consecutivo.Count(e => e.id == id) > 0;
        }
    }
}