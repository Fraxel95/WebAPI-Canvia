using System;
using System.Net.Http;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AutoMapper;
using WebAPI_Canvia.Models;

namespace WebAPI_Canvia.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EspecialidadController : Controller
    {
        private readonly Conexion _context;

        public EspecialidadController(Conexion context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<EspecialidadModel>>> Get()
        {
            EspecialidadModel especialidad = new EspecialidadModel();
            var bdParameters = new List<SqlParameter>();
            bdParameters.Add(new SqlParameter("@ESPECIALIDAD_CODIGO", SqlDbType.Int) { Value = Utilities.IsNull(especialidad.ESPECIALIDAD_CODIGO) });
            bdParameters.Add(new SqlParameter("@ESPECIALIDAD", SqlDbType.VarChar) { Value = Utilities.IsNull(especialidad.ESPECIALIDAD) });
            bdParameters.Add(new SqlParameter("@ESPECIALIDAD_ESTADO", SqlDbType.VarChar) { Value = Utilities.IsNull(especialidad.ESPECIALIDAD_ESTADO) });

            string StoredProc = " exec SP_LISTA_ESPECIALIDAD " +
                                " @ESPECIALIDAD_CODIGO       ," +
                                " @ESPECIALIDAD          ," +
                                " @ESPECIALIDAD_ESTADO       ";
            return await _context.EspecialidadModel.FromSqlRaw(StoredProc, bdParameters.ToArray()).ToListAsync();
        }
    }
}
