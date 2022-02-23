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
    public class PacienteController : Controller
    {
        private readonly Conexion _context;

        public PacienteController(Conexion context)
        {
            _context = context;
        }
        [HttpGet]
        // GET: Paciente
        public async Task<ActionResult<IEnumerable<PacienteModel>>> Get()
        {
            return await Task.Run(() => GetByParam(new PacienteDataIn() { }));
        }

        [HttpGet("Buscar")]
        // GET: Buscar
        public async Task<ActionResult<IEnumerable<PacienteModel>>> GetByParam(PacienteDataIn paciente)
        {
            var bdParameters = new List<SqlParameter>();
            bdParameters.Add(new SqlParameter("@PACIENTE_CODIGO", SqlDbType.Int) { Value = Utilities.IsNull(paciente.PACIENTE_CODIGO) });
            bdParameters.Add(new SqlParameter("@PACIENTE_DNI", SqlDbType.VarChar) { Value = Utilities.IsNull(paciente.PACIENTE_DNI) });
            bdParameters.Add(new SqlParameter("@PACIENTE_NOMBRE", SqlDbType.VarChar) { Value = Utilities.IsNull(paciente.PACIENTE_NOMBRE) });
            bdParameters.Add(new SqlParameter("@PACIENTE_APELLIDO", SqlDbType.VarChar) { Value = Utilities.IsNull(paciente.PACIENTE_APELLIDO) });
            bdParameters.Add(new SqlParameter("@PACIENTE_EDAD", SqlDbType.Int) { Value = Utilities.IsNull(paciente.PACIENTE_EDAD) });
            bdParameters.Add(new SqlParameter("@PACIENTE_GENERO", SqlDbType.VarChar) { Value = Utilities.IsNull(paciente.PACIENTE_GENERO) });
            bdParameters.Add(new SqlParameter("@PACIENTE_ESTADO", SqlDbType.VarChar) { Value = Utilities.IsNull(paciente.PACIENTE_ESTADO) });
            bdParameters.Add(new SqlParameter("@ColOrder", SqlDbType.Int) { Value = Utilities.IsNull(paciente.COLORDER) });


            string StoredProc = " exec SP_LISTA_PACIENTE " +
                                " @PACIENTE_CODIGO     ," +
                                " @PACIENTE_DNI        ," +
                                " @PACIENTE_NOMBRE     ," +
                                " @PACIENTE_APELLIDO   ," +
                                " @PACIENTE_EDAD       ," +
                                " @PACIENTE_GENERO     ," +
                                " @PACIENTE_ESTADO     ," +
                                " @ColOrder            ";
            return await _context.PacienteModel.FromSqlRaw(StoredProc, bdParameters.ToArray()).ToListAsync();
        }

        [HttpGet("BuscarPag")]
        // GET: BuscarPag
        public async Task<ActionResult<IEnumerable<ResultBuscar>>> GetByParamPag(PacienteDataIn paciente)
        {
            List<ResultBuscar> _result = new List<ResultBuscar>();
            List<PacienteModel> _pacientes = new List<PacienteModel>();
            var bdParameters = new List<SqlParameter>();
            bdParameters.Add(new SqlParameter("@PACIENTE_CODIGO", SqlDbType.Int) { Value = Utilities.IsNull(paciente.PACIENTE_CODIGO) });
            bdParameters.Add(new SqlParameter("@PACIENTE_DNI", SqlDbType.VarChar) { Value = Utilities.IsNull(paciente.PACIENTE_DNI) });
            bdParameters.Add(new SqlParameter("@PACIENTE_NOMBRE", SqlDbType.VarChar) { Value = Utilities.IsNull(paciente.PACIENTE_NOMBRE) });
            bdParameters.Add(new SqlParameter("@PACIENTE_APELLIDO", SqlDbType.VarChar) { Value = Utilities.IsNull(paciente.PACIENTE_APELLIDO) });
            bdParameters.Add(new SqlParameter("@PACIENTE_EDAD", SqlDbType.Int) { Value = Utilities.IsNull(paciente.PACIENTE_EDAD) });
            bdParameters.Add(new SqlParameter("@PACIENTE_GENERO", SqlDbType.VarChar) { Value = Utilities.IsNull(paciente.PACIENTE_GENERO) });
            bdParameters.Add(new SqlParameter("@PACIENTE_ESTADO", SqlDbType.VarChar) { Value = Utilities.IsNull(paciente.PACIENTE_ESTADO) });
            bdParameters.Add(new SqlParameter("@ColOrder", SqlDbType.Int) { Value = Utilities.IsNull(paciente.COLORDER) });
            bdParameters.Add(new SqlParameter("@NumResult", SqlDbType.Int) { Value = Utilities.IsNull(paciente.NUMRESULT) });
            bdParameters.Add(new SqlParameter("@NumPag", SqlDbType.Int) { Value = Utilities.IsNull(paciente.NUMPAG) });
            bdParameters.Add(new SqlParameter { ParameterName = "@MaxPag", DbType = DbType.Int32, Direction = ParameterDirection.Output });

            string StoredProc = " exec SP_LISTA_PAG_PACIENTE " +
                                " @PACIENTE_CODIGO     ," +
                                " @PACIENTE_DNI        ," +
                                " @PACIENTE_NOMBRE     ," +
                                " @PACIENTE_APELLIDO   ," +
                                " @PACIENTE_EDAD       ," +
                                " @PACIENTE_GENERO     ," +
                                " @PACIENTE_ESTADO     ," +
                                " @ColOrder            ," +
                                " @NumResult           ," +
                                " @NumPag              ," +
                                " @MaxPag            OUT";
            var _task = await Task.Run(() => _pacientes = _context.PacienteModel.FromSqlRaw(@StoredProc, bdParameters.ToArray()).ToList());
            _result.Add(new ResultBuscar() { MaxPag = int.Parse(bdParameters[10].Value.ToString()), ListResult = _pacientes.Cast<Object>().ToList() });
            return _result;
        }

        [HttpPost("Registrar")]
        // POST: Registrar
        public async Task<ActionResult<PacienteModel>> Post(PacienteDataIn paciente)
        {
            List<PacienteModel> _listExist = new List<PacienteModel>();
            PacienteModel _result = new PacienteModel();
            var _taskExist = await Task.Run(() => GetByParam(new PacienteDataIn() { PACIENTE_DNI = paciente.PACIENTE_DNI }));
            if (_taskExist.Value.Count() > 0)
            {
                _result.PACIENTE_NOMBRE = "El DNI del paciente ya se encuentra registrado, coloque otro DNI.";
                return _result;
            };
            var bdParameters = new List<SqlParameter>();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<PacienteDataIn, PacienteModel>()));
            bdParameters.Add(new SqlParameter("@PACIENTE_DNI", SqlDbType.VarChar) { Value = Utilities.IsNull(paciente.PACIENTE_DNI) });
            bdParameters.Add(new SqlParameter("@PACIENTE_NOMBRE", SqlDbType.VarChar) { Value = Utilities.IsNull(paciente.PACIENTE_NOMBRE) });
            bdParameters.Add(new SqlParameter("@PACIENTE_APELLIDO", SqlDbType.VarChar) { Value = Utilities.IsNull(paciente.PACIENTE_APELLIDO) });
            bdParameters.Add(new SqlParameter("@PACIENTE_EDAD", SqlDbType.Int) { Value = Utilities.IsNull(paciente.PACIENTE_EDAD) });
            bdParameters.Add(new SqlParameter("@PACIENTE_GENERO", SqlDbType.VarChar) { Value = Utilities.IsNull(paciente.PACIENTE_GENERO) });
            bdParameters.Add(new SqlParameter("@PACIENTE_ESTADO", SqlDbType.VarChar) { Value = Utilities.IsNull(paciente.PACIENTE_ESTADO) });
            bdParameters.Add(new SqlParameter { ParameterName = "@PACIENTE_CODIGO", DbType = DbType.Int32, Direction = ParameterDirection.Output });
            string StoredProc = " exec SP_INSERT_PACIENTE " +
                                " @PACIENTE_DNI        ," +
                                " @PACIENTE_NOMBRE     ," +
                                " @PACIENTE_APELLIDO   ," +
                                " @PACIENTE_EDAD       ," +
                                " @PACIENTE_GENERO     ," +
                                " @PACIENTE_ESTADO     ," +
                                " @PACIENTE_CODIGO     OUT";
            var _task = await Task.Run(() => _context.Database.ExecuteSqlRaw(@StoredProc, bdParameters.ToArray()));
            _result = mapper.Map<PacienteModel>(paciente);
            _result.PACIENTE_CODIGO = int.Parse(bdParameters[6].Value.ToString());
            return _result;
        }

        [HttpPost("Actualizar")]
        // PUt: Actualizar
        public IActionResult PostUpdatePaciente(PacienteDataIn paciente)
        {
            MsgResult _msg = new MsgResult();
            if (paciente.PACIENTE_DNI == null)
            {
                _msg.Result = "ERROR";
                _msg.Content = "Ingrese el DNI del paciente.";
                return Json(_msg);
            }
            var bdParameters = new List<SqlParameter>();
            bdParameters.Add(new SqlParameter("@PACIENTE_DNI", SqlDbType.VarChar) { Value = Utilities.IsNull(paciente.PACIENTE_DNI) });
            bdParameters.Add(new SqlParameter("@PACIENTE_NOMBRE", SqlDbType.VarChar) { Value = Utilities.IsNull(paciente.PACIENTE_NOMBRE) });
            bdParameters.Add(new SqlParameter("@PACIENTE_APELLIDO", SqlDbType.VarChar) { Value = Utilities.IsNull(paciente.PACIENTE_APELLIDO) });
            bdParameters.Add(new SqlParameter("@PACIENTE_EDAD", SqlDbType.Int) { Value = Utilities.IsNull(paciente.PACIENTE_EDAD) });
            bdParameters.Add(new SqlParameter("@PACIENTE_GENERO", SqlDbType.VarChar) { Value = Utilities.IsNull(paciente.PACIENTE_GENERO) });
            bdParameters.Add(new SqlParameter("@PACIENTE_ESTADO", SqlDbType.VarChar) { Value = Utilities.IsNull(paciente.PACIENTE_ESTADO) });
            string StoredProc = " exec SP_UPDATE_PACIENTE " +
                                " @PACIENTE_DNI        ," +
                                " @PACIENTE_NOMBRE     ," +
                                " @PACIENTE_APELLIDO   ," +
                                " @PACIENTE_EDAD       ," +
                                " @PACIENTE_GENERO     ," +
                                " @PACIENTE_ESTADO     ";
            var _task = _context.Database.ExecuteSqlRaw(@StoredProc, bdParameters.ToArray());
            if (_task > 0)
            {
                _msg.Result = "SUCCESS";
                _msg.Content = "El paciente con DNI :" + paciente.PACIENTE_DNI + " ha sido actualizado.";
                return Json(_msg);
            }
            else
            {
                _msg.Result = "ERROR";
                _msg.Content = "Error al actualizar el paciente.";
                return Json(_msg);
            }
        }

        [HttpPost("Deshabilitar")]
        // PUt: Deshabilitar
        public IActionResult PostLogicDelPaciente(PacienteDataIn paciente)
        {
            MsgResult _msg = new MsgResult();
            if (paciente.PACIENTE_DNI == null)
            {
                _msg.Result = "ERROR";
                _msg.Content = "Ingrese el DNI del paciente.";
                return Json(_msg);
            }
            return PostUpdatePaciente(new PacienteDataIn() { PACIENTE_DNI = paciente.PACIENTE_DNI, PACIENTE_ESTADO = "I" });
        }

        [HttpPost("Eliminar")]
        // PUt: Eliminar
        public IActionResult PostDeletePaciente(PacienteDataIn paciente)
        {
            MsgResult _msg = new MsgResult();
            if (paciente.PACIENTE_DNI == null && paciente.PACIENTE_CODIGO == null)
            {
                _msg.Result = "ERROR";
                _msg.Content = "Ingrese el Codigo o DNI del paciente.";
                return Json(_msg);
            }
            paciente.PACIENTE_DNI = (paciente.PACIENTE_CODIGO != null) ? null : paciente.PACIENTE_DNI;
            var bdParameters = new List<SqlParameter>();
            bdParameters.Add(new SqlParameter("@PACIENTE_CODIGO", SqlDbType.Int) { Value = Utilities.IsNull(paciente.PACIENTE_CODIGO) });
            bdParameters.Add(new SqlParameter("@PACIENTE_DNI", SqlDbType.VarChar) { Value = Utilities.IsNull(paciente.PACIENTE_DNI) });
            string StoredProc = " exec SP_DELETE_PACIENTE " +
                                " @PACIENTE_CODIGO        ," +
                                " @PACIENTE_DNI     ";
            var _task = _context.Database.ExecuteSqlRaw(@StoredProc, bdParameters.ToArray());
            if (_task > 0)
            {
                _msg.Result = "SUCCESS";
                _msg.Content = "El paciente con " + ((paciente.PACIENTE_DNI == null) ? "Codigo" : "DNI") + ":" + ((paciente.PACIENTE_DNI == null) ? paciente.PACIENTE_CODIGO.ToString() : paciente.PACIENTE_DNI) + " ha sido eliminado.";
                return Json(_msg);
            }
            else
            {
                _msg.Result = "ERROR";
                _msg.Content = "Error al eliminar el paciente.";
                return Json(_msg);
            }
        }
    }
}
