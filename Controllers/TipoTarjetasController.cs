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
using V_Vuelos_Main_API.Models;
using V_Vuelos_Main_API.Crypto;

namespace V_Vuelos_Main_API.Controllers
{
    public class TipoTarjetasController : ApiController
    {
        V_Vuelos_MainEntities db;
        Crypt c;

        public TipoTarjetasController()
        {
            db = new V_Vuelos_MainEntities();
            c = new Crypt();
        }

        // GET: api/TipoTarjetas
        public IEnumerable<TipoTarjeta> GetTipoTarjeta()
        {
            IEnumerable<TipoTarjeta> resultado = db.TipoTarjeta;

            foreach (var tipoTarjeta in resultado)
            {
                tipoTarjeta.descripcion = c.desencriptar(tipoTarjeta.descripcion);
            }

            return resultado;
        }

        // GET: api/TipoTarjetas/5
        [ResponseType(typeof(TipoTarjeta))]
        public IHttpActionResult GetTipoTarjeta(decimal id)
        {
            TipoTarjeta tipoTarjeta = db.TipoTarjeta.Find(id);
            if (tipoTarjeta == null)
            {
                return NotFound();
            }

            tipoTarjeta.descripcion = c.desencriptar(tipoTarjeta.descripcion);

            return Ok(tipoTarjeta);
        }

        // PUT: api/TipoTarjetas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTipoTarjeta(decimal id, TipoTarjeta tipoTarjeta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipoTarjeta.id)
            {
                return BadRequest();
            }

            tipoTarjeta.descripcion = c.encriptar(tipoTarjeta.descripcion);
            db.Entry(tipoTarjeta).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoTarjetaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.Accepted);
        }

        // POST: api/TipoTarjetas
        [ResponseType(typeof(TipoTarjeta))]
        public IHttpActionResult PostTipoTarjeta(TipoTarjeta tipoTarjeta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            tipoTarjeta.descripcion = c.encriptar(tipoTarjeta.descripcion);

            db.TipoTarjeta.Add(tipoTarjeta);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tipoTarjeta.id }, tipoTarjeta);
        }

        // DELETE: api/TipoTarjetas/5
        [ResponseType(typeof(TipoTarjeta))]
        public IHttpActionResult DeleteTipoTarjeta(decimal id)
        {
            TipoTarjeta tipoTarjeta = db.TipoTarjeta.Find(id);
            if (tipoTarjeta == null)
            {
                return NotFound();
            }

            db.TipoTarjeta.Remove(tipoTarjeta);
            db.SaveChanges();

            return Ok(tipoTarjeta);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TipoTarjetaExists(decimal id)
        {
            return db.TipoTarjeta.Count(e => e.id == id) > 0;
        }
    }
}