using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebAPI_Canvia.Models
{
    public class CitaModel
    {
        [Key]
        public Nullable<Int32> CITA_CODIGO { get; set; }
        public string CITA_FECHA { get; set; }
        public string CITA_HORA { get; set; }
        public Nullable<Int32> CITA_ESPECIALIDAD { get; set; }
        public Nullable<Int32> CITA_MEDICO { get; set; }
        public Nullable<Int32> CITA_PACIENTE { get; set; }
        public string CITA_ESTADO { get; set; }
        public string PACIENTE_DNI { get; set; }
        public string PACIENTE_NOMBRE { get; set; }
        public string PACIENTE_APELLIDO { get; set; }
        public string MEDICO_DNI { get; set; }
        public string MEDICO_NOMBRE { get; set; }
        public string MEDICO_APELLIDO { get; set; }
        public string MEDICO_CONSULTORIO { get; set; }
        public string MEDICO_TURNO { get; set; }
    }
}
