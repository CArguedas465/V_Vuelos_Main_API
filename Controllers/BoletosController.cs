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
    public class BoletosController : ApiController
    {
        private V_Vuelos_MainEntities db;
        Crypt c;
        public BoletosController()
        {
            db = new V_Vuelos_MainEntities();
            c = new Crypt();
        }

        // GET: api/Boletos
        public IEnumerable<Boleto> GetBoleto()
        {
            IEnumerable<Boleto> resultado = db.Boleto;

            foreach (var boleto in resultado)
            {
                boleto.precio = c.desencriptar(boleto.precio);
                boleto.cantidad = c.desencriptar(boleto.cantidad);
            }

            return resultado;
        }

        // GET: api/Boletos/5
        [ResponseType(typeof(Boleto))]
        public IHttpActionResult GetBoleto(string id)
        {
            Boleto boleto = db.Boleto.Find(id);

            if (boleto == null)
            {
                return NotFound();
            }

            boleto.precio = c.desencriptar(boleto.precio);
            boleto.cantidad = c.desencriptar(boleto.cantidad);

            return Ok(boleto);
        }

        // PUT: api/Boletos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBoleto(string id, Boleto boleto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != boleto.id)
            {
                return BadRequest();
            }

            boleto.precio = c.encriptar(boleto.precio);
            boleto.cantidad = c.encriptar(boleto.cantidad);

            db.Entry(boleto).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BoletoExists(id))
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

        // POST: api/Boletos
        [ResponseType(typeof(Boleto))]
        public IHttpActionResult PostBoleto(Boleto boleto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Boleto boletoDesc = new Boleto()
            {
                id = boleto.id,
                forma_pago = boleto.forma_pago,
                tipo_transaccion = boleto.tipo_transaccion, 
                vuelo = boleto.vuelo,
                cliente = boleto.cliente, 
                precio = boleto.precio, 
                cantidad = boleto.cantidad
            };

            boleto.precio = c.encriptar(boleto.precio);
            boleto.cantidad = c.encriptar(boleto.cantidad);

            db.Boleto.Add(boleto);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (BoletoExists(boleto.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", boletoDesc, boleto);
        }

        // DELETE: api/Boletos/5
        [ResponseType(typeof(Boleto))]
        public IHttpActionResult DeleteBoleto(string id)
        {
            Boleto boleto = db.Boleto.Find(id);
            if (boleto == null)
            {
                return NotFound();
            }

            db.Boleto.Remove(boleto);
            db.SaveChanges();

            return Ok(boleto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BoletoExists(string id)
        {
            return db.Boleto.Count(e => e.id == id) > 0;
        }
    }
}