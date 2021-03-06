﻿using IMD.Admin.Utilities.Business;
using IMD.Admin.Utilities.Entities;
using IMD.Meditoc.CallCenter.Mx.Business;
using IMD.Meditoc.CallCenter.Mx.Business.Catalogos;
using IMD.Meditoc.CallCenter.Mx.Entities;
using IMD.Meditoc.CallCenter.Mx.Entities.Catalogos;
using IMD.Meditoc.CallCenter.Mx.Web.Tokens;
using log4net;
using System;
using System.Web.Http;

namespace IMD.Meditoc.CallCenter.Mx.Web.Controllers
{
    public class PoliticasController : ApiController
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(PoliticasController));

        [HttpGet]
        [Route("Api/Politicas/Get/Politicas")]
        public IMDResponse<EntPoliticas> CGetPoliticas()
        {
            IMDResponse<EntPoliticas> response = new IMDResponse<EntPoliticas>();

            string metodo = nameof(this.CGetPoliticas);
            logger.Info(IMDSerialize.Serialize(67823458379320, $"Inicia {metodo}()"));

            try
            {
                BusPoliticas busPoliticas = new BusPoliticas();

                response = busPoliticas.BObtenerPoliticas();
            }
            catch (Exception ex)
            {
                response.Code = 67823458380097;
                response.Message = "Ocurrió un error inesperado en el servicio al obtener la información pública del sistema.";

                logger.Error(IMDSerialize.Serialize(67823458380097, $"Error en {metodo}(): {ex.Message}", ex, response));
            }
            return response;
        }

        [MeditocAuthentication]
        [HttpGet]
        [Route("Api/Politicas/Get/Sistema/Catalogos")]
        public IMDResponse<EntCatalogos> CGetCatalogosSistema()
        {
            IMDResponse<EntCatalogos> response = new IMDResponse<EntCatalogos>();

            string metodo = nameof(this.CGetCatalogosSistema);
            logger.Info(IMDSerialize.Serialize(67823458645831, $"Inicia {metodo}"));

            try
            {
                BusCatalogo busCatalogo = new BusCatalogo();
                response = busCatalogo.BGetCatalogos();
            }
            catch (Exception ex)
            {
                response.Code = 67823458646608;
                response.Message = "Ocurrió un error inesperado";

                logger.Error(IMDSerialize.Serialize(67823458646608, $"Error en {metodo}: {ex.Message}", ex, response));
            }
            return response;
        }
    }
}
