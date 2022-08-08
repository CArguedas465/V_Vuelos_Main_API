using System;
using System.Collections.Generic;
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
    public class RolesController : ApiController
    {
        V_Vuelos_MainEntities db;
        Crypt c;

        public RolesController()
        {
            db = new V_Vuelos_MainEntities();
            c = new Crypt();
        }

        // GET: api/Roles
        public IEnumerable<Rol> Get()
        {
            IEnumerable<Rol> resultado = db.Rol;

            foreach (var rol in resultado)
            {
                rol.descripcion = c.desencriptar(rol.descripcion);
            }

            return resultado;
        }

        // GET: api/Roles/5
        [ResponseType(typeof(Pregunta))]
        public IHttpActionResult Get(int id)
        {
            Rol rol = db.Rol.Find(id);

            if (rol == null)
            {
                return NotFound();
            }

            rol.descripcion = c.desencriptar(rol.descripcion);

            return Ok(rol);
        }

        // POST: api/Roles
        [ResponseType(typeof(Pregunta))]
        [DisableCors]
        public IHttpActionResult Post(Rol rol)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            rol.descripcion = c.encriptar(rol.descripcion);

            db.Rol.Add(rol);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (RolExiste(rol.descripcion))
                {
                    return Conflict();
                }
                throw;
            }

            return CreatedAtRoute("DefaultApi", new { id = rol.id }, rol);
        }

        // PUT: api/Roles/5
        [ResponseType(typeof(Pregunta))]
        [DisableCors]
        public IHttpActionResult Put(int id, Rol rol)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (id != rol.id)
            {
                return BadRequest();
            }

            rol.descripcion = c.encriptar(rol.descripcion);
            db.Entry(rol).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (!RolExiste(rol.descripcion))
                {
                    return NotFound();
                }
                throw;
            }

            return StatusCode(HttpStatusCode.OK);
        }

        // DELETE: api/Roles/5
        [ResponseType(typeof(Pregunta))]
        [DisableCors]
        public IHttpActionResult Delete(int id)
        {
            Rol rol = db.Rol.Find(id);
            if (rol == null)
            {
                return NotFound();
            }

            db.Rol.Remove(rol);
            db.SaveChanges();

            return Ok(rol);
        }

        private bool RolExiste(string rol)
        {
            return db.Rol.Count(e => e.descripcion == rol) > 0;
        }
    }
}
