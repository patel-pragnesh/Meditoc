﻿using IMD.Admin.Utilities.Business;
using IMD.Admin.Utilities.Entities;
using IMD.Meditoc.CallCenter.Mx.Business.Colaborador;
using IMD.Meditoc.CallCenter.Mx.Business.Consulta;
using IMD.Meditoc.CallCenter.Mx.Business.Folio;
using IMD.Meditoc.CallCenter.Mx.Business.Paciente;
using IMD.Meditoc.CallCenter.Mx.Data.CallCenter;
using IMD.Meditoc.CallCenter.Mx.Entities.CallCenter;
using IMD.Meditoc.CallCenter.Mx.Entities.Catalogos;
using IMD.Meditoc.CallCenter.Mx.Entities.Colaborador;
using IMD.Meditoc.CallCenter.Mx.Entities.Consultas;
using IMD.Meditoc.CallCenter.Mx.Entities.Folio;
using IMD.Meditoc.CallCenter.Mx.Entities.Paciente;
using IMD.Meditoc.CallCenter.Mx.Entities.Producto;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace IMD.Meditoc.CallCenter.Mx.Business.CallCenter
{
    public class BusCallCenter
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(BusCallCenter));
        DatCallCenter datCallCenter;
        BusColaborador busColaborador;
        BusFolio busFolio;
        BusPaciente busPaciente;
        BusConsulta busConsulta;

        public BusCallCenter()
        {
            datCallCenter = new DatCallCenter();
            busColaborador = new BusColaborador();
            busFolio = new BusFolio();
            busPaciente = new BusPaciente();
            busConsulta = new BusConsulta();
        }

        /// <summary>
        /// Cambia el estatus del colaborador OCUPADO - DISPONIBLE
        /// </summary>
        /// <param name="entOnlineMod"></param>
        /// <returns></returns>
        public IMDResponse<bool> BCallCenterOnline(EntOnlineMod entOnlineMod)
        {
            IMDResponse<bool> response = new IMDResponse<bool>();

            string metodo = nameof(this.BCallCenterOnline);
            logger.Info(IMDSerialize.Serialize(67823458509079, $"Inicia {metodo}(EntOnlineMod entOnlineMod)", entOnlineMod));

            try
            {
                if (entOnlineMod == null)
                {
                    response.Code = 767872123751097;
                    response.Message = "No se ingresó información del colaborador.";
                    return response;
                }

                //Actualizar status
                IMDResponse<bool> resSvaOnline = datCallCenter.DCallCenterOnline(entOnlineMod.iIdColaborador, entOnlineMod.bOnline, entOnlineMod.bOcupado, entOnlineMod.iIdUsuarioMod);
                if (resSvaOnline.Code != 0)
                {
                    return resSvaOnline;
                }

                response.Code = 0;
                response.Result = true;
                response.Message = entOnlineMod.bOnline ? "Se ha cambiado el estatus a EN LÍNEA" + (entOnlineMod.bOcupado ? " - OCUPADO." : " - DISPONIBLE.") : "Se ha cambiado el estatus a FUERA DE LÍNEA.";

            }
            catch (Exception ex)
            {
                response.Code = 67823458509856;
                response.Message = "Ocurrió un error inesperado al intentar actualizar el estatus.";

                logger.Error(IMDSerialize.Serialize(67823458509856, $"Error en {metodo}(EntOnlineMod entOnlineMod): {ex.Message}", entOnlineMod, ex, response));
            }
            return response;
        }

        /// <summary>
        /// Crea o busca la consulta para un paciente dependiendo del tipo de colaborador basandose en el folio proporcionado
        /// </summary>
        /// <param name="iIdColaborador"></param>
        /// <param name="sFolio"></param>
        /// <param name="iIdUsuarioMod"></param>
        /// <returns></returns>
        public IMDResponse<EntCallCenter> BAccesoConsulta(int iIdColaborador, string sFolio, int iIdUsuarioMod)
        {
            IMDResponse<EntCallCenter> response = new IMDResponse<EntCallCenter>();

            string metodo = nameof(this.BAccesoConsulta);
            logger.Info(IMDSerialize.Serialize(67823458513741, $"Inicia {metodo}(int iIdColaborador, string sFolio, int iIdUsuarioMod)", iIdColaborador, sFolio, iIdUsuarioMod));

            try
            {
                EntCallCenter entCallCenter = new EntCallCenter();

                //Obtener info de colaborador
                IMDResponse<List<EntColaborador>> resGetColaborador = busColaborador.BGetColaborador(piIdColaborador: iIdColaborador);
                if (resGetColaborador.Code != 0)
                {
                    return resGetColaborador.GetResponse<EntCallCenter>();
                }
                if (resGetColaborador.Result.Count != 1)
                {
                    response.Code = -88793457892374;
                    response.Message = "No se encontró la información del colaborador.";
                    return response;
                }

                entCallCenter.entColaborador = resGetColaborador.Result.First();

                //Obtener info del folio
                IMDResponse<List<EntFolioReporte>> resGetFolio = busFolio.BGetFolios(psFolio: sFolio, pbActivo: null, pbBaja: null);
                if (resGetFolio.Code != 0)
                {
                    return resGetFolio.GetResponse<EntCallCenter>();
                }
                if (resGetFolio.Result.Count != 1)
                {
                    response.Code = -23459710761984;
                    response.Message = "No se encontró el folio solicitado.";
                    return response;
                }

                entCallCenter.entFolio = resGetFolio.Result.First();

                if (entCallCenter.entFolio.dtFechaVencimiento != null)
                {
                    if (DateTime.Now > entCallCenter.entFolio.dtFechaVencimiento)
                    {
                        response.Code = -9776259868345;
                        response.Message = $"El folio expiró el {entCallCenter.entFolio.dtFechaVencimiento?.ToString("dd/MM/yyyy")} a las {entCallCenter.entFolio.dtFechaVencimiento?.ToString("hh:mm tt")}";
                        return response;
                    }
                }

                //Obtener información del paciente
                IMDResponse<List<EntPaciente>> resGetPaciente = busPaciente.BGetPacientes(piIdFolio: entCallCenter.entFolio.iIdFolio);
                if (resGetPaciente.Code != 0)
                {
                    return resGetPaciente.GetResponse<EntCallCenter>();
                }

                if (resGetPaciente.Result.Count != 1)
                {
                    response.Code = -2345771987764112;
                    response.Message = "No se encontró información del paciente.";
                    return response;
                }

                entCallCenter.entPaciente = resGetPaciente.Result.First();

                //Validar tipo de colaborador
                if (entCallCenter.entColaborador.iIdTipoDoctor == (int)EnumTipoDoctor.MedicoCallCenter || entCallCenter.entColaborador.iIdTipoDoctor == (int)EnumTipoDoctor.MedicoAdministrativo)
                {
                    if (entCallCenter.entFolio.iIdOrigen == (int)EnumOrigen.Particular)
                    {
                        response.Code = -19854873834598;
                        response.Message = "El folio proporcionado solo es válido para consultas programadas con Especialistas.";
                        return response;
                    }

                    if (!entCallCenter.entFolio.bActivo || entCallCenter.entFolio.bBaja)
                    {
                        response.Code = -8634576876234;
                        response.Message = "El folio ha expirado o ha sido dado de baja.";
                        return response;
                    }

                    //Para doctores callcenter, generar la consulta y retornar datos
                    EntConsulta entConsulta = new EntConsulta
                    {
                        iIdColaborador = entCallCenter.entColaborador.iIdColaborador,
                        iIdPaciente = entCallCenter.entPaciente.iIdPaciente,
                        iIdEstatusConsulta = (int)EnumEstatusConsulta.CreadoProgramado,
                        dtFechaProgramadaInicio = DateTime.Now
                    };

                    IMDResponse<EntConsulta> resSaveConsulta = busConsulta.BSaveConsulta(entConsulta, iIdUsuarioMod);
                    if (resSaveConsulta.Code != 0)
                    {
                        return resSaveConsulta.GetResponse<EntCallCenter>();
                    }

                    entCallCenter.entConsulta = resSaveConsulta.Result;

                    //Consultar historial clínico generado por otros colaboradores callcenter
                    List<int> doctoresCallCenter = new List<int> { (int)EnumTipoDoctor.MedicoCallCenter, (int)EnumTipoDoctor.MedicoAdministrativo };

                    IMDResponse<List<EntHistorialClinico>> resGetHistorial = busConsulta.BGetHistorialMedico(piIdFolio: entCallCenter.entFolio.iIdFolio, psIdTipoDoctor: string.Join(",", doctoresCallCenter));
                    if (resGetHistorial.Code != 0)
                    {
                        return resGetHistorial.GetResponse<EntCallCenter>();
                    }

                    entCallCenter.lstHistorialClinico = resGetHistorial.Result;
                }
                else if (entCallCenter.entColaborador.iIdTipoDoctor == (int)EnumTipoDoctor.MedicoEspecialista)
                {
                    //Para doctores especialistas, buscar consultas programadas en la agenda
                    IMDResponse<List<EntDetalleConsulta>> resVerificarConsulta = busConsulta.BGetConsultaMomento(entCallCenter.entPaciente.iIdPaciente, entCallCenter.entColaborador.iIdColaborador);
                    if (resVerificarConsulta.Code != 0)
                    {
                        return resVerificarConsulta.GetResponse<EntCallCenter>();
                    }

                    if (resVerificarConsulta.Result.Count < 1)
                    {
                        response.Code = -56878843909375;
                        response.Message = $"No se encontró una consulta programada para el paciente. La tolerancia para el horario programado de la consulta es de {ConfigurationManager.AppSettings["iMinToleraciaConsultaInicio"]} minutos antes de la hora.";
                        return response;
                    }

                    //Validar que la consulta programada esté en el horario programada y con status válido
                    List<EntDetalleConsulta> HayConsultaProgramadaOReprograma = resVerificarConsulta.Result.Where(x => x.iIdEstatusConsulta != (int)EnumEstatusConsulta.Finalizado && x.iIdEstatusConsulta != (int)EnumEstatusConsulta.Cancelado).ToList();
                    if (HayConsultaProgramadaOReprograma.Count > 0)
                    {
                        EntDetalleConsulta entDetalleConsulta = HayConsultaProgramadaOReprograma.First();

                        EntConsulta entConsulta = new EntConsulta
                        {
                            iIdConsulta = (int)entDetalleConsulta.iIdConsulta,
                            iIdColaborador = entCallCenter.entColaborador.iIdColaborador,
                            iIdPaciente = entCallCenter.entPaciente.iIdPaciente,
                            iIdEstatusConsulta = entDetalleConsulta.iIdEstatusConsulta,
                        };

                        entCallCenter.entConsulta = entConsulta;
                    }
                    else
                    {
                        EntDetalleConsulta entDetalleConsulta = resVerificarConsulta.Result.Last();

                        if (entDetalleConsulta.iIdEstatusConsulta == (int)EnumEstatusConsulta.Cancelado)
                        {
                            response.Code = -978912879598735;
                            response.Message = $"La consulta ya fue cancelada.";
                            return response;
                        }

                        if (entDetalleConsulta.iIdEstatusConsulta == (int)EnumEstatusConsulta.Finalizado)
                        {
                            response.Code = -4498871498234;
                            response.Message = $"La consulta ya ha finalizado.";
                            return response;
                        }
                    }

                    //Obtener historial clínico sólo de las consultas con el colaborador especialista actual
                    IMDResponse<List<EntHistorialClinico>> resGetHistorial = busConsulta.BGetHistorialMedico(piIdFolio: entCallCenter.entFolio.iIdFolio, piIdColaborador: entCallCenter.entColaborador.iIdColaborador);
                    if (resGetHistorial.Code != 0)
                    {
                        return resGetHistorial.GetResponse<EntCallCenter>();
                    }

                    entCallCenter.lstHistorialClinico = resGetHistorial.Result;
                }
                else
                {
                    response.Code = -3338296867623;
                    response.Message = $"No se puede determinar el tipo de cuenta del usuario.";
                    return response;
                }



                response.Code = 0;
                response.Result = entCallCenter;
                response.Message = "Se ha accedido a la consulta.";

            }
            catch (Exception ex)
            {
                response.Code = 67823458514518;
                response.Message = "Ocurrió un error inesperado al acceder a la consulta del paciente.";

                logger.Error(IMDSerialize.Serialize(67823458514518, $"Error en {metodo}(int iIdColaborador, string sFolio, int iIdUsuarioMod): {ex.Message}", iIdColaborador, sFolio, iIdUsuarioMod, ex, response));
            }
            return response;
        }

        /// <summary>
        /// Iniciar la consulta seteando la fecha de inicio de una consulta proporcionada
        /// </summary>
        /// <param name="iIdConsulta"></param>
        /// <param name="iIdColaborador"></param>
        /// <param name="iIdUsuarioMod"></param>
        /// <returns></returns>
        public IMDResponse<bool> BIniciarConsulta(int iIdConsulta, int iIdColaborador, int iIdUsuarioMod)
        {
            IMDResponse<bool> response = new IMDResponse<bool>();

            string metodo = nameof(this.BIniciarConsulta);
            logger.Info(IMDSerialize.Serialize(67823458526173, $"Inicia {metodo}(int iIdConsulta, int iIdColaborador, int iIdUsuarioMod)", iIdConsulta, iIdColaborador, iIdUsuarioMod));

            try
            {
                if (iIdConsulta == 0)
                {
                    response.Code = -8793487583457;
                    response.Message = "La información para iniciar la consulta está incompleta.";
                    return response;
                }

                EntConsulta entConsulta = new EntConsulta
                {
                    iIdConsulta = iIdConsulta,
                    dtFechaConsultaInicio = DateTime.Now,
                    iIdEstatusConsulta = (int)EnumEstatusConsulta.EnConsulta
                };

                //Actualizar fecha de inicio de consulta
                IMDResponse<EntConsulta> resSaveConsulta = busConsulta.BSaveConsulta(entConsulta, iIdUsuarioMod);
                if (resSaveConsulta.Code != 0)
                {
                    return resSaveConsulta.GetResponse<bool>();
                }

                response.Code = 0;
                response.Message = "La consulta ha iniciado.";
                response.Result = true;
            }
            catch (Exception ex)
            {
                response.Code = 67823458526950;
                response.Message = "Ocurrió un error inesperado al iniciar la consulta.";

                logger.Error(IMDSerialize.Serialize(67823458526950, $"Error en {metodo}(int iIdConsulta, int iIdColaborador, int iIdUsuarioMod): {ex.Message}", iIdConsulta, iIdColaborador, iIdUsuarioMod, ex, response));
            }
            return response;
        }

        /// <summary>
        /// Finaliza la consulta seteando la fecha de fin de consulta proporcionado
        /// </summary>
        /// <param name="iIdConsulta"></param>
        /// <param name="iIdColaborador"></param>
        /// <param name="iIdUsuarioMod"></param>
        /// <returns></returns>
        public IMDResponse<bool> BFinalizarConsulta(int iIdConsulta, int iIdColaborador, int iIdUsuarioMod)
        {
            IMDResponse<bool> response = new IMDResponse<bool>();

            string metodo = nameof(this.BFinalizarConsulta);
            logger.Info(IMDSerialize.Serialize(67823458527727, $"Inicia {metodo}(int iIdConsulta, int iIdColaborador, int iIdUsuarioMod)", iIdConsulta, iIdColaborador, iIdUsuarioMod));

            try
            {
                if (iIdConsulta == 0)
                {
                    response.Code = -676363680712478;
                    response.Message = "La información para finalizar la consulta está incompleta.";
                    return response;
                }

                //Obtener el detalle de la consulta actual
                IMDResponse<List<EntDetalleConsulta>> resGetConsulta = busConsulta.BGetDetalleConsulta(piIdConsulta: iIdConsulta);
                if (resGetConsulta.Code != 0)
                {
                    return resGetConsulta.GetResponse<bool>();
                }

                if (resGetConsulta.Result.Count < 1)
                {
                    response.Code = -87812314544512;
                    response.Message = "No se encontró la consulta.";
                    return response;
                }

                EntConsulta entConsulta = new EntConsulta
                {
                    iIdConsulta = iIdConsulta,
                    dtFechaConsultaFin = DateTime.Now,
                    iIdEstatusConsulta = (int)EnumEstatusConsulta.Finalizado
                };

                //Actualizar fecha de fin de consulta
                IMDResponse<EntConsulta> resSaveConsulta = busConsulta.BSaveConsulta(entConsulta, iIdUsuarioMod);
                if (resSaveConsulta.Code != 0)
                {
                    return resSaveConsulta.GetResponse<bool>();
                }

                EntDetalleConsulta consulta = resGetConsulta.Result.First();
                EntFolioFV entFolio = new EntFolioFV
                {
                    iIdEmpresa = (int)consulta.iIdEmpresa,
                    iIdUsuario = iIdUsuarioMod,
                    lstFolios = new List<EntFolioFVItem>
                        {
                            new EntFolioFVItem
                            {
                                iIdFolio = (int)consulta.iIdFolio
                            }
                        }
                };

                if (consulta.iIdTipoDoctor == (int)EnumTipoDoctor.MedicoCallCenter || consulta.iIdTipoDoctor == (int)EnumTipoDoctor.MedicoAdministrativo)
                {
                    if (consulta.iIdTipoProducto == (int)EnumTipoProducto.Servicio)
                    {
                        //Dar de baja a los folios de servicio que hayan sido atendido por colaborador callcenter
                        IMDResponse<bool> resDesactivarFolios = busFolio.BEliminarFoliosEmpresa(entFolio);
                        if (resDesactivarFolios.Code != 0)
                        {
                            return resDesactivarFolios;
                        }
                    }
                }
                else
                {
                    if (consulta.iIdOrigen == (int)EnumOrigen.Particular)
                    {
                        //Dar de baja a los folios cuando sean generados por un especialista y tengan el origen Particular
                        IMDResponse<bool> resDesactivarFolios = busFolio.BEliminarFoliosEmpresa(entFolio);
                        if (resDesactivarFolios.Code != 0)
                        {
                            return resDesactivarFolios;
                        }
                    }
                }

                response.Code = 0;
                response.Message = "La consulta ha finalizado.";
                response.Result = true;
            }
            catch (Exception ex)
            {
                response.Code = 67823458528504;
                response.Message = "Ocurrió un error inesperado al intentar finalizar la consulta.";

                logger.Error(IMDSerialize.Serialize(67823458528504, $"Error en {metodo}(int iIdConsulta, int iIdColaborador, int iIdUsuarioMod): {ex.Message}", iIdConsulta, iIdColaborador, iIdUsuarioMod, ex, response));
            }
            return response;
        }
    }
}
