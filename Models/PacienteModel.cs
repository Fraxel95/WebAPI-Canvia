using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebAPI_Canvia.Models
{
    public class PacienteModel
    {
        [Key]
        public Nullable<Int32> PACIENTE_CODIGO { get; set; }
        public string PACIENTE_DNI { get; set; }
        public string PACIENTE_NOMBRE { get; set; }
        public string PACIENTE_APELLIDO { get; set; }
        public Nullable<Int32> PACIENTE_EDAD { get; set; }
        public string PACIENTE_GENERO { get; set; }
        public string PACIENTE_ESTADO { get; set; }

    }
}
