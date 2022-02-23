using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebAPI_Canvia.Models
{
    public class MedicoModel
    {
        [Key]
        public Nullable<Int32> MEDICO_CODIGO { get; set; }
        public string MEDICO_DNI { get; set; }
        public string MEDICO_NOMBRE { get; set; }
        public string MEDICO_APELLIDO { get; set; }
        public Nullable<Int32> MEDICO_EDAD { get; set; }
        public string MEDICO_GENERO { get; set; }
        public string MEDICO_CONSULTORIO { get; set; }
        public string MEDICO_TURNO { get; set; }
        public Nullable<Int32> MEDICO_ESPECIALIDAD { get; set; }
        public string MEDICO_ESTADO { get; set; }
        public string ESPECIALIDAD { get; set; }

    }
}
