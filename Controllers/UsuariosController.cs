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
using V_Vuelos_Main_API.Models;
using V_Vuelos_Main_API.Crypto;

namespace V_Vuelos_Main_API.Controllers
{
    public class UsuariosController : ApiController
    {
        private V_Vuelos_Main_NotEncryptedEntities db;
        Crypt c;
        public UsuariosController()
        {
            db = new V_Vuelos_Main_NotEncryptedEntities();
            c = new Crypt();    
        }
         

        // GET: api/Usuarios
        public IEnumerable<Usuario> GetUsuario()
        {
            IEnumerable<Usuario> resultado = db.Usuario;

            foreach (var usuario in resultado)
            {
                usuario.contrasena = c.desencriptar(usuario.contrasena);
                usuario.correo = c.desencriptar(usuario.correo);
                usuario.respuesta_seguridad = c.desencriptar(usuario.respuesta_seguridad);
            }

            return resultado;
        }

        // GET: api/Usuarios/5
        [ResponseType(typeof(Usuario))]
        public IHttpActionResult GetUsuario(string id)
        {
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }

            usuario.contrasena = c.desencriptar(usuario.contrasena);
            usuario.correo = c.desencriptar(usuario.correo);
            usuario.respuesta_seguridad = c.desencriptar(usuario.respuesta_seguridad);

            return Ok(usuario);
        }

        // PUT: api/Usuarios/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUsuario(string id, Usuario usuario)
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
            usuario.correo = c.encriptar(usuario.correo);
            usuario.respuesta_seguridad = c.encriptar(usuario.respuesta_seguridad);

            db.Entry(usuario).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.Accepted);
        }

        // POST: api/Usuarios
        [ResponseType(typeof(Usuario))]
        public IHttpActionResult PostUsuario(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            usuario.contrasena = c.encriptar(usuario.contrasena);
            usuario.correo = c.encriptar(usuario.correo);
            usuario.respuesta_seguridad = c.encriptar(usuario.respuesta_seguridad);

            db.Usuario.Add(usuario);

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

        // DELETE: api/Usuarios/5
        [ResponseType(typeof(Usuario))]
        public IHttpActionResult DeleteUsuario(string id)
        {
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }

            db.Usuario.Remove(usuario);
            db.SaveChanges();

            return Ok(usuario);
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