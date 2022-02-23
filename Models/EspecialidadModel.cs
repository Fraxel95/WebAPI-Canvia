using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebAPI_Canvia.Models
{
    public class EspecialidadModel
    {
        [Key]
        public Nullable<Int32> ESPECIALIDAD_CODIGO { get; set; }
        public string ESPECIALIDAD { get; set; }
        public string ESPECIALIDAD_ESTADO { get; set; }
    }
}
