using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_Canvia
{
    public class Utilities
    {
        public static object IsNull(object objeto)
        {
            var retorno = new object();
            if (objeto == null)
            {
                retorno = DBNull.Value;
            }
            else {
                retorno = objeto;
            }
            return retorno;
        }        
    }
}
