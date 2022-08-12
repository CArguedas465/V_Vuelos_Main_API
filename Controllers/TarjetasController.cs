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
    public class TarjetasController : ApiController
    {
        private V_Vuelos_MainEntities db;
        Crypt c;
        public TarjetasController()
        {
            db = new V_Vuelos_MainEntities();
            c = new Crypt();
        }

        // GET: api/Tarjetas
        public IEnumerable<Tarjeta> GetTarjeta()
        {
            IEnumerable<Tarjeta> resultado = db.Tarjeta;

            foreach (var tarjeta in resultado)
            {
                tarjeta.numero_tarjeta = c.desencriptar(tarjeta.numero_tarjeta);
                tarjeta.fecha_expiracion = c.desencriptar(tarjeta.fecha_expiracion);
                tarjeta.cvv = c.desencriptar(tarjeta.cvv);
            }

            return resultado;
        }

        // GET: api/Tarjetas/5
        [ResponseType(typeof(Tarjeta))]
        public IHttpActionResult GetTarjeta(string id)
        {
            Tarjeta tarjeta = db.Tarjeta.Find(id);
            if (tarjeta == null)
            {
                return NotFound();
            }

            tarjeta.numero_tarjeta = c.desencriptar(tarjeta.numero_tarjeta);
            tarjeta.fecha_expiracion = c.desencriptar(tarjeta.fecha_expiracion);
            tarjeta.cvv = c.desencriptar(tarjeta.cvv);

            return Ok(tarjeta);
        }

        // PUT: api/Tarjetas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTarjeta(string id, Tarjeta tarjeta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tarjeta.numero_tarjeta)
            {
                return BadRequest();
            }

            tarjeta.numero_tarjeta = c.encriptar(tarjeta.numero_tarjeta);
            tarjeta.fecha_expiracion = c.encriptar(tarjeta.fecha_expiracion);
            tarjeta.cvv = c.encriptar(tarjeta.cvv);

            db.Entry(tarjeta).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TarjetaExists(id))
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

        // POST: api/Tarjetas
        [ResponseType(typeof(Tarjeta))]
        public IHttpActionResult PostTarjeta(Tarjeta tarjeta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Tarjeta tarjetaDesc = new Tarjeta()
            {
                numero_tarjeta = tarjeta.numero_tarjeta,
                fecha_expiracion = tarjeta.fecha_expiracion,
                tipo_tarjeta = tarjeta.tipo_tarjeta,
                cliente = tarjeta.cliente,
                cvv = tarjeta.cvv
            };

            tarjeta.numero_tarjeta = c.encriptar(tarjeta.numero_tarjeta);
            tarjeta.fecha_expiracion = c.encriptar(tarjeta.fecha_expiracion);
            tarjeta.cvv = c.encriptar(tarjeta.cvv);

            db.Tarjeta.Add(tarjeta);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (TarjetaExists(tarjeta.numero_tarjeta))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", tarjetaDesc, tarjeta);
        }

        // DELETE: api/Tarjetas/5
        [ResponseType(typeof(Tarjeta))]
        public IHttpActionResult DeleteTarjeta(string id)
        {
            Tarjeta tarjeta = db.Tarjeta.Find(id);
            if (tarjeta == null)
            {
                return NotFound();
            }

            db.Tarjeta.Remove(tarjeta);
            db.SaveChanges();

            return Ok(tarjeta);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TarjetaExists(string id)
        {
            return db.Tarjeta.Count(e => e.numero_tarjeta == id) > 0;
        }
    }
}