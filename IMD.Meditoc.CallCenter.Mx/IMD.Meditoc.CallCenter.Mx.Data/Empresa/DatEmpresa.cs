﻿using IMD.Admin.Utilities.Business;
using IMD.Admin.Utilities.Data;
using IMD.Admin.Utilities.Entities;
using IMD.Meditoc.CallCenter.Mx.Entities.Empresa;
using log4net;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;

namespace IMD.Meditoc.CallCenter.Mx.Data.Empresa
{
    public class DatEmpresa
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(DatEmpresa));
        private Database database;
        IMDCommonData imdCommonData;
        string spSaveEmpresa;
        string spGetEmpresa;

        public DatEmpresa()
        {
            imdCommonData = new IMDCommonData();
            string FsConnectionString = "cnxMeditoc";
            database = imdCommonData.DGetDatabase(FsConnectionString, "MeditocComercial", "Meditoc1");

            spSaveEmpresa = "sva_meditoc_save_empresa";
            spGetEmpresa = "svc_meditoc_empresa";
        }

        public IMDResponse<DataTable> DSaveEmpresa(EntEmpresa entEmpresa)
        {
            IMDResponse<DataTable> response = new IMDResponse<DataTable>();

            string metodo = nameof(this.DSaveEmpresa);
            logger.Info(IMDSerialize.Serialize(67823458382428, $"Inicia {metodo}(EntEmpresa entEmpresa)", entEmpresa));

            try
            {
                using (DbCommand dbCommand = database.GetStoredProcCommand(spSaveEmpresa))
                {
                    database.AddInParameter(dbCommand, "piIdEmpresa", DbType.Int32, entEmpresa.iIdEmpresa);
                    database.AddInParameter(dbCommand, "psNombre", DbType.String, entEmpresa.sNombre);
                    database.AddInParameter(dbCommand, "psCorreo", DbType.String, entEmpresa.sCorreo);
                    database.AddInParameter(dbCommand, "piIdUsuarioMod", DbType.String, entEmpresa.iIdUsuarioMod);
                    database.AddInParameter(dbCommand, "pbActivo", DbType.Boolean, entEmpresa.bActivo);
                    database.AddInParameter(dbCommand, "pbBaja", DbType.Boolean, entEmpresa.bBaja);

                    response = imdCommonData.DExecuteDT(database, dbCommand);
                }
            }
            catch (Exception ex)
            {
                response.Code = 67823458383205;
                response.Message = "Ocurrió un error inesperado en la base de datos al guardar la empresa.";

                logger.Error(IMDSerialize.Serialize(67823458383205, $"Error en {metodo}(EntEmpresa entEmpresa): {ex.Message}", entEmpresa, ex, response));
            }
            return response;
        }


        public IMDResponse<DataTable> DGetEmpresas(int? iIdEmpresa, string psCorreo = null, string psFolioEmpresa = null)
        {
            IMDResponse<DataTable> response = new IMDResponse<DataTable>();

            string metodo = nameof(this.DGetEmpresas);
            logger.Info(IMDSerialize.Serialize(67823458390198, $"Inicia {metodo}(int? iIdEmpresa, string psCorreo = null, string psFolioEmpresa = null)", iIdEmpresa, psCorreo, psFolioEmpresa));

            try
            {
                using (DbCommand dbCommand = database.GetStoredProcCommand(spGetEmpresa))
                {
                    database.AddInParameter(dbCommand, "piIdEmpresa", DbType.Int32, iIdEmpresa);
                    database.AddInParameter(dbCommand, "psCorreo", DbType.String, psCorreo);
                    database.AddInParameter(dbCommand, "psFolioEmpresa", DbType.String, psFolioEmpresa);

                    response = imdCommonData.DExecuteDT(database, dbCommand);
                }
            }
            catch (Exception ex)
            {
                response.Code = 67823458390975;
                response.Message = "Ocurrió un error inesperado en la base de datos al consutar las empresas.";

                logger.Error(IMDSerialize.Serialize(67823458390975, $"Error en {metodo}(int? iIdEmpresa, string psCorreo = null, string psFolioEmpresa = null): {ex.Message}", iIdEmpresa, psCorreo, psFolioEmpresa, ex, response));
            }
            return response;
        }
    }
}
