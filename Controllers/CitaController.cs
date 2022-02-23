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
    public class CitaController : Controller
    {
        private readonly Conexion _context;

        public CitaController(Conexion context)
        {
            _context = context;
        }

        [HttpGet("BuscarPag")]
        // GET: BuscarPag
        public async Task<ActionResult<IEnumerable<ResultBuscar>>> GetByParamPag(CitaDataIn cita)
        {
            List<ResultBuscar> _result = new List<ResultBuscar>();
            List<CitaModel> _citas = new List<CitaModel>();
            var bdParameters = new List<SqlParameter>();
            bdParameters.Add(new SqlParameter("@CITA_CODIGO", SqlDbType.Int) { Value = Utilities.IsNull(cita.CITA_CODIGO) });          
            bdParameters.Add(new SqlParameter("@CITA_FECHA", SqlDbType.VarChar) { Value = Utilities.IsNull(cita.CITA_FECHA) });
            bdParameters.Add(new SqlParameter("@CITA_HORA", SqlDbType.VarChar) { Value = Utilities.IsNull(cita.CITA_HORA) });
            bdParameters.Add(new SqlParameter("@CITA_ESPECIALIDAD", SqlDbType.Int) { Value = Utilities.IsNull(cita.CITA_ESPECIALIDAD) });
            bdParameters.Add(new SqlParameter("@CITA_MEDICO", SqlDbType.Int) { Value = Utilities.IsNull(cita.CITA_MEDICO) });
            bdParameters.Add(new SqlParameter("@CITA_PACIENTE", SqlDbType.Int) { Value = Utilities.IsNull(cita.CITA_PACIENTE) });
            bdParameters.Add(new SqlParameter("@CITA_ESTADO", SqlDbType.VarChar) { Value = Utilities.IsNull(cita.CITA_ESTADO) });
            bdParameters.Add(new SqlParameter("@ColOrder", SqlDbType.Int) { Value = Utilities.IsNull(cita.COLORDER) });
            bdParameters.Add(new SqlParameter("@NumResult", SqlDbType.Int) { Value = Utilities.IsNull(cita.NUMRESULT) });
            bdParameters.Add(new SqlParameter("@NumPag", SqlDbType.Int) { Value = Utilities.IsNull(cita.NUMPAG) });
            bdParameters.Add(new SqlParameter { ParameterName = "@MaxPag", DbType = DbType.Int32, Direction = ParameterDirection.Output });

            string StoredProc = " exec SP_LISTA_PAG_CITA " +
                                "@CITA_CODIGO		 ," +
                                "@CITA_FECHA		 ," +
                                "@CITA_HORA			 ," +
                                "@CITA_ESPECIALIDAD	 ," +
                                "@CITA_MEDICO		 ," +
                                "@CITA_PACIENTE		 ," +
                                "@CITA_ESTADO		 ," +
                                "@ColOrder			 ," +
                                "@NumResult			 ," +
                                "@NumPag			 ," +
                                "@MaxPag         OUT";
            var _task = await Task.Run(() => _citas = _context.CitaModel.FromSqlRaw(@StoredProc, bdParameters.ToArray()).ToList());
            _result.Add(new ResultBuscar() { MaxPag = int.Parse(bdParameters[10].Value.ToString()), ListResult = _citas.Cast<Object>().ToList() });
            return _result;
        }

        [HttpPost("Registrar")]
        // POST: Registrar
        public async Task<ActionResult<CitaModel>> Post(CitaDataIn cita)
        {
            List<CitaModel> _listExist = new List<CitaModel>();
            CitaModel _result = new CitaModel();
            var _taskExist = await Task.Run(() => GetByParamPag(new CitaDataIn() { CITA_MEDICO = cita.CITA_MEDICO, CITA_FECHA = cita.CITA_FECHA, CITA_HORA = cita.CITA_HORA }));
            if (_taskExist.Value.ToList()[0].ListResult.Count() > 0)
            {
                _result.CITA_FECHA = "El médico no se encuentra disponible en la fecha y hora indicada.";
                return _result;
            };
            var bdParameters = new List<SqlParameter>();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<CitaDataIn, CitaModel>()));
            bdParameters.Add(new SqlParameter("@CITA_FECHA", SqlDbType.VarChar) { Value = Utilities.IsNull(cita.CITA_FECHA) });
            bdParameters.Add(new SqlParameter("@CITA_HORA", SqlDbType.VarChar) { Value = Utilities.IsNull(cita.CITA_HORA) });
            bdParameters.Add(new SqlParameter("@CITA_ESPECIALIDAD", SqlDbType.Int) { Value = Utilities.IsNull(cita.CITA_ESPECIALIDAD) });
            bdParameters.Add(new SqlParameter("@CITA_MEDICO", SqlDbType.Int) { Value = Utilities.IsNull(cita.CITA_MEDICO) });
            bdParameters.Add(new SqlParameter("@CITA_PACIENTE", SqlDbType.Int) { Value = Utilities.IsNull(cita.CITA_PACIENTE) });
            bdParameters.Add(new SqlParameter("@CITA_ESTADO", SqlDbType.VarChar) { Value = Utilities.IsNull(cita.CITA_ESTADO) });
            bdParameters.Add(new SqlParameter { ParameterName = "@CITA_CODIGO", DbType = DbType.Int32, Direction = ParameterDirection.Output });
            string StoredProc = " exec SP_INSERT_CITA " +
                                "@CITA_FECHA		 ," +
                                "@CITA_HORA			 ," +
                                "@CITA_ESPECIALIDAD	 ," +
                                "@CITA_MEDICO		 ," +
                                "@CITA_PACIENTE		 ," +
                                "@CITA_ESTADO		 ," +
                                "@CITA_CODIGO     OUT";
            var _task = await Task.Run(() => _context.Database.ExecuteSqlRaw(@StoredProc, bdParameters.ToArray()));
            _result = mapper.Map<CitaModel>(cita);
            _result.CITA_CODIGO = int.Parse(bdParameters[6].Value.ToString());
            return _result;
        }
    }
}
