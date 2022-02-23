using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_Canvia.Models
{
    public abstract class GlobalModel
    {
        [NotMapped]
        [IgnoreDataMember]
        public Nullable<Int32> COLORDER { get; set; }
        [NotMapped]
        [IgnoreDataMember]
        public Nullable<Int32> NUMRESULT { get; set; }
        [NotMapped]
        [IgnoreDataMember]
        public Nullable<Int32> NUMPAG { get; set; }
        [NotMapped]
        [IgnoreDataMember]
        public Nullable<Int32> MAXPAG { get; set; } 

    }
}
