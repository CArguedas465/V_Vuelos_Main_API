using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace V_Vuelos_Main_API.Models
{
    public class BusquedaBitacora
    {
        public decimal tipo_busqueda { get; set; }
        public string nombre_usuario { get; set; }
        public decimal tipo_accion { get; set; }
        public string fecha_inicio { get; set; }
        public string fecha_final { get; set; }
    }
}