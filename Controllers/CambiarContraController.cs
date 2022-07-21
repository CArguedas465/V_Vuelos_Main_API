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
    public class CambiarContraController : ApiController
    {
        private V_Vuelos_MainEntities db;
        Crypt c;
        public CambiarContraController()
        {
            db = new V_Vuelos_MainEntities();
            c = new Crypt();
        }

        // POST: api/CambiarContra
        [ResponseType(typeof(Usuario))]
        public IHttpActionResult PutContrasenna(string id, Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuario.usuario1)
            {
                return BadRequest();
            }

            usuario.contrasena = c.encriptar(usuario.contrasena);

            db.uspCambiarContrasenaUsuario(usuario.usuario1, usuario.contrasena);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UsuarioExists(usuario.usuario1))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = usuario.usuario1 }, usuario);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UsuarioExists(string id)
        {
            return db.Usuario.Count(e => e.usuario1 == id) > 0;
        }
    }
}