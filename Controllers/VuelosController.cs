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
    public class VuelosController : ApiController
    {
        private V_Vuelos_MainEntities db;
        Crypt c;
        public VuelosController()
        {
            db = new V_Vuelos_MainEntities();
            c = new Crypt();
        }

        // GET: api/Vuelos
        public IEnumerable <Vuelo> GetVuelo()
        {
            IEnumerable<Vuelo> resultado = db.Vuelo;

            foreach (var vuelo in resultado)
            {
                vuelo.fecha_partida = c.desencriptar(vuelo.fecha_partida);
                vuelo.hora_partida = c.desencriptar(vuelo.hora_partida);
                vuelo.fecha_llegada = c.desencriptar(vuelo.fecha_llegada);
                vuelo.hora_llegada = c.desencriptar(vuelo.hora_llegada);
                vuelo.precio = c.desencriptar(vuelo.precio);

            }

            return resultado;
        }

        // GET: api/Vuelos/5
        [ResponseType(typeof(Vuelo))]
        public IHttpActionResult GetVuelo(string id)
        {
            Vuelo vuelo = db.Vuelo.Find(id);
            if (vuelo == null)
            {
                return NotFound();
            }

            vuelo.fecha_partida = c.desencriptar(vuelo.fecha_partida);
            vuelo.hora_partida = c.desencriptar(vuelo.hora_partida);
            vuelo.fecha_llegada = c.desencriptar(vuelo.fecha_llegada);
            vuelo.hora_llegada = c.desencriptar(vuelo.hora_llegada);
            vuelo.precio = c.desencriptar(vuelo.precio);


            return Ok(vuelo);
        }

        // PUT: api/Vuelos/5
        [ResponseType(typeof(void))]
        [DisableCors]
        public IHttpActionResult PutVuelo(string id, Vuelo vuelo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (id != vuelo.id)
            {
                return BadRequest();
            }

            vuelo.fecha_partida = c.encriptar(vuelo.fecha_partida);
            vuelo.hora_partida = c.encriptar(vuelo.hora_partida);
            vuelo.fecha_llegada = c.encriptar(vuelo.fecha_llegada);
            vuelo.hora_llegada = c.encriptar(vuelo.hora_llegada);
            vuelo.precio = c.encriptar(vuelo.precio);

            db.Entry(vuelo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VueloExists(id))
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

        // POST: api/Vuelos
        [ResponseType(typeof(Vuelo))]
        [DisableCors]
        public IHttpActionResult PostVuelo(Vuelo vuelo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Consecutivo consecutivo = db.Consecutivo.Find(5);

            vuelo.id = c.desencriptar(consecutivo.prefijo) + "-" + c.desencriptar(consecutivo.valor);
            vuelo.fecha_partida = c.encriptar(vuelo.fecha_partida);
            vuelo.hora_partida = c.encriptar(vuelo.hora_partida);
            vuelo.fecha_llegada = c.encriptar(vuelo.fecha_llegada);
            vuelo.hora_llegada = c.encriptar(vuelo.hora_llegada);
            vuelo.precio = c.encriptar(vuelo.precio);

            int valor = Convert.ToInt32(c.desencriptar(consecutivo.valor));
            valor++;
            consecutivo.valor = c.encriptar(valor.ToString());

            db.Vuelo.Add(vuelo);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (VueloExists(vuelo.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            var aerolinea = db.Aerolinea.Find(vuelo.aerolinea);
            var puerta = db.Puerta.Find(vuelo.puerta);
            var estado = db.EstadoVuelo.Find(vuelo.estado);
            var parte_de = db.Pais.Find(vuelo.parte_de);
            var llega_a = db.Pais.Find(vuelo.llega_a);

            var fechaPartidaDesc = c.desencriptar(vuelo.fecha_partida);
            var horaPartidaDesc = c.desencriptar(vuelo.hora_partida);
            var fechaLlegadaDesc = c.desencriptar(vuelo.fecha_llegada);
            var horaLlegadaDesc = c.desencriptar(vuelo.hora_llegada);

            var aerolineaDesc = c.desencriptar(aerolinea.nombre);
            var puertaDesc = c.desencriptar(puerta.numero);
            var estadoDesc = c.desencriptar(estado.descripcion);
            var parteDesc = c.desencriptar(parte_de.nombre);
            var llegaDesc = c.desencriptar(llega_a.nombre);
            var precioDesc = c.desencriptar(vuelo.precio);

            return  Ok(new
            {
                id = vuelo.id,
                fechaPartidaDesc,
                horaPartidaDesc,
                fechaLlegadaDesc,
                horaLlegadaDesc,
                aerolineaDesc,
                puertaDesc,
                estadoDesc,
                parteDesc,
                llegaDesc,
                precioDesc
            });
        }

        // DELETE: api/Vuelos/5
        [ResponseType(typeof(Vuelo))]
        [DisableCors]
        public IHttpActionResult DeleteVuelo(string id)
        {
            Vuelo vuelo = db.Vuelo.Find(id);
            if (vuelo == null)
            {
                return NotFound();
            }

            db.Vuelo.Remove(vuelo);
            db.SaveChanges();

            return Ok(vuelo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VueloExists(string id)
        {
            return db.Vuelo.Count(e => e.id == id) > 0;
        }
    }
}