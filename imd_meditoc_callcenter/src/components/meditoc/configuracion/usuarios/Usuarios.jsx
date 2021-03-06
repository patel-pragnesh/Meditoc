import { IconButton, Tooltip } from "@material-ui/core";
import React, { Fragment } from "react";

import CGUController from "../../../../controllers/CGUController";
import DeleteIcon from "@material-ui/icons/Delete";
import DetalleUsuario from "./DetalleUsuario";
import EditIcon from "@material-ui/icons/Edit";
import { EnumPerfilesPrincipales } from "../../../../configurations/enumConfig";
import FormUsuario from "./FormUsuario";
import MeditocBody from "../../../utilidades/MeditocBody";
import MeditocConfirmacion from "../../../utilidades/MeditocConfirmacion";
import MeditocHeader1 from "../../../utilidades/MeditocHeader1";
import MeditocTable from "../../../utilidades/MeditocTable";
import PersonAddIcon from "@material-ui/icons/PersonAdd";
import PropTypes from "prop-types";
import ReplayIcon from "@material-ui/icons/Replay";
import VisibilityIcon from "@material-ui/icons/Visibility";
import { cellProps } from "../../../../configurations/dataTableIconsConfig";
import { emptyFunc } from "../../../../configurations/preventConfig";
import { useEffect } from "react";
import { useState } from "react";

/*************************************************************
 * Descripcion: Submódulo para vista principal "USUARIOS" del portal Meditoc
 * Creado: Cristopher Noh
 * Fecha: 26/08/2020
 * Invocado desde: ContentMain
 *************************************************************/
const Usuarios = (props) => {
    const { usuarioSesion, permisos, funcLoader, funcAlert } = props;

    //Servicios API
    const cguController = new CGUController();

    //Entidad vacía de un usuario
    const usuarioEntidadVacia = {
        iIdUsuario: 0,
        iIdTipoCuenta: 1,
        iIdPerfil: 0,
        sTipoCuenta: "",
        sPerfil: "",
        sUsuario: "",
        sPassword: "",
        sNombres: "",
        sApellidoPaterno: "",
        sApellidoMaterno: "",
        dtFechaNacimiento: null,
        sTelefono: "",
        sCorreo: "",
        sDomicilio: "",
        iIdUsuarioMod: 0,
        bAcceso: true,
        bActivo: false,
        bBaja: false,
    };

    //Columnas de la tabla de usuarios
    const columns = [
        { title: "ID", field: "iIdUsuario", hidden: true, ...cellProps },
        { title: "Usuario", field: "sUsuario", ...cellProps },
        { title: "Tipo de cuenta", field: "sTipoCuenta", ...cellProps },
        { title: "Perfil", field: "sPerfil", ...cellProps },
        //{ title: "Nombre", field: "sNombres", ...cellProps },
        //{ title: "Apellido", field: "sApellidoPaterno", ...cellProps },
        { title: "Registrado", field: "sFechaCreacion", ...cellProps },
    ];

    //Lista de usuarios activos del portal
    const [listaUsuarios, setListaUsuarios] = useState([]);

    //Lista de perfiles activos del portal
    const [listaPerfiles, setListaPerfiles] = useState([]);

    //Usuario seleccionado de la tabla
    const [usuarioSeleccionado, setUsuarioSeleccionado] = useState(usuarioEntidadVacia);

    //Usuario para enviar el formulario de crear/editar
    const [usuarioParaModalForm, setUsuarioParaModalForm] = useState(usuarioEntidadVacia);

    //State para mostrar/ocultar el formulario para crear/editar usuario
    const [modalFormUsuarioNuevoOpen, setModalFormUsuarioNuevoOpen] = useState(false);

    //State para mostrar/ocultar la alerta de confirmación para dar de baja un usuario
    const [modalFormUsuarioEliminarOpen, setModalFormUsuarioEliminarOpen] = useState(false);

    const [modalDetalleUsuarioOpen, setModalDetalleUsuarioOpen] = useState(false);

    //Función para abrir el formulario para crear un usuario
    const handleClickNuevoUsuario = () => {
        setUsuarioParaModalForm(usuarioEntidadVacia);
        setModalFormUsuarioNuevoOpen(true);
    };

    const handleClickDetalleUsuario = () => {
        if (usuarioSeleccionado.iIdUsuario === 0) {
            funcAlert("Seleccione un usuario para continuar");
            return;
        }
        setModalDetalleUsuarioOpen(true);
    };

    //Función para abrir el formulario para editar un usuario
    const handleClickEditarUsuario = () => {
        if (usuarioSeleccionado.iIdUsuario === 0) {
            funcAlert("Seleccione un usuario para continuar");
            return;
        }
        if (
            usuarioSeleccionado.iIdPerfil === EnumPerfilesPrincipales.DoctorCallCenter ||
            usuarioSeleccionado.iIdPerfil === EnumPerfilesPrincipales.DoctorEspecialista ||
            usuarioSeleccionado.iIdPerfil === EnumPerfilesPrincipales.AdministradorEspecialiesta
        ) {
            funcAlert(
                "Los usuarios con cuenta de colaborador o doctor solo se pueden editar en la sección de Colaboradores del menú Administración",
                "warning"
            );
            return;
        }
        setUsuarioParaModalForm(usuarioSeleccionado);
        setModalFormUsuarioNuevoOpen(true);
    };

    //Función para abrir la alerta de confirmación para dar de baja un usuario
    const handleClickEliminarUsuario = () => {
        if (usuarioSeleccionado.iIdUsuario === 0) {
            funcAlert("Seleccione un usuario para continuar");
            return;
        }

        if (usuarioSeleccionado.iIdUsuario === 1) {
            funcAlert("El usuario sysadmin no se puede eliminar", "warning");
            return;
        }

        if (
            usuarioSeleccionado.iIdPerfil === EnumPerfilesPrincipales.DoctorCallCenter ||
            usuarioSeleccionado.iIdPerfil === EnumPerfilesPrincipales.DoctorEspecialista ||
            usuarioSeleccionado.iIdPerfil === EnumPerfilesPrincipales.AdministradorEspecialiesta
        ) {
            funcAlert(
                "Los usuarios con cuenta de colaborador o doctor solo se pueden dar de baja en la sección de Colaboradores del menú Administración",
                "warning"
            );
            return;
        }
        setModalFormUsuarioEliminarOpen(true);
    };

    //Consumir servicio para obtener los usuarios activos del portal
    const funcGetUsuarios = async () => {
        funcLoader(true, "Consultado usuarios del Meditoc...");
        const response = await cguController.funcGetUsuarios();

        if (response.Code === 0) {
            setListaUsuarios(response.Result);
        } else {
            funcAlert(response.Message);
        }

        funcLoader();
    };

    //Consumir servicio para obtener los perfiles activos del portal
    const funcGetPerfiles = async () => {
        funcLoader(true, "Consultado perfiles del sistema...");
        const response = await cguController.funcGetPerfiles();

        if (response.Code === 0) {
            setListaPerfiles(response.Result);
        } else {
            funcAlert(response.Message);
        }
        funcLoader();
    };

    //Consumir servicio para dar de baja un usuario del portal
    const funcEliminarUsuario = async () => {
        const entUsuarioSave = {
            iIdUsuario: usuarioSeleccionado.iIdUsuario,
            iIdTipoCuenta: usuarioSeleccionado.iIdTipoCuenta,
            // iIdPerfil: null,
            // sTipoCuenta: null,
            // sPerfil: null,
            // sUsuario: null,
            // sPassword: null,
            // sNombres: null,
            // sApellidoPaterno: null,
            // sApellidoMaterno: null,
            // dtFechaNacimiento: null,
            // sTelefono: null,
            // sCorreo: null,
            // sDomicilio: null,
            iIdUsuarioMod: usuarioSesion.iIdUsuario,
            bActivo: false,
            bBaja: true,
        };

        funcLoader(true, "Eliminando usuario...");

        const response = await cguController.funcSaveUsuario(entUsuarioSave);
        if (response.Code === 0) {
            setModalFormUsuarioEliminarOpen(false);

            await funcGetUsuarios();

            funcAlert(response.Message, "success");
        } else {
            funcAlert(response.Message);
        }

        funcLoader();
    };

    const getData = async () => {
        await funcGetUsuarios();
        await funcGetPerfiles();
    };

    //Consultar datos al cargar este componente
    useEffect(() => {
        getData();

        // eslint-disable-next-line
    }, []);

    return (
        <Fragment>
            <MeditocHeader1 title={permisos.Nombre}>
                {permisos.Botones["1"] !== undefined && ( //Nuevo usuario
                    <Tooltip title={permisos.Botones["1"].Nombre} arrow>
                        <IconButton onClick={handleClickNuevoUsuario}>
                            <PersonAddIcon className="color-0" />
                        </IconButton>
                    </Tooltip>
                )}
                {permisos.Botones["2"] !== undefined && ( //Detalle usuario
                    <Tooltip title={permisos.Botones["2"].Nombre} arrow>
                        <IconButton onClick={handleClickDetalleUsuario}>
                            <VisibilityIcon className="color-0" />
                        </IconButton>
                    </Tooltip>
                )}
                {permisos.Botones["3"] !== undefined && ( //Editar usuario
                    <Tooltip title={permisos.Botones["3"].Nombre} arrow>
                        <IconButton onClick={handleClickEditarUsuario}>
                            <EditIcon className="color-0" />
                        </IconButton>
                    </Tooltip>
                )}
                {permisos.Botones["4"] !== undefined && ( //Eliminar usuario
                    <Tooltip title={permisos.Botones["4"].Nombre} arrow>
                        <IconButton onClick={handleClickEliminarUsuario}>
                            <DeleteIcon className="color-0" />
                        </IconButton>
                    </Tooltip>
                )}
                {permisos.Botones["5"] !== undefined && ( //Actualizar tabla
                    <Tooltip title={permisos.Botones["5"].Nombre} arrow>
                        <IconButton onClick={getData}>
                            <ReplayIcon className="color-0" />
                        </IconButton>
                    </Tooltip>
                )}
            </MeditocHeader1>
            <MeditocBody>
                <MeditocTable
                    columns={columns}
                    data={listaUsuarios}
                    rowSelected={usuarioSeleccionado}
                    setRowSelected={setUsuarioSeleccionado}
                    mainField="iIdUsuario"
                    doubleClick={permisos.Botones["2"] !== undefined ? handleClickDetalleUsuario : emptyFunc}
                />
            </MeditocBody>
            <FormUsuario
                entUsuario={usuarioParaModalForm}
                listaPerfiles={listaPerfiles}
                open={modalFormUsuarioNuevoOpen}
                setUsuarioSeleccionado={setUsuarioSeleccionado}
                setOpen={setModalFormUsuarioNuevoOpen}
                funcGetUsuarios={funcGetUsuarios}
                usuarioSesion={usuarioSesion}
                funcLoader={funcLoader}
                funcAlert={funcAlert}
            />
            <DetalleUsuario
                entUsuario={usuarioSeleccionado}
                open={modalDetalleUsuarioOpen}
                setOpen={setModalDetalleUsuarioOpen}
            />
            <MeditocConfirmacion
                title="Eliminar usuario"
                open={modalFormUsuarioEliminarOpen}
                setOpen={setModalFormUsuarioEliminarOpen}
                okFunc={funcEliminarUsuario}
            >
                ¿Desea eliminar el usuario {usuarioSeleccionado.sUsuario}?
            </MeditocConfirmacion>
        </Fragment>
    );
};

Usuarios.propTypes = {
    funcAlert: PropTypes.func,
    funcLoader: PropTypes.func,
    title: PropTypes.string,
    usuarioSesion: PropTypes.shape({
        iIdUsuario: PropTypes.number,
    }),
};

export default Usuarios;
