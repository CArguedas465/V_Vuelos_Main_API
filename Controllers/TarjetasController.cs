using CreditCardValidator;
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
                tarjeta.mes_expiracion = c.desencriptar(tarjeta.mes_expiracion);
                tarjeta.anio_expiracion = c.desencriptar(tarjeta.anio_expiracion);
                tarjeta.cvv = c.desencriptar(tarjeta.cvv);
                if (tarjeta.clienteToken != null)
                {
                    tarjeta.clienteToken = c.desencriptar(tarjeta.clienteToken);
                }

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
            tarjeta.mes_expiracion = c.desencriptar(tarjeta.mes_expiracion);
            tarjeta.anio_expiracion = c.desencriptar(tarjeta.anio_expiracion);
            tarjeta.cvv = c.desencriptar(tarjeta.cvv);
            if (tarjeta.clienteToken != null)
            {
                tarjeta.clienteToken = c.desencriptar(tarjeta.clienteToken);
            }

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
            tarjeta.mes_expiracion = c.encriptar(tarjeta.mes_expiracion);
            tarjeta.anio_expiracion = c.encriptar(tarjeta.anio_expiracion);
            tarjeta.cvv = c.encriptar(tarjeta.cvv);
            if (tarjeta.clienteToken != null)
            {
                tarjeta.clienteToken = c.encriptar(tarjeta.clienteToken);
            }

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
                anio_expiracion = tarjeta.anio_expiracion, 
                mes_expiracion = tarjeta.mes_expiracion,
                tipo_tarjeta = tarjeta.tipo_tarjeta,
                clienteWeb = tarjeta.clienteWeb,
                clienteToken = tarjeta.clienteToken,
                cvv = tarjeta.cvv
            };

            tarjeta.numero_tarjeta = c.encriptar(tarjeta.numero_tarjeta);
            tarjeta.mes_expiracion = c.encriptar(tarjeta.mes_expiracion);
            tarjeta.anio_expiracion = c.encriptar(tarjeta.anio_expiracion);
            tarjeta.cvv = c.encriptar(tarjeta.cvv);
            if (tarjeta.clienteToken != null)
            {
                tarjeta.clienteToken = c.encriptar(tarjeta.clienteToken);
            }

            CreditCardDetector detector = new CreditCardDetector(tarjetaDesc.numero_tarjeta);

            if (!detector.IsValid())
            {
                return BadRequest("-1; Número de tarjeta inválido.");
            }

            TipoTarjeta tipoTarjeta = db.TipoTarjeta.Find(tarjetaDesc.tipo_tarjeta);
          
            if (!(detector.BrandName.ToUpper() == c.desencriptar(tipoTarjeta.descripcion).ToUpper()))
            {
                return BadRequest("-1; Número de tarjeta inválido.");
            };

            DateTime date = DateTime.Now;

            if (date.Year > Convert.ToInt32(tarjetaDesc.anio_expiracion))
            {
                return BadRequest("-2; Fecha de expiración inválida o la tarjeta expiró.");
            }
            else
            {
                if (date.Year == Convert.ToInt32(tarjetaDesc.anio_expiracion))
                {
                    if (date.Month > Convert.ToInt32(tarjetaDesc.mes_expiracion))
                    {
                        return BadRequest("-2; Fecha de expiración inválida o la tarjeta expiró.");
                    }
                }
            }

            var cvvLength = tarjetaDesc.cvv.ToString().Length;
            if (cvvLength != 3)
            {
                return BadRequest("-3; CVV incorrecto.");
            }

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
            Tarjeta tarjeta = db.Tarjeta.Find(c.encriptar(id));
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