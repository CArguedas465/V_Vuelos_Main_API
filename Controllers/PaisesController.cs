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
    public class PaisesController : ApiController
    {
        private V_Vuelos_MainEntities db;
        Crypt c;
        public PaisesController()
        {
            db = new V_Vuelos_MainEntities();
            c = new Crypt();
        }

        // GET: api/Paises
        public IEnumerable<Pais> GetPais()
        {
            IEnumerable<Pais> resultado = db.Pais;

            foreach (var pais in resultado)
            {
                pais.nombre = c.desencriptar(pais.nombre);
            }

            return resultado;
        }

        // GET: api/Paises/5
        [ResponseType(typeof(Pais))]
        public IHttpActionResult GetPais(string id)
        {
            Pais pais = db.Pais.Find(id);
            if (pais == null)
            {
                return NotFound();
            }

            pais.nombre = c.desencriptar(pais.nombre);

            return Ok(pais);
        }

        // PUT: api/Paises/5
        [ResponseType(typeof(void))]
        [DisableCors]
        public IHttpActionResult PutPais(string id, Pais pais)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pais.id)
            {
                return BadRequest();
            }

            pais.nombre = c.encriptar(pais.nombre);
            db.Entry(pais).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaisExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(pais);
        }

        // POST: api/Paises
        [ResponseType(typeof(Pais))]
        [DisableCors]
        public IHttpActionResult PostPais(Pais pais)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Consecutivo consecutivo = db.Consecutivo.Find(1);

            pais.id = c.desencriptar(consecutivo.prefijo) + "-" + c.desencriptar(consecutivo.valor);
            pais.nombre = c.encriptar(pais.nombre);

            int valor = Convert.ToInt32(c.desencriptar(consecutivo.valor));
            valor++;
            consecutivo.valor = c.encriptar(valor.ToString());
            db.Pais.Add(pais);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PaisExists(pais.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = pais.id }, pais);
        }

        // DELETE: api/Paises/5
        [ResponseType(typeof(Pais))]
        [DisableCors]
        public IHttpActionResult DeletePais(string id)
        {
            Pais pais = db.Pais.Find(id);
            if (pais == null)
            {
                return NotFound();
            }

            db.Pais.Remove(pais);
            db.SaveChanges();

            return Ok(pais);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PaisExists(string id)
        {
            return db.Pais.Count(e => e.id == id) > 0;
        }
    }
}