using System;
using System.Collections.Generic;

namespace EntidadApi.Models
{
    public class PersonaNatural
    {
        public int EntidadID { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string? ApellidoMaterno { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int Edad { get; set; }
        public string TipoDocumento { get; set; } = string.Empty;
        public string NumeroDocumento { get; set; } = string.Empty;
        public string? Sexo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<TelefonoContacto> Telefonos { get; set; } = new List<TelefonoContacto>();
    }
}