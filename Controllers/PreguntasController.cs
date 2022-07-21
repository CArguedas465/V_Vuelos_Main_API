using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using V_Vuelos_Main_API.Models;
using V_Vuelos_Main_API.Crypto;
using System.Data.Entity.Core.Objects;
using System.Web.Http.Description;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

namespace V_Vuelos_Main_API.Controllers
{
    public class PreguntasController : ApiController
    {
        V_Vuelos_MainEntities db;
        Crypt c;

        public PreguntasController()
        {
            db = new V_Vuelos_MainEntities();
            c = new Crypt();
            
        }
        // GET: api/Preguntas
        public IEnumerable<Pregunta> Get()
        {
            IEnumerable<Pregunta> resultado = db.Pregunta;

            foreach (var pregunta in resultado)
            {
                pregunta.descripcion = c.desencriptar(pregunta.descripcion);
            }

            return resultado;
        }

        // GET: api/Preguntas/5
        [ResponseType(typeof(Pregunta))]
        public IHttpActionResult Get(int id)
        {
            Pregunta pregunta = db.Pregunta.Find(id);

            if(pregunta==null)
            {
                return NotFound();
            }

            return Ok(pregunta);
        }

        // POST: api/Preguntas
        [ResponseType(typeof(Pregunta))]
        public IHttpActionResult Post(Pregunta pregunta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            pregunta.descripcion = c.encriptar(pregunta.descripcion);

            db.Pregunta.Add(pregunta);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PreguntaExiste(pregunta.descripcion))
                {
                    return Conflict();
                }
                throw;
            }

            return CreatedAtRoute("DefaultApi", new { id = pregunta.id }, pregunta);
        }

        // PUT: api/Preguntas/5
        [ResponseType(typeof(Pregunta))]
        public IHttpActionResult Put(int id, Pregunta pregunta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (id != pregunta.id)
            {
                return BadRequest();
            }

            db.Entry(pregunta).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (!PreguntaExiste(pregunta.descripcion))
                {
                    return NotFound();
                }
                throw;
            }

            return StatusCode(HttpStatusCode.OK);
        }

        // DELETE: api/Preguntas/5
        [ResponseType(typeof(Pregunta))]
        public IHttpActionResult Delete(int id)
        {
            Pregunta pregunta = db.Pregunta.Find(id);
            if (pregunta == null)
            {
                return NotFound();
            }

            db.Pregunta.Remove(pregunta);
            db.SaveChanges();

            return Ok(pregunta);
        }

        private bool PreguntaExiste(string pregunta)
        {
            return db.Pregunta.Count(e => e.descripcion == pregunta) > 0;
        }
    }
}
