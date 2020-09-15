﻿using IMD.Admin.Utilities.Business;
using IMD.Admin.Utilities.Entities;
using IMD.Meditoc.CallCenter.Mx.Business.CallCenter;
using IMD.Meditoc.CallCenter.Mx.Business.Consulta;
using IMD.Meditoc.CallCenter.Mx.Entities.CallCenter;
using IMD.Meditoc.CallCenter.Mx.Entities.Consultas;
using IMD.Meditoc.CallCenter.Mx.Web.Tokens;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IMD.Meditoc.CallCenter.Mx.Web.Controllers
{
    [MeditocAuthentication]
    public class CallCenterController : ApiController
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(CallCenterController));

        [HttpPost]
        [Route("Api/CallCenter/Set/Colaborador/Online")]
        public IMDResponse<bool> CCallCenterOnline(EntOnlineMod entOnlineMod)
        {
            IMDResponse<bool> response = new IMDResponse<bool>();

            string metodo = nameof(this.CCallCenterOnline);
            logger.Info(IMDSerialize.Serialize(67823458510633, $"Inicia {metodo}"));

            try
            {
                BusCallCenter busCallCenter = new BusCallCenter();
                response = busCallCenter.BCallCenterOnline(entOnlineMod);
            }
            catch (Exception ex)
            {
                response.Code = 67823458511410;
                response.Message = "Ocurrió un error inesperado";

                logger.Error(IMDSerialize.Serialize(67823458511410, $"Error en {metodo}: {ex.Message}", ex, response));
            }
            return response;
        }

        [HttpPost]
        [Route("Api/CallCenter/Start/Service/WithFolio")]
        public IMDResponse<EntCallCenter> CCallCenterStartWithFolio(int iIdColaborador, string sFolio)
        {
            IMDResponse<EntCallCenter> response = new IMDResponse<EntCallCenter>();

            string metodo = nameof(this.CCallCenterStartWithFolio);
            logger.Info(IMDSerialize.Serialize(67823458512187, $"Inicia {metodo}"));

            try
            {
                BusCallCenter busCallCenter = new BusCallCenter();
                response = busCallCenter.BCallCenterStartWithFolio(iIdColaborador, sFolio);
            }
            catch (Exception ex)
            {
                response.Code = 67823458512964;
                response.Message = "Ocurrió un error inesperado";

                logger.Error(IMDSerialize.Serialize(67823458512964, $"Error en {metodo}: {ex.Message}", ex, response));
            }
            return response;
        }

        [HttpPost]
        [Route("Api/CallCenter/Save/Consulta")]
        public IMDResponse<EntConsulta> CSaveConsulta(EntConsulta entConsulta)
        {
            IMDResponse<EntConsulta> response = new IMDResponse<EntConsulta>();

            string metodo = nameof(this.CSaveConsulta);
            logger.Info(IMDSerialize.Serialize(67823458521511, $"Inicia {metodo}"));

            try
            {
                BusConsulta busConsulta = new BusConsulta();
                response = busConsulta.BSaveConsulta(entConsulta);
            }
            catch (Exception ex)
            {
                response.Code = 67823458522288;
                response.Message = "Ocurrió un error inesperado";

                logger.Error(IMDSerialize.Serialize(67823458522288, $"Error en {metodo}: {ex.Message}", ex, response));
            }
            return response;
        }

        [HttpGet]
        [Route("Api/CallCenter/Get/Consulta/Detalle")]
        public IMDResponse<List<EntDetalleConsulta>> CGetDetalleConsulta(int? piIdConsulta = null, int? piIdPaciente = null, int? piIdColaborador = null, int? piIdEstatusConsulta = null, DateTime? pdtFechaProgramadaInicio = null, DateTime? pdtFechaProgramadaFin = null, DateTime? pdtFechaConsultaInicio = null, DateTime? pdtFechaConsultaFin = null)
        {
            IMDResponse<List<EntDetalleConsulta>> response = new IMDResponse<List<EntDetalleConsulta>>();

            string metodo = nameof(this.CGetDetalleConsulta);
            logger.Info(IMDSerialize.Serialize(67823458532389, $"Inicia {metodo}"));

            try
            {
                BusConsulta busConsulta = new BusConsulta();
                response = busConsulta.BGetDetalleConsulta(piIdConsulta, piIdPaciente, piIdColaborador, piIdEstatusConsulta, pdtFechaProgramadaInicio, pdtFechaProgramadaFin, pdtFechaConsultaInicio, pdtFechaConsultaFin);
            }
            catch (Exception ex)
            {
                response.Code = 67823458533166;
                response.Message = "Ocurrió un error inesperado";

                logger.Error(IMDSerialize.Serialize(67823458533166, $"Error en {metodo}: {ex.Message}", ex, response));
            }
            return response;
        }
    }
}