using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using V_Vuelos_Main_API.Crypto;
using V_Vuelos_Main_API.Models;

namespace V_Vuelos_Main_API.Controllers
{
    public class BitacorasController : ApiController
    {
        private V_Vuelos_MainEntities db;
        Crypt c;
        public BitacorasController()
        {
            db = new V_Vuelos_MainEntities();
            c = new Crypt();
        }

        // GET: api/Bitacoras
        public IEnumerable<Bitacora> GetBitacora()
        {
            IEnumerable<Bitacora> resultado = db.Bitacora;

            foreach (var bitacora in resultado)
            {
                bitacora.hora = c.desencriptar(bitacora.hora);
                bitacora.registro_detalle = c.desencriptar(bitacora.registro_detalle);
                bitacora.descripcion = c.desencriptar(bitacora.descripcion);
            }

            return resultado;
        }

        // GET: api/Bitacoras/5
        [ResponseType(typeof(Bitacora))]
        public IHttpActionResult GetBitacora(decimal id)
        {
            Bitacora bitacora = db.Bitacora.Find(id);
            if (bitacora == null)
            {
                return NotFound();
            }

            bitacora.hora = c.desencriptar(bitacora.hora);
            bitacora.registro_detalle = c.desencriptar(bitacora.registro_detalle);
            bitacora.descripcion = c.desencriptar(bitacora.descripcion);

            return Ok(bitacora);
        }

        // PUT: api/Bitacoras/5
        [ResponseType(typeof(void))]
        [DisableCors]
        public IHttpActionResult PutBitacora(decimal id, Bitacora bitacora)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bitacora.id)
            {
                return BadRequest();
            }

            bitacora.hora = c.encriptar(bitacora.hora);
            bitacora.registro_detalle = c.encriptar(bitacora.registro_detalle);
            bitacora.descripcion = c.encriptar(bitacora.descripcion);

            db.Entry(bitacora).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BitacoraExists(id))
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

        // POST: api/Bitacoras
        [ResponseType(typeof(Bitacora))]
        [DisableCors]
        public IHttpActionResult PostBitacora(Bitacora bitacora)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bitacora.hora = c.encriptar(bitacora.hora);
            bitacora.registro_detalle = c.encriptar(bitacora.registro_detalle);
            bitacora.descripcion = c.encriptar(bitacora.descripcion);

           
            try 
            {
                db.Bitacora.Add(bitacora);
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var errors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in errors.ValidationErrors)
                    {
                        // get the error message 
                        string errorMessage = validationError.ErrorMessage;
                    }
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = bitacora.id }, bitacora);
        }

        // DELETE: api/Bitacoras/5
        [ResponseType(typeof(Bitacora))]
        [DisableCors]
        public IHttpActionResult DeleteBitacora(decimal id)
        {
            Bitacora bitacora = db.Bitacora.Find(id);
            if (bitacora == null)
            {
                return NotFound();
            }

            db.Bitacora.Remove(bitacora);
            db.SaveChanges();

            return Ok(bitacora);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BitacoraExists(decimal id)
        {
            return db.Bitacora.Count(e => e.id == id) > 0;
        }
    }
}