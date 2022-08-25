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
    public class ClientesTokenController : ApiController
    {
        private V_Vuelos_MainEntities db;
        Crypt c;
        public ClientesTokenController()
        {
            db = new V_Vuelos_MainEntities();
            c = new Crypt();
        }

        // GET: api/ClientesToken
        public IEnumerable<ClienteToken> GetClienteToken()
        {
            IEnumerable<ClienteToken> resultado = db.ClienteToken;

            foreach (var clienteToken in resultado)
            {
                clienteToken.correo = c.desencriptar(clienteToken.correo);
            }

            return resultado;
        }

        // GET: api/ClientesToken/5
        [ResponseType(typeof(ClienteToken))]
        public IHttpActionResult GetClienteToken(string id)
        {
            ClienteToken clienteToken = db.ClienteToken.Find(c.encriptar(id));
            if (clienteToken == null)
            {
                return NotFound();
            }

            clienteToken.correo = c.desencriptar(clienteToken.correo);

            return Ok(clienteToken);
        }

        // PUT: api/ClientesToken/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutClienteToken(string id, ClienteToken clienteToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != clienteToken.correo)
            {
                return BadRequest();
            }

            clienteToken.correo = c.encriptar(clienteToken.correo);
            db.Entry(clienteToken).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteTokenExists(id))
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

        // POST: api/ClientesToken
        [ResponseType(typeof(ClienteToken))]
        public IHttpActionResult PostClienteToken(ClienteToken clienteToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ClienteToken clienteDesc = new ClienteToken() { correo = clienteToken.correo };

            clienteToken.correo = c.encriptar(clienteToken.correo);

            db.ClienteToken.Add(clienteToken);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ClienteTokenExists(clienteToken.correo))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", clienteDesc, clienteToken);
        }

        // DELETE: api/ClientesToken/5
        [ResponseType(typeof(ClienteToken))]
        public IHttpActionResult DeleteClienteToken(string id)
        {
            ClienteToken clienteToken = db.ClienteToken.Find(c.encriptar(id));
            if (clienteToken == null)
            {
                return NotFound();
            }

            db.ClienteToken.Remove(clienteToken);
            db.SaveChanges();

            return Ok(clienteToken);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClienteTokenExists(string id)
        {
            return db.ClienteToken.Count(e => e.correo == id) > 0;
        }
    }
}