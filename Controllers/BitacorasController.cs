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
    public class BitacorasController : ApiController
    {
        private V_Vuelos_Main_NotEncryptedEntities db;
        Crypt c;
        public BitacorasController()
        {
            db = new V_Vuelos_Main_NotEncryptedEntities();
            c = new Crypt();
        }

        // GET: api/Bitacoras
        public IEnumerable<Bitacora> GetBitacora()
        {
            IEnumerable<Bitacora> resultado = db.Bitacora;

            foreach (var bitacora in resultado)
            {
                bitacora.fecha = c.desencriptar(bitacora.fecha);
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

            bitacora.fecha = c.desencriptar(bitacora.fecha);
            bitacora.hora = c.desencriptar(bitacora.hora);
            bitacora.registro_detalle = c.desencriptar(bitacora.registro_detalle);
            bitacora.descripcion = c.desencriptar(bitacora.descripcion);

            return Ok(bitacora);
        }

        // PUT: api/Bitacoras/5
        [ResponseType(typeof(void))]
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

            bitacora.fecha = c.encriptar(bitacora.fecha);
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
        public IHttpActionResult PostBitacora(Bitacora bitacora)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bitacora.fecha = c.encriptar(bitacora.fecha);
            bitacora.hora = c.encriptar(bitacora.hora);
            bitacora.registro_detalle = c.encriptar(bitacora.registro_detalle);
            bitacora.descripcion = c.encriptar(bitacora.descripcion);

            db.Bitacora.Add(bitacora);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = bitacora.id }, bitacora);
        }

        // DELETE: api/Bitacoras/5
        [ResponseType(typeof(Bitacora))]
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