//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace V_Vuelos_Main_API.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Boleto
    {
        public string id { get; set; }
        public decimal forma_pago { get; set; }
        public decimal tipo_transaccion { get; set; }
        public string vuelo { get; set; }
        public string cliente { get; set; }
        public string precio { get; set; }
        public decimal cantidad { get; set; }
    }
}
