//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServiAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class vehiculo
    {
        public int id_vehiculo { get; set; }
        public string marca { get; set; }
        public string modelo { get; set; }
        public int anho { get; set; }
        public string tipo_aceite { get; set; }
        public Nullable<int> km_ultimo_servicio { get; set; }
        public Nullable<System.DateTime> fecha_servicio { get; set; }
        public int id_usuario { get; set; }
        public Nullable<System.Guid> guid { get; set; }
    
        [Newtonsoft.Json.JsonIgnore] public virtual usuario usuario { get; set; }
    }
}
