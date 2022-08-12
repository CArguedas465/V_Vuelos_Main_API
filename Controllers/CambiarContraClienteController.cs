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
    public class CambiarContraClienteController : ApiController
    {
        private V_Vuelos_MainEntities db;
        Crypt c;
        public CambiarContraClienteController()
        {
            db = new V_Vuelos_MainEntities();
            c = new Crypt();
        }


        // PUT: api/CambiarContraCliente/5
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

            clienteWeb.contrasena = c.encriptar(clienteWeb.contrasena);

            db.uspCambiarContrasenaCliente(clienteWeb.usuario, clienteWeb.contrasena);

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