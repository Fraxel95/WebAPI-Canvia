using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WebAPI_Canvia.Models;

namespace WebAPI_Canvia.Models
{
    public class Conexion : DbContext
    {
        public Conexion() { 

        }
        public Conexion(DbContextOptions<Conexion> options) : base(options) { 

        }
        public DbSet<WebAPI_Canvia.Models.PacienteModel> PacienteModel { get; set; }
        public DbSet<WebAPI_Canvia.Models.MedicoModel> MedicoModel { get; set; }
        public DbSet<WebAPI_Canvia.Models.EspecialidadModel> EspecialidadModel { get; set; }
        public DbSet<WebAPI_Canvia.Models.CitaModel> CitaModel { get; set; }
    }
}
