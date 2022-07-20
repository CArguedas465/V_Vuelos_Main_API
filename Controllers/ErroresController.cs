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
    public class ErroresController : ApiController
    {
        private V_Vuelos_Main_NotEncryptedEntities db;
        Crypt c;
        public ErroresController()
        {
            db = new V_Vuelos_Main_NotEncryptedEntities();
            c = new Crypt();
        }

        // GET: api/Errores
        public IEnumerable<Error> GetError()
        {
            IEnumerable<Error> resultado = db.Error;

            foreach (var error in resultado)
            {
                error.fecha = c.desencriptar(error.fecha);
                error.hora = c.desencriptar(error.hora);
                error.numero_error = c.desencriptar(error.numero_error);
                error.mensaje_error = c.desencriptar(error.mensaje_error);
            }

            return resultado;
        }

        // GET: api/Errores/5
        [ResponseType(typeof(Error))]
        public IHttpActionResult GetError(decimal id)
        {
            Error error = db.Error.Find(id);
            if (error == null)
            {
                return NotFound();
            }

            error.fecha = c.desencriptar(error.fecha);
            error.hora = c.desencriptar(error.hora);
            error.numero_error = c.desencriptar(error.numero_error);
            error.mensaje_error = c.desencriptar(error.mensaje_error);

            return Ok(error);
        }

        // PUT: api/Errores/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutError(decimal id, Error error)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != error.id)
            {
                return BadRequest();
            }

            error.fecha = c.encriptar(error.fecha);
            error.hora = c.encriptar(error.hora);
            error.numero_error = c.encriptar(error.numero_error);
            error.mensaje_error = c.encriptar(error.mensaje_error);

            db.Entry(error).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ErrorExists(id))
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

        // POST: api/Errores
        [ResponseType(typeof(Error))]
        public IHttpActionResult PostError(Error error)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            error.fecha = c.encriptar(error.fecha);
            error.hora = c.encriptar(error.hora);
            error.numero_error = c.encriptar(error.numero_error);
            error.mensaje_error = c.encriptar(error.mensaje_error);

            db.Error.Add(error);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = error.id }, error);
        }

        // DELETE: api/Errores/5
        [ResponseType(typeof(Error))]
        public IHttpActionResult DeleteError(decimal id)
        {
            Error error = db.Error.Find(id);
            if (error == null)
            {
                return NotFound();
            }

            db.Error.Remove(error);
            db.SaveChanges();

            return Ok(error);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ErrorExists(decimal id)
        {
            return db.Error.Count(e => e.id == id) > 0;
        }
    }
}