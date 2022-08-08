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
    public class FormaPagosController : ApiController
    {
        V_Vuelos_MainEntities db;
        Crypt c;

        public FormaPagosController()
        {
            db = new V_Vuelos_MainEntities();
            c = new Crypt();
        }

        // GET: api/FormaPagos
        public IEnumerable<FormaPago> GetFormaPago()
        {
            IEnumerable<FormaPago> resultado = db.FormaPago;

            foreach (var formaPago in resultado)
            {
                formaPago.descripcion = c.desencriptar(formaPago.descripcion);
            }

            return resultado;
        }

        // GET: api/FormaPagos/5
        [ResponseType(typeof(FormaPago))]
        public IHttpActionResult GetFormaPago(decimal id)
        {
            FormaPago formaPago = db.FormaPago.Find(id);
            if (formaPago == null)
            {
                return NotFound();
            }

            formaPago.descripcion = c.desencriptar(formaPago.descripcion);

            return Ok(formaPago);
        }

        // PUT: api/FormaPagos/5
        [ResponseType(typeof(void))]
        [DisableCors]
        public IHttpActionResult PutFormaPago(decimal id, FormaPago formaPago)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != formaPago.id)
            {
                return BadRequest();
            }

            formaPago.descripcion = c.encriptar(formaPago.descripcion);
            db.Entry(formaPago).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormaPagoExists(id))
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

        // POST: api/FormaPagos
        [ResponseType(typeof(FormaPago))]
        [DisableCors]
        public IHttpActionResult PostFormaPago(FormaPago formaPago)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            formaPago.descripcion = c.encriptar(formaPago.descripcion);

            db.FormaPago.Add(formaPago);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = formaPago.id }, formaPago);
        }

        // DELETE: api/FormaPagos/5
        [ResponseType(typeof(FormaPago))]
        [DisableCors]
        public IHttpActionResult DeleteFormaPago(decimal id)
        {
            FormaPago formaPago = db.FormaPago.Find(id);
            if (formaPago == null)
            {
                return NotFound();
            }

            db.FormaPago.Remove(formaPago);
            db.SaveChanges();

            return Ok(formaPago);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FormaPagoExists(decimal id)
        {
            return db.FormaPago.Count(e => e.id == id) > 0;
        }
    }
}