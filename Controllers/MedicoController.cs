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
    public class MedicoController : Controller
    {
        private readonly Conexion _context;

        public MedicoController(Conexion context)
        {
            _context = context;
        }
        public async Task<ActionResult<IEnumerable<MedicoModel>>> Get()
        {
            return await Task.Run(() => GetByParam(new MedicoDataIn() { }));
        }

        [HttpGet("Buscar")]
        // GET: Buscar
        public async Task<ActionResult<IEnumerable<MedicoModel>>> GetByParam(MedicoDataIn medico)
        {
            
            var bdParameters = new List<SqlParameter>();
            bdParameters.Add(new SqlParameter("@MEDICO_CODIGO", SqlDbType.Int) { Value = Utilities.IsNull(medico.MEDICO_CODIGO) });
            bdParameters.Add(new SqlParameter("@MEDICO_DNI", SqlDbType.VarChar) { Value = Utilities.IsNull(medico.MEDICO_DNI) });
            bdParameters.Add(new SqlParameter("@MEDICO_NOMBRE", SqlDbType.VarChar) { Value = Utilities.IsNull(medico.MEDICO_NOMBRE) });
            bdParameters.Add(new SqlParameter("@MEDICO_APELLIDO", SqlDbType.VarChar) { Value = Utilities.IsNull(medico.MEDICO_APELLIDO) });
            bdParameters.Add(new SqlParameter("@MEDICO_EDAD", SqlDbType.Int) { Value = Utilities.IsNull(medico.MEDICO_EDAD) });
            bdParameters.Add(new SqlParameter("@MEDICO_GENERO", SqlDbType.VarChar) { Value = Utilities.IsNull(medico.MEDICO_GENERO) });
            bdParameters.Add(new SqlParameter("@MEDICO_CONSULTORIO", SqlDbType.VarChar) { Value = Utilities.IsNull(medico.MEDICO_CONSULTORIO) });
            bdParameters.Add(new SqlParameter("@MEDICO_TURNO", SqlDbType.VarChar) { Value = Utilities.IsNull(medico.MEDICO_TURNO) });
            bdParameters.Add(new SqlParameter("@MEDICO_ESPECIALIDAD", SqlDbType.Int) { Value = Utilities.IsNull(medico.MEDICO_ESPECIALIDAD) });
            bdParameters.Add(new SqlParameter("@MEDICO_ESTADO", SqlDbType.VarChar) { Value = Utilities.IsNull(medico.MEDICO_ESTADO) });
            bdParameters.Add(new SqlParameter("@ColOrder", SqlDbType.Int) { Value = Utilities.IsNull(medico.COLORDER) });


            string StoredProc = " exec SP_LISTA_MEDICO " +
                                " @MEDICO_CODIGO       ," +
                                " @MEDICO_DNI          ," +
                                " @MEDICO_NOMBRE       ," +
                                " @MEDICO_APELLIDO     ," +
                                " @MEDICO_EDAD         ," +
                                " @MEDICO_GENERO       ," +
                                " @MEDICO_CONSULTORIO  ," +
                                " @MEDICO_TURNO        ," +
                                " @MEDICO_ESPECIALIDAD ," +
                                " @MEDICO_ESTADO       ," +
                                " @ColOrder            ";
            return await _context.MedicoModel.FromSqlRaw(StoredProc, bdParameters.ToArray()).ToListAsync();
        }

        [HttpGet("BuscarPag")]
        // GET: BuscarPag
        public async Task<ActionResult<IEnumerable<ResultBuscar>>> GetByParamPag(MedicoDataIn medico)
        {            
            List<ResultBuscar> _result = new List<ResultBuscar>();
            List<MedicoModel> _medicos = new List<MedicoModel>();
            var bdParameters = new List<SqlParameter>();
            bdParameters.Add(new SqlParameter("@MEDICO_CODIGO", SqlDbType.Int) { Value = Utilities.IsNull(medico.MEDICO_CODIGO) });
            bdParameters.Add(new SqlParameter("@MEDICO_DNI", SqlDbType.VarChar) { Value = Utilities.IsNull(medico.MEDICO_DNI) });
            bdParameters.Add(new SqlParameter("@MEDICO_NOMBRE", SqlDbType.VarChar) { Value = Utilities.IsNull(medico.MEDICO_NOMBRE) });
            bdParameters.Add(new SqlParameter("@MEDICO_APELLIDO", SqlDbType.VarChar) { Value = Utilities.IsNull(medico.MEDICO_APELLIDO) });
            bdParameters.Add(new SqlParameter("@MEDICO_EDAD", SqlDbType.Int) { Value = Utilities.IsNull(medico.MEDICO_EDAD) });
            bdParameters.Add(new SqlParameter("@MEDICO_GENERO", SqlDbType.VarChar) { Value = Utilities.IsNull(medico.MEDICO_GENERO) });
            bdParameters.Add(new SqlParameter("@MEDICO_CONSULTORIO", SqlDbType.VarChar) { Value = Utilities.IsNull(medico.MEDICO_CONSULTORIO) });
            bdParameters.Add(new SqlParameter("@MEDICO_TURNO", SqlDbType.VarChar) { Value = Utilities.IsNull(medico.MEDICO_TURNO) });
            bdParameters.Add(new SqlParameter("@MEDICO_ESPECIALIDAD", SqlDbType.Int) { Value = Utilities.IsNull(medico.MEDICO_ESPECIALIDAD) });
            bdParameters.Add(new SqlParameter("@MEDICO_ESTADO", SqlDbType.VarChar) { Value = Utilities.IsNull(medico.MEDICO_ESTADO) });
            bdParameters.Add(new SqlParameter("@ColOrder", SqlDbType.Int) { Value = Utilities.IsNull(medico.COLORDER) });
            bdParameters.Add(new SqlParameter("@NumResult", SqlDbType.Int) { Value = Utilities.IsNull(medico.NUMRESULT) });
            bdParameters.Add(new SqlParameter("@NumPag", SqlDbType.Int) { Value = Utilities.IsNull(medico.NUMPAG) });
            bdParameters.Add(new SqlParameter { ParameterName = "@MaxPag", DbType = DbType.Int32, Direction = ParameterDirection.Output });

            string StoredProc = " exec SP_LISTA_PAG_MEDICO " +
                                " @MEDICO_CODIGO       ," +
                                " @MEDICO_DNI          ," +
                                " @MEDICO_NOMBRE       ," +
                                " @MEDICO_APELLIDO     ," +
                                " @MEDICO_EDAD         ," +
                                " @MEDICO_GENERO       ," +
                                " @MEDICO_CONSULTORIO  ," +
                                " @MEDICO_TURNO        ," +
                                " @MEDICO_ESPECIALIDAD ," +
                                " @MEDICO_ESTADO       ," +
                                " @ColOrder            ," +
                                " @NumResult           ," +
                                " @NumPag              ," +
                                " @MaxPag            OUT";            
            var _task = await Task.Run(() => _medicos = _context.MedicoModel.FromSqlRaw(@StoredProc, bdParameters.ToArray()).ToList());
            _result.Add(new ResultBuscar() { MaxPag = int.Parse(bdParameters[13].Value.ToString()), ListResult = _medicos.Cast<Object>().ToList() });
            return _result;
        }

        [HttpPost("Registrar")]
        // POST: Registrar
        public async Task<ActionResult<MedicoModel>> Post(MedicoDataIn medico)
        {
            List<MedicoModel> _listExist = new List<MedicoModel>();
            MedicoModel _result = new MedicoModel();
            var _taskExist = await Task.Run(() => GetByParam(new MedicoDataIn() { MEDICO_DNI = medico.MEDICO_DNI }));
            if (_taskExist.Value.Count() > 0)
            {
                _result.MEDICO_NOMBRE = "El DNI del médico ya se encuentra registrado, coloque otro DNI.";
                return _result;
            };
            var bdParameters = new List<SqlParameter>();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<MedicoDataIn, MedicoModel>()));            
            bdParameters.Add(new SqlParameter("@MEDICO_DNI", SqlDbType.VarChar) { Value = Utilities.IsNull(medico.MEDICO_DNI) });
            bdParameters.Add(new SqlParameter("@MEDICO_NOMBRE", SqlDbType.VarChar) { Value = Utilities.IsNull(medico.MEDICO_NOMBRE) });
            bdParameters.Add(new SqlParameter("@MEDICO_APELLIDO", SqlDbType.VarChar) { Value = Utilities.IsNull(medico.MEDICO_APELLIDO) });
            bdParameters.Add(new SqlParameter("@MEDICO_EDAD", SqlDbType.Int) { Value = Utilities.IsNull(medico.MEDICO_EDAD) });
            bdParameters.Add(new SqlParameter("@MEDICO_GENERO", SqlDbType.VarChar) { Value = Utilities.IsNull(medico.MEDICO_GENERO) });
            bdParameters.Add(new SqlParameter("@MEDICO_CONSULTORIO", SqlDbType.VarChar) { Value = Utilities.IsNull(medico.MEDICO_CONSULTORIO) });
            bdParameters.Add(new SqlParameter("@MEDICO_TURNO", SqlDbType.VarChar) { Value = Utilities.IsNull(medico.MEDICO_TURNO) });
            bdParameters.Add(new SqlParameter("@MEDICO_ESPECIALIDAD", SqlDbType.Int) { Value = Utilities.IsNull(medico.MEDICO_ESPECIALIDAD) });
            bdParameters.Add(new SqlParameter("@MEDICO_ESTADO", SqlDbType.VarChar) { Value = Utilities.IsNull(medico.MEDICO_ESTADO) });
            bdParameters.Add(new SqlParameter { ParameterName = "@MEDICO_CODIGO", DbType = DbType.Int32, Direction = ParameterDirection.Output });
            string StoredProc = " exec SP_INSERT_MEDICO " +
                                " @MEDICO_DNI          ," +
                                " @MEDICO_NOMBRE       ," +
                                " @MEDICO_APELLIDO     ," +
                                " @MEDICO_EDAD         ," +
                                " @MEDICO_GENERO       ," +
                                " @MEDICO_CONSULTORIO  ," +
                                " @MEDICO_TURNO        ," +
                                " @MEDICO_ESPECIALIDAD ," +
                                " @MEDICO_ESTADO       ," +
                                " @MEDICO_CODIGO     OUT";
            var _task = await Task.Run(() => _context.Database.ExecuteSqlRaw(@StoredProc, bdParameters.ToArray()));
            _result = mapper.Map<MedicoModel>(medico);
            _result.MEDICO_CODIGO = int.Parse(bdParameters[9].Value.ToString());
            return _result;
        }

        [HttpPost("Actualizar")]
        // PUt: Actualizar
        public IActionResult PostUpdateMedico(MedicoDataIn medico)
        {
            MsgResult _msg = new MsgResult();
            if (medico.MEDICO_CODIGO == null)
            {
                _msg.Result = "ERROR";
                _msg.Content = "Ingrese el Código del médico.";
                return Json(_msg);
            }
            var bdParameters = new List<SqlParameter>();
            bdParameters.Add(new SqlParameter("@MEDICO_CODIGO", SqlDbType.Int) { Value = Utilities.IsNull(medico.MEDICO_CODIGO) });
            bdParameters.Add(new SqlParameter("@MEDICO_DNI", SqlDbType.VarChar) { Value = Utilities.IsNull(medico.MEDICO_DNI) });
            bdParameters.Add(new SqlParameter("@MEDICO_NOMBRE", SqlDbType.VarChar) { Value = Utilities.IsNull(medico.MEDICO_NOMBRE) });
            bdParameters.Add(new SqlParameter("@MEDICO_APELLIDO", SqlDbType.VarChar) { Value = Utilities.IsNull(medico.MEDICO_APELLIDO) });
            bdParameters.Add(new SqlParameter("@MEDICO_EDAD", SqlDbType.Int) { Value = Utilities.IsNull(medico.MEDICO_EDAD) });
            bdParameters.Add(new SqlParameter("@MEDICO_GENERO", SqlDbType.VarChar) { Value = Utilities.IsNull(medico.MEDICO_GENERO) });
            bdParameters.Add(new SqlParameter("@MEDICO_CONSULTORIO", SqlDbType.VarChar) { Value = Utilities.IsNull(medico.MEDICO_CONSULTORIO) });
            bdParameters.Add(new SqlParameter("@MEDICO_TURNO", SqlDbType.VarChar) { Value = Utilities.IsNull(medico.MEDICO_TURNO) });
            bdParameters.Add(new SqlParameter("@MEDICO_ESPECIALIDAD", SqlDbType.Int) { Value = Utilities.IsNull(medico.MEDICO_ESPECIALIDAD) });
            bdParameters.Add(new SqlParameter("@MEDICO_ESTADO", SqlDbType.VarChar) { Value = Utilities.IsNull(medico.MEDICO_ESTADO) });
            string StoredProc = " exec SP_UPDATE_MEDICO " +
                                " @MEDICO_CODIGO       ," +
                                " @MEDICO_DNI          ," +
                                " @MEDICO_NOMBRE       ," +
                                " @MEDICO_APELLIDO     ," +
                                " @MEDICO_EDAD         ," +
                                " @MEDICO_GENERO       ," +
                                " @MEDICO_CONSULTORIO  ," +
                                " @MEDICO_TURNO        ," +
                                " @MEDICO_ESPECIALIDAD ," +
                                " @MEDICO_ESTADO     ";
            var _task = _context.Database.ExecuteSqlRaw(@StoredProc, bdParameters.ToArray());
            if (_task > 0)
            {
                _msg.Result = "SUCCESS";
                _msg.Content = "El médico con Código :" + medico.MEDICO_CODIGO + " ha sido actualizado.";
                return Json(_msg);
            }
            else
            {
                _msg.Result = "ERROR";
                _msg.Content = "Error al actualizar el médico.";
                return Json(_msg);
            }
        }

        [HttpPost("Deshabilitar")]
        // PUt: Deshabilitar
        public IActionResult PostLogicDelMedico(MedicoDataIn medico)
        {
            MsgResult _msg = new MsgResult();
            if (medico.MEDICO_CODIGO == null)
            {
                _msg.Result = "ERROR";
                _msg.Content = "Ingrese el Código del médico.";
                return Json(_msg);
            }
            return PostUpdateMedico(new MedicoDataIn() { MEDICO_CODIGO = medico.MEDICO_CODIGO, MEDICO_ESTADO = "I" });
        }

        [HttpPost("Eliminar")]
        // PUt: Eliminar
        public IActionResult PostDeleteMedico(MedicoDataIn medico)
        {
            MsgResult _msg = new MsgResult();
            if (medico.MEDICO_DNI == null && medico.MEDICO_CODIGO == null)
            {
                _msg.Result = "ERROR";
                _msg.Content = "Ingrese el Código o DNI del médico.";
                return Json(_msg);
            }
            medico.MEDICO_DNI = (medico.MEDICO_CODIGO != null) ? null : medico.MEDICO_DNI;
            var bdParameters = new List<SqlParameter>();
            bdParameters.Add(new SqlParameter("@MEDICO_CODIGO", SqlDbType.Int) { Value = Utilities.IsNull(medico.MEDICO_CODIGO) });
            bdParameters.Add(new SqlParameter("@MEDICO_DNI", SqlDbType.VarChar) { Value = Utilities.IsNull(medico.MEDICO_DNI) });
            string StoredProc = " exec SP_DELETE_MEDICO " +
                                " @MEDICO_CODIGO        ," +
                                " @MEDICO_DNI     ";
            var _task = _context.Database.ExecuteSqlRaw(@StoredProc, bdParameters.ToArray());
            if (_task > 0)
            {
                _msg.Result = "SUCCESS";
                _msg.Content = "El médico con " + ((medico.MEDICO_DNI == null) ? "Código" : "DNI") + ":" + ((medico.MEDICO_DNI == null) ? medico.MEDICO_CODIGO.ToString() : medico.MEDICO_DNI) + " ha sido eliminado.";
                return Json(_msg);
            }
            else
            {
                _msg.Result = "ERROR";
                _msg.Content = "Error al eliminar el médico.";
                return Json(_msg);
            }
        }

    }
}
