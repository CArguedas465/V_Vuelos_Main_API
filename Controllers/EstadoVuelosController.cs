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
    public class EstadoVuelosController : ApiController
    {
        V_Vuelos_MainEntities db;
        Crypt c;

        public EstadoVuelosController()
        {
            db = new V_Vuelos_MainEntities();
            c = new Crypt();
        }

        // GET: api/EstadoVueloes
        public IEnumerable<EstadoVuelo> GetEstadoVuelo()
        {
            IEnumerable<EstadoVuelo> resultado = db.EstadoVuelo;

            foreach (var estado in resultado)
            {
                estado.descripcion = c.desencriptar(estado.descripcion);
            }

            return resultado;
        }

        // GET: api/EstadoVueloes/5
        [ResponseType(typeof(EstadoVuelo))]
        public IHttpActionResult GetEstadoVuelo(decimal id)
        {
            EstadoVuelo estadoVuelo = db.EstadoVuelo.Find(id);
            if (estadoVuelo == null)
            {
                return NotFound();
            }

            estadoVuelo.descripcion = c.desencriptar(estadoVuelo.descripcion);

            return Ok(estadoVuelo);
        }

        // PUT: api/EstadoVueloes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEstadoVuelo(decimal id, EstadoVuelo estadoVuelo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != estadoVuelo.id)
            {
                return BadRequest();
            }

            estadoVuelo.descripcion = c.encriptar(estadoVuelo.descripcion);
            db.Entry(estadoVuelo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadoVueloExists(id))
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

        // POST: api/EstadoVueloes
        [ResponseType(typeof(EstadoVuelo))]
        public IHttpActionResult PostEstadoVuelo(EstadoVuelo estadoVuelo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            estadoVuelo.descripcion = c.encriptar(estadoVuelo.descripcion);

            db.EstadoVuelo.Add(estadoVuelo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = estadoVuelo.id }, estadoVuelo);
        }

        // DELETE: api/EstadoVueloes/5
        [ResponseType(typeof(EstadoVuelo))]
        public IHttpActionResult DeleteEstadoVuelo(decimal id)
        {
            EstadoVuelo estadoVuelo = db.EstadoVuelo.Find(id);
            if (estadoVuelo == null)
            {
                return NotFound();
            }

            db.EstadoVuelo.Remove(estadoVuelo);
            db.SaveChanges();

            return Ok(estadoVuelo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EstadoVueloExists(decimal id)
        {
            return db.EstadoVuelo.Count(e => e.id == id) > 0;
        }
    }
}