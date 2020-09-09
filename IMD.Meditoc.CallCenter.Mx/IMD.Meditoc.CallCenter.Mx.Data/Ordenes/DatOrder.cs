﻿using IMD.Admin.Conekta.Entities;
using IMD.Admin.Utilities.Business;
using IMD.Admin.Utilities.Data;
using IMD.Admin.Utilities.Entities;
using log4net;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace IMD.Admin.Conekta.Data
{
    public class DatOrder
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(DatOrder));

#if DEBUG
        private Database database;
        IMDCommonData imdCommonData;
        private string spSaveConektaOrder;
        private string spSaveCustomerInfo;
        private string spSaveLineItem;
        private string spSaveCharge;
        private string spGetConektaOrder;
#else
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Database database;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        IMDCommonData imdCommonData;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string spSaveConektaOrder;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string spSaveCustomerInfo;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string spSaveLineItem;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string spSaveCharge;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string spGetConektaOrder;
#endif
        public DatOrder()
        {
            imdCommonData = new IMDCommonData();
            string FsConnectionString = "cnxConekta";
            database = imdCommonData.DGetDatabase(FsConnectionString, "hdiu4soi3IHD334F", "SKlru3nc");

            switch (ConfigurationManager.ConnectionStrings[FsConnectionString].ProviderName)
            {
                case "System.Data.SqlClient":
                    spSaveConektaOrder = "Orders.svaSaveConektaOrder";
                    spSaveCustomerInfo = "Orders.svaSaveCustomerInfo";
                    spSaveLineItem = "Orders.svaSaveLineItem";
                    spSaveCharge = "Payments.svaSaveCharge";
                    spGetConektaOrder = "Orders.svcGetConektaOrder";
                    break;
                case "MySql.Data.MySqlClient":
                    spSaveConektaOrder = "sva_save_conekta_order";
                    spSaveCustomerInfo = "sva_save_customer_info";
                    spSaveLineItem = "sva_save_line_item";
                    spSaveCharge = "sva_save_charge";
                    spGetConektaOrder = "svc_get_conekta_order";
                    break;
                default:
                    throw new Exception("No se ha especificado el motor de base de datos");
            }
        }

        /// <summary>
        /// Función: Guarda los datos de la orden en la base de datos
        /// Creado: Cristopher Noh 03/07/2020
        /// Modificado:
        /// </summary>
        /// <param name="puId">ID generado de la orden</param>
        /// <param name="entOrder">Datos de la orden</param>
        /// <returns></returns>
        public IMDResponse<bool> DSaveConektaOrder(Guid puId, EntOrder entOrder, string psOrigin, int? piIdCupon = null)
        {
            IMDResponse<bool> response = new IMDResponse<bool>();

            string metodo = nameof(this.DSaveConektaOrder);
            logger.Info(IMDSerialize.Serialize(67823458097269, $"Inicia {metodo}(Guid puId, EntOrder entOrder)", puId, entOrder));

            try
            {
                using (DbCommand dbCommand = database.GetStoredProcCommand(spSaveConektaOrder))
                {
                    database.AddInParameter(dbCommand, "puId", DbType.Guid, puId);
                    database.AddInParameter(dbCommand, "psOrderId", DbType.String, entOrder.id);
                    database.AddInParameter(dbCommand, "pnAmount", DbType.Decimal, entOrder.amount);
                    database.AddInParameter(dbCommand, "pnAmountPaid", DbType.Decimal, entOrder.amount_paid);
                    database.AddInParameter(dbCommand, "pnAmountRefunded", DbType.Decimal, entOrder.amount_refunded);
                    database.AddInParameter(dbCommand, "psCurrency", DbType.String, entOrder.currency);
                    database.AddInParameter(dbCommand, "psPaymentStatus", DbType.String, entOrder.payment_status);
                    database.AddInParameter(dbCommand, "psOrigin", DbType.String, psOrigin);
                    database.AddInParameter(dbCommand, "piIdCupon", DbType.Int32, piIdCupon);
                    database.AddInParameter(dbCommand, "piCreated", DbType.Int32, entOrder.created_at);
                    database.AddInParameter(dbCommand, "piUpdated", DbType.Int32, entOrder.updated_at);

                    response = imdCommonData.DExecute(database, dbCommand);
                }
            }
            catch (Exception ex)
            {
                response.Code = 67823458098046;
                response.Message = "Ocurrió un error inesperado al guardar el detalle de la orden en la base de datos";

                logger.Error(IMDSerialize.Serialize(67823458098046, $"Error en {metodo}(Guid puId, EntOrder entOrder): {ex.Message}", puId, entOrder, ex, response));
            }
            return response;
        }

        /// <summary>
        /// Función: Guarda los datos del comprador
        /// Creado: Cristopher Noh 03/07/2020
        /// Modificado:
        /// </summary>
        /// <param name="puId">ID generado de la orden</param>
        /// <param name="entCustomerInfo">Datos del cliente</param>
        /// <returns></returns>
        public IMDResponse<bool> DSaveCustomerInfo(Guid puId, EntCustomerInfo entCustomerInfo)
        {
            IMDResponse<bool> response = new IMDResponse<bool>();

            string metodo = nameof(this.DSaveCustomerInfo);
            logger.Info(IMDSerialize.Serialize(67823458098823, $"Inicia {metodo}(Guid puId, EntCustomerInfo entCustomerInfo)", puId, entCustomerInfo));

            try
            {
                using (DbCommand dbCommand = database.GetStoredProcCommand(spSaveCustomerInfo))
                {
                    database.AddInParameter(dbCommand, "puId", DbType.Guid, puId);
                    database.AddInParameter(dbCommand, "psEmail", DbType.String, entCustomerInfo.email);
                    database.AddInParameter(dbCommand, "psPhone", DbType.String, entCustomerInfo.phone);
                    database.AddInParameter(dbCommand, "psName", DbType.String, entCustomerInfo.name);

                    response = imdCommonData.DExecute(database, dbCommand);
                }
            }
            catch (Exception ex)
            {
                response.Code = 67823458099600;
                response.Message = "Ocurrió un error inesperado al guardar el detalle del comprador en la base de datos";

                logger.Error(IMDSerialize.Serialize(67823458099600, $"Error en {metodo}(Guid puId, EntCustomerInfo entCustomerInfo): {ex.Message}", puId, entCustomerInfo, ex, response));
            }
            return response;
        }

        /// <summary>
        /// Función: Guarda los datos del artículo
        /// Creado: Cristopher Noh 02/07/2020
        /// Modificado:
        /// </summary>
        /// <param name="puId">ID generado de la orden</param>
        /// <param name="piConsecutive">Consecutivo generado del artículo</param>
        /// <param name="entLineItemDetail">Datos del artículo</param>
        /// <returns></returns>
        public IMDResponse<bool> DSaveLineItem(Guid puId, int piConsecutive, EntLineItemDetail entLineItemDetail)
        {
            IMDResponse<bool> response = new IMDResponse<bool>();

            string metodo = nameof(this.DSaveLineItem);
            logger.Info(IMDSerialize.Serialize(67823458100377, $"Inicia {metodo}(Guid puId, int piConsecutive, EntLineItemDetail entLineItemDetail)", puId, piConsecutive, entLineItemDetail));

            try
            {
                using (DbCommand dbCommand = database.GetStoredProcCommand(spSaveLineItem))
                {
                    database.AddInParameter(dbCommand, "puId", DbType.Guid, puId);
                    database.AddInParameter(dbCommand, "piConsecutive", DbType.Int32, piConsecutive);
                    database.AddInParameter(dbCommand, "psItemId", DbType.String, entLineItemDetail.id);
                    database.AddInParameter(dbCommand, "psName", DbType.String, entLineItemDetail.name);
                    database.AddInParameter(dbCommand, "pnUnitPrice", DbType.Decimal, entLineItemDetail.unit_price);
                    database.AddInParameter(dbCommand, "piQuantity", DbType.Int32, entLineItemDetail.quantity);

                    response = imdCommonData.DExecute(database, dbCommand);
                }
            }
            catch (Exception ex)
            {
                response.Code = 67823458101154;
                response.Message = "Ocurrió un error inesperado al guardar el detalle del artículo en la base de datos";

                logger.Error(IMDSerialize.Serialize(67823458101154, $"Error en {metodo}(Guid puId, int piConsecutive, EntLineItemDetail entLineItemDetail): {ex.Message}", puId, piConsecutive, entLineItemDetail, ex, response));
            }
            return response;
        }

        /// <summary>
        /// Función: Guarda los datos del pago en la base de datos
        /// Creado: Cristopher Noh 02/07/2020
        /// Modificado:
        /// </summary>
        /// <param name="puId">ID generado de la orden</param>
        /// <param name="entChargeDetail">Datos de pago</param>
        /// <returns></returns>
        public IMDResponse<bool> DSaveCharge(Guid puId, EntChargeDetail entChargeDetail, string psOrigin)
        {
            IMDResponse<bool> response = new IMDResponse<bool>();

            string metodo = nameof(this.DSaveCharge);
            logger.Info(IMDSerialize.Serialize(67823458101931, $"Inicia {metodo}(Guid puId, EntChargeDetail entChargeDetail)", puId, entChargeDetail));

            try
            {
                using (DbCommand dbCommand = database.GetStoredProcCommand(spSaveCharge))
                {
                    database.AddInParameter(dbCommand, "puId", DbType.Guid, puId);
                    database.AddInParameter(dbCommand, "psChargeId", DbType.String, entChargeDetail.id);
                    database.AddInParameter(dbCommand, "psDeviceFingerprint", DbType.String, entChargeDetail.device_fingerprint);
                    database.AddInParameter(dbCommand, "psName", DbType.String, entChargeDetail.payment_method.name);
                    database.AddInParameter(dbCommand, "psExpMonth", DbType.String, entChargeDetail.payment_method.exp_month);
                    database.AddInParameter(dbCommand, "psExpYear", DbType.String, entChargeDetail.payment_method.exp_year);
                    database.AddInParameter(dbCommand, "psAuthCode", DbType.String, entChargeDetail.payment_method.auth_code);
                    database.AddInParameter(dbCommand, "psType", DbType.String, entChargeDetail.payment_method.type);
                    database.AddInParameter(dbCommand, "psLast4", DbType.String, entChargeDetail.payment_method.last4);
                    database.AddInParameter(dbCommand, "psBrand", DbType.String, entChargeDetail.payment_method.brand);
                    database.AddInParameter(dbCommand, "psIssuer", DbType.String, entChargeDetail.payment_method.issuer);
                    database.AddInParameter(dbCommand, "psAccountType", DbType.String, entChargeDetail.payment_method.account_type);
                    database.AddInParameter(dbCommand, "piMonthlyInstallments", DbType.Int32, entChargeDetail.monthly_installments);
                    database.AddInParameter(dbCommand, "psCountry", DbType.String, entChargeDetail.payment_method.country);
                    database.AddInParameter(dbCommand, "psServiceName", DbType.String, entChargeDetail.payment_method.service_name);
                    database.AddInParameter(dbCommand, "psBarcodeUrl", DbType.String, entChargeDetail.payment_method.barcode_url);
                    database.AddInParameter(dbCommand, "piExpiresAt", DbType.Int32, entChargeDetail.payment_method.expires_at);
                    database.AddInParameter(dbCommand, "psStoreName", DbType.String, entChargeDetail.payment_method.store_name);
                    database.AddInParameter(dbCommand, "psReference", DbType.String, entChargeDetail.payment_method.reference);
                    database.AddInParameter(dbCommand, "psStatus", DbType.String, entChargeDetail.status);
                    database.AddInParameter(dbCommand, "psOrigin", DbType.String, psOrigin);
                    database.AddInParameter(dbCommand, "pnAmount", DbType.Decimal, entChargeDetail.amount);
                    database.AddInParameter(dbCommand, "piFee", DbType.Int32, entChargeDetail.fee);
                    database.AddInParameter(dbCommand, "psCustomerId", DbType.String, entChargeDetail.customer_id);

                    response = imdCommonData.DExecute(database, dbCommand);
                }
            }
            catch (Exception ex)
            {
                response.Code = 67823458102708;
                response.Message = "Ocurrió un error inesperado al guardar el detalle del pago en la base de datos";

                logger.Error(IMDSerialize.Serialize(67823458102708, $"Error en {metodo}(Guid puId, EntChargeDetail entChargeDetail): {ex.Message}", puId, entChargeDetail, ex, response));
            }
            return response;
        }

        public IMDResponse<DataTable> DGetOrder(string psOrderId)
        {
            IMDResponse<DataTable> response = new IMDResponse<DataTable>();

            string metodo = nameof(this.DGetOrder);
            logger.Info(IMDSerialize.Serialize(67823458157098, $"Inicia {metodo}(string psOrderId)", psOrderId));

            try
            {
                using (DbCommand dbCommand = database.GetStoredProcCommand(spGetConektaOrder))
                {
                    database.AddInParameter(dbCommand, "psOrderId", DbType.String, psOrderId);

                    response = imdCommonData.DExecuteDT(database, dbCommand);
                }
            }
            catch (Exception ex)
            {
                response.Code = 67823458157875;
                response.Message = "Ocurrió un error inesperado al consultar el detalle de la orden";

                logger.Error(IMDSerialize.Serialize(67823458157875, $"Error en {metodo}: {ex.Message}(string psOrderId)", psOrderId, ex, response));
            }
            return response;
        }
    }
}