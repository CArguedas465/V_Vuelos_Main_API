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
    public class EstadoPuertasController : ApiController
    {

        V_Vuelos_MainEntities db;
        Crypt c;

        public EstadoPuertasController()
        {
            db = new V_Vuelos_MainEntities();
            c = new Crypt();
        }

        // GET: api/EstadoPuertas
        public IEnumerable<EstadoPuerta> GetEstadoPuerta()
        {
            IEnumerable<EstadoPuerta> resultado = db.EstadoPuerta;

            foreach (var estado in resultado)
            {
                estado.descripcion = c.desencriptar(estado.descripcion);
            }

            return resultado;
        }

        // GET: api/EstadoPuertas/5
        [ResponseType(typeof(EstadoPuerta))]
        public IHttpActionResult GetEstadoPuerta(decimal id)
        {
            EstadoPuerta estadoPuerta = db.EstadoPuerta.Find(id);
            if (estadoPuerta == null)
            {
                return NotFound();
            }

            estadoPuerta.descripcion = c.desencriptar(estadoPuerta.descripcion);

            return Ok(estadoPuerta);
        }

        // PUT: api/EstadoPuertas/5
        [ResponseType(typeof(void))]
        [DisableCors]
        public IHttpActionResult PutEstadoPuerta(decimal id, EstadoPuerta estadoPuerta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != estadoPuerta.id)
            {
                return BadRequest();
            }

            estadoPuerta.descripcion = c.encriptar(estadoPuerta.descripcion);

            db.Entry(estadoPuerta).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadoPuertaExists(id))
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

        // POST: api/EstadoPuertas
        [ResponseType(typeof(EstadoPuerta))]
        [DisableCors]
        public IHttpActionResult PostEstadoPuerta(EstadoPuerta estadoPuerta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            estadoPuerta.descripcion = c.encriptar(estadoPuerta.descripcion);

            db.EstadoPuerta.Add(estadoPuerta);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = estadoPuerta.id }, estadoPuerta);
        }

        // DELETE: api/EstadoPuertas/5
        [ResponseType(typeof(EstadoPuerta))]
        [DisableCors]
        public IHttpActionResult DeleteEstadoPuerta(decimal id)
        {
            EstadoPuerta estadoPuerta = db.EstadoPuerta.Find(id);
            if (estadoPuerta == null)
            {
                return NotFound();
            }

            db.EstadoPuerta.Remove(estadoPuerta);
            db.SaveChanges();

            return Ok(estadoPuerta);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EstadoPuertaExists(decimal id)
        {
            return db.EstadoPuerta.Count(e => e.id == id) > 0;
        }
    }
}