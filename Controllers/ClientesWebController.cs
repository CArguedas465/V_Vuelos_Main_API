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
    public class ClientesWebController : ApiController
    {
        private V_Vuelos_MainEntities db;
        Crypt c;
        public ClientesWebController()
        {
            db = new V_Vuelos_MainEntities();
            c = new Crypt();
        }

        // GET: api/ClientesWeb
        public IEnumerable<ClienteWeb> GetClienteWeb()
        {
            IEnumerable<ClienteWeb> resultado = db.ClienteWeb;

            foreach (ClienteWeb cliente in resultado)
            {
                cliente.nombre = c.desencriptar(cliente.nombre);
                cliente.primer_apellido = c.desencriptar(cliente.primer_apellido);
                cliente.segundo_apellido = c.desencriptar(cliente.segundo_apellido);
                cliente.correo = c.desencriptar(cliente.correo);
                cliente.contrasena = c.desencriptar(cliente.contrasena);
                cliente.respuesta_seguridad = c.desencriptar(cliente.respuesta_seguridad);
            }

            return resultado;
        }

        // GET: api/ClientesWeb/5
        [ResponseType(typeof(ClienteWeb))]
        public IHttpActionResult GetClienteWeb(string id)
        {
            ClienteWeb clienteWeb = db.ClienteWeb.Find(id);
            if (clienteWeb == null)
            {
                return NotFound();
            }

            clienteWeb.nombre = c.desencriptar(clienteWeb.nombre);
            clienteWeb.primer_apellido = c.desencriptar(clienteWeb.primer_apellido);
            clienteWeb.segundo_apellido = c.desencriptar(clienteWeb.segundo_apellido);
            clienteWeb.correo = c.desencriptar(clienteWeb.correo);
            clienteWeb.contrasena = c.desencriptar(clienteWeb.contrasena);
            clienteWeb.respuesta_seguridad = c.desencriptar(clienteWeb.respuesta_seguridad);

            return Ok(clienteWeb);
        }

        // PUT: api/ClientesWeb/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutClienteWeb(string id, ClienteWeb clienteWeb)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != clienteWeb.usuario)
            {
                return BadRequest();
            }


            clienteWeb.nombre = c.encriptar(clienteWeb.nombre);
            clienteWeb.primer_apellido = c.encriptar(clienteWeb.primer_apellido);
            clienteWeb.segundo_apellido = c.encriptar(clienteWeb.segundo_apellido);
            clienteWeb.correo = c.encriptar(clienteWeb.correo);
            clienteWeb.contrasena = c.encriptar(clienteWeb.contrasena);
            clienteWeb.respuesta_seguridad = c.encriptar(clienteWeb.respuesta_seguridad);

            db.Entry(clienteWeb).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteWebExists(id))
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

        // POST: api/ClientesWeb
        [ResponseType(typeof(ClienteWeb))]
        public IHttpActionResult PostClienteWeb(ClienteWeb clienteWeb)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var clienteWebDesencriptado =
                new ClienteWeb() 
                {
                    usuario = clienteWeb.usuario,
                    nombre = clienteWeb.nombre,
                    primer_apellido = clienteWeb.primer_apellido,
                    segundo_apellido = clienteWeb.segundo_apellido, 
                    correo = clienteWeb.correo, 
                    contrasena = null, 
                    pregunta_seguridad = clienteWeb.pregunta_seguridad, 
                    respuesta_seguridad = null
                };

            clienteWeb.nombre = c.encriptar(clienteWeb.nombre);
            clienteWeb.primer_apellido = c.encriptar(clienteWeb.primer_apellido);
            clienteWeb.segundo_apellido = c.encriptar(clienteWeb.segundo_apellido);
            clienteWeb.correo = c.encriptar(clienteWeb.correo);
            clienteWeb.contrasena = c.encriptar(clienteWeb.contrasena);
            clienteWeb.respuesta_seguridad = c.encriptar(clienteWeb.respuesta_seguridad);

            db.ClienteWeb.Add(clienteWeb);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ClienteWebExists(clienteWeb.usuario))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", clienteWebDesencriptado, clienteWeb);
        }

        // DELETE: api/ClientesWeb/5
        [ResponseType(typeof(ClienteWeb))]
        public IHttpActionResult DeleteClienteWeb(string id)
        {
            ClienteWeb clienteWeb = db.ClienteWeb.Find(id);
            if (clienteWeb == null)
            {
                return NotFound();
            }

            db.ClienteWeb.Remove(clienteWeb);
            db.SaveChanges();

            return Ok(clienteWeb);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClienteWebExists(string id)
        {
            return db.ClienteWeb.Count(e => e.usuario == id) > 0;
        }
    }
}