using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_Canvia.Models
{
    public class PacienteDataIn : GlobalModel
    {        
        public Nullable<Int32> PACIENTE_CODIGO { get; set; }
        public string PACIENTE_DNI { get; set; }
        public string PACIENTE_NOMBRE { get; set; }
        public string PACIENTE_APELLIDO { get; set; }
        public Nullable<Int32> PACIENTE_EDAD { get; set; }
        public string PACIENTE_GENERO { get; set; }
        public string PACIENTE_ESTADO { get; set; }

    }
    public class MedicoDataIn : GlobalModel
    {
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

    }
    public class CitaDataIn : GlobalModel
    {
        public Nullable<Int32> CITA_CODIGO { get; set; }
        public string CITA_FECHA { get; set; }
        public string CITA_HORA { get; set; }
        public Nullable<Int32> CITA_ESPECIALIDAD { get; set; }
        public Nullable<Int32> CITA_MEDICO { get; set; }
        public Nullable<Int32> CITA_PACIENTE { get; set; }
        public string CITA_ESTADO { get; set; }

    }
    public class MsgResult
    { 
        public string Result { get; set; }
        public string Content { get; set; }
    }
    public class ResultBuscar
    {
        public Nullable<Int32> MaxPag { get; set; }
        public List<Object> ListResult { get; set; }
    }
}
