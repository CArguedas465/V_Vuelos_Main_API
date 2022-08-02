using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
    public class TipoTransaccionesController : ApiController
    {
        V_Vuelos_MainEntities db;
        Crypt c;

        public TipoTransaccionesController()
        {
            db = new V_Vuelos_MainEntities();
            c = new Crypt();
        }

        // GET: api/TipoTransaccions
        public IEnumerable<TipoTransaccion> GetTipoTransaccion()
        {
            IEnumerable<TipoTransaccion> resultado = db.TipoTransaccion;

            foreach (var tipoTransaccion in resultado)
            {
                tipoTransaccion.descripcion = c.desencriptar(tipoTransaccion.descripcion);
            }

            return resultado;
        }

        // GET: api/TipoTransaccions/5
        [ResponseType(typeof(TipoTransaccion))]
        public IHttpActionResult GetTipoTransaccion(decimal id)
        {
            TipoTransaccion tipoTransaccion = db.TipoTransaccion.Find(id);
            if (tipoTransaccion == null)
            {
                return NotFound();
            }

            tipoTransaccion.descripcion = c.desencriptar(tipoTransaccion.descripcion);

            return Ok(tipoTransaccion);
        }

        // PUT: api/TipoTransaccions/5
        [ResponseType(typeof(void))]
        [DisableCors]
        public IHttpActionResult PutTipoTransaccion(decimal id, TipoTransaccion tipoTransaccion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipoTransaccion.id)
            {
                return BadRequest();
            }

            tipoTransaccion.descripcion = c.encriptar(tipoTransaccion.descripcion);
            db.Entry(tipoTransaccion).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoTransaccionExists(id))
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

        // POST: api/TipoTransaccions
        [ResponseType(typeof(TipoTransaccion))]
        [DisableCors]
        public IHttpActionResult PostTipoTransaccion(TipoTransaccion tipoTransaccion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            tipoTransaccion.descripcion = c.encriptar(tipoTransaccion.descripcion);

            db.TipoTransaccion.Add(tipoTransaccion);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tipoTransaccion.id }, tipoTransaccion);
        }

        // DELETE: api/TipoTransaccions/5
        [ResponseType(typeof(TipoTransaccion))]
        [DisableCors]
        public IHttpActionResult DeleteTipoTransaccion(decimal id)
        {
            TipoTransaccion tipoTransaccion = db.TipoTransaccion.Find(id);
            if (tipoTransaccion == null)
            {
                return NotFound();
            }

            db.TipoTransaccion.Remove(tipoTransaccion);
            db.SaveChanges();

            return Ok(tipoTransaccion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TipoTransaccionExists(decimal id)
        {
            return db.TipoTransaccion.Count(e => e.id == id) > 0;
        }
    }
}