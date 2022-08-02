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
    public class BusquedaBitacorasController : ApiController
    {
        private V_Vuelos_MainEntities db;
        Crypt c;
        public BusquedaBitacorasController()
        {
            db = new V_Vuelos_MainEntities();
            c = new Crypt();
        }

        // POST: api/BusquedaBitacoras
        [ResponseType(typeof(Bitacora))]
        public IHttpActionResult PostBitacora(BusquedaBitacora busqueda)
        {
            if (busqueda.tipo_busqueda == 1) // Por nombre usuario
            {
                var bitacora = from bit in db.Bitacora
                               where bit.usuario.Equals(busqueda.nombre_usuario)
                               select bit;

                if (!bitacora.Any())
                {
                    return NotFound();
                }

                foreach (var bit in bitacora)
                {
                    bit.hora = c.desencriptar(bit.hora);
                    bit.registro_detalle = c.desencriptar(bit.registro_detalle);
                    bit.descripcion = c.desencriptar(bit.descripcion);
                }

                return Ok(bitacora);
            }
            else if (busqueda.tipo_busqueda == 2) // Por Operación
            {
                var bitacora = from bit in db.Bitacora
                               where bit.operacion.Equals(busqueda.tipo_accion)
                               select bit;

                if (!bitacora.Any())
                {
                    return NotFound();
                }

                foreach (var bit in bitacora)
                {
                    bit.hora = c.desencriptar(bit.hora);
                    bit.registro_detalle = c.desencriptar(bit.registro_detalle);
                    bit.descripcion = c.desencriptar(bit.descripcion);
                }

                return Ok(bitacora);
            }
            else if (busqueda.tipo_busqueda == 3) // Por rango fechas
            {
                var initial_date = DateTime.ParseExact(busqueda.fecha_inicio, "yyyy'-'MM'-'dd", null);
                var final_date = DateTime.ParseExact(busqueda.fecha_final, "yyyy'-'MM'-'dd", null);

                if (initial_date > final_date)
                {
                    return BadRequest("La fecha inicial no debe ser mayor a la final.");
                }

                var bitacora = from bit in db.Bitacora
                               where (initial_date < bit.fecha && final_date > bit.fecha)
                               select bit;

                if (!bitacora.Any())
                {
                    return NotFound();
                }

                foreach (var bit in bitacora)
                {
                    bit.hora = c.desencriptar(bit.hora);
                    bit.registro_detalle = c.desencriptar(bit.registro_detalle);
                    bit.descripcion = c.desencriptar(bit.descripcion);
                }

                return Ok(bitacora);
            }
            else if (busqueda.tipo_busqueda == 4) // Por Nombre Usuario y Operación
            {
                var bitacora = from bit in db.Bitacora
                               where (bit.usuario == busqueda.nombre_usuario && bit.operacion == busqueda.tipo_accion)
                               select bit;

                if (!bitacora.Any())
                {
                    return NotFound();
                }

                foreach (var bit in bitacora)
                {
                    bit.hora = c.desencriptar(bit.hora);
                    bit.registro_detalle = c.desencriptar(bit.registro_detalle);
                    bit.descripcion = c.desencriptar(bit.descripcion);
                }

                return Ok(bitacora);
            }
            else if (busqueda.tipo_busqueda == 5) // Por Nombre y Rango de Fechas
            {
                var initial_date = DateTime.ParseExact(busqueda.fecha_inicio, "yyyy'-'MM'-'dd", null);
                var final_date = DateTime.ParseExact(busqueda.fecha_final, "yyyy'-'MM'-'dd", null);

                if (initial_date > final_date)
                {
                    return BadRequest("La fecha inicial no debe ser mayor a la final.");
                }

                var bitacora = from bit in db.Bitacora
                               where ((initial_date < bit.fecha && final_date > bit.fecha) && (bit.usuario == busqueda.nombre_usuario))
                               select bit;

                if (!bitacora.Any())
                {
                    return NotFound();
                }

                foreach (var bit in bitacora)
                {
                    bit.hora = c.desencriptar(bit.hora);
                    bit.registro_detalle = c.desencriptar(bit.registro_detalle);
                    bit.descripcion = c.desencriptar(bit.descripcion);
                }

                return Ok(bitacora);
            }
            else if (busqueda.tipo_busqueda == 6) // Por Operacion y Rango de Fechas
            {
                var initial_date = DateTime.ParseExact(busqueda.fecha_inicio, "yyyy'-'MM'-'dd", null);
                var final_date = DateTime.ParseExact(busqueda.fecha_final, "yyyy'-'MM'-'dd", null);

                if (initial_date > final_date)
                {
                    return BadRequest("La fecha inicial no debe ser mayor a la final.");
                }

                var bitacora = from bit in db.Bitacora
                               where ((initial_date < bit.fecha && final_date > bit.fecha) && (bit.operacion == busqueda.tipo_accion))
                               select bit;

                if (!bitacora.Any())
                {
                    return NotFound();
                }

                foreach (var bit in bitacora)
                {
                    bit.hora = c.desencriptar(bit.hora);
                    bit.registro_detalle = c.desencriptar(bit.registro_detalle);
                    bit.descripcion = c.desencriptar(bit.descripcion);
                }

                return Ok(bitacora);
            }
            else // Por Operacion, Usuario y Rango de Fechas, ID 7
            {
                var initial_date = DateTime.ParseExact(busqueda.fecha_inicio, "yyyy'-'MM'-'dd", null);
                var final_date = DateTime.ParseExact(busqueda.fecha_final, "yyyy'-'MM'-'dd", null);

                if (initial_date > final_date)
                {
                    return BadRequest("La fecha inicial no debe ser mayor a la final.");
                }

                var bitacora = from bit in db.Bitacora
                               where ((initial_date < bit.fecha && final_date > bit.fecha) && (bit.operacion == busqueda.tipo_accion) && (bit.usuario == busqueda.nombre_usuario))
                               select bit;

                if (!bitacora.Any())
                {
                    return NotFound();
                }

                foreach (var bit in bitacora)
                {
                    bit.hora = c.desencriptar(bit.hora);
                    bit.registro_detalle = c.desencriptar(bit.registro_detalle);
                    bit.descripcion = c.desencriptar(bit.descripcion);
                }

                return Ok(bitacora);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}