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
    public class OperacionesController : ApiController
    {
        V_Vuelos_Main_NotEncryptedEntities db;
        Crypt c;

        public OperacionesController()
        {
            db = new V_Vuelos_Main_NotEncryptedEntities();
            c = new Crypt();
        }

        // GET: api/Operaciones
        public IEnumerable<Operacion> GetOperacion()
        {
            IEnumerable<Operacion> resultado = db.Operacion;

            foreach (var operacion in resultado)
            {
                operacion.descripcion = c.desencriptar(operacion.descripcion);
            }

            return resultado;
        }

        // GET: api/Operaciones/5
        [ResponseType(typeof(Operacion))]
        public IHttpActionResult GetOperacion(decimal id)
        {
            Operacion operacion = db.Operacion.Find(id);
            if (operacion == null)
            {
                return NotFound();
            }

            operacion.descripcion = c.desencriptar(operacion.descripcion);

            return Ok(operacion);
        }

        // PUT: api/Operaciones/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOperacion(decimal id, Operacion operacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != operacion.id)
            {
                return BadRequest();
            }

            operacion.descripcion = c.encriptar(operacion.descripcion);
            db.Entry(operacion).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OperacionExists(id))
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

        // POST: api/Operaciones
        [ResponseType(typeof(Operacion))]
        public IHttpActionResult PostOperacion(Operacion operacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            operacion.descripcion = c.encriptar(operacion.descripcion);

            db.Operacion.Add(operacion);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = operacion.id }, operacion);
        }

        // DELETE: api/Operaciones/5
        [ResponseType(typeof(Operacion))]
        public IHttpActionResult DeleteOperacion(decimal id)
        {
            Operacion operacion = db.Operacion.Find(id);
            if (operacion == null)
            {
                return NotFound();
            }

            db.Operacion.Remove(operacion);
            db.SaveChanges();

            return Ok(operacion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OperacionExists(decimal id)
        {
            return db.Operacion.Count(e => e.id == id) > 0;
        }
    }
}