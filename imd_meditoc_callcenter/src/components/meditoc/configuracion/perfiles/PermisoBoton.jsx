import { IconButton, TableCell, TableRow, Tooltip } from "@material-ui/core";
import React, { Fragment, useState } from "react";

import BlockIcon from "@material-ui/icons/Block";
import CGUController from "../../../../controllers/CGUController";
import ExtensionIcon from "@material-ui/icons/Extension";
import MeditocConfirmacion from "../../../utilidades/MeditocConfirmacion";
import PropTypes from "prop-types";
import { makeStyles } from "@material-ui/core/styles";

const useStyles = makeStyles({
    cell: {
        padding: "0px 24px 0px 16px",
    },
});

/*************************************************************
 * Descripcion: Representa una fila de un botón con la opción de quitar el permiso
 * Creado: Cristopher Noh
 * Fecha: 28/08/2020
 * Invocado desde: PermisoSubmodulo
 *************************************************************/
const PermisoBoton = (props) => {
    const { entPerfil, entBoton, usuarioSesion, funcGetPermisosXPerfil, permisos, funcLoader, funcAlert } = props;

    //Variables para estilos
    const classes = useStyles();
    const colorClass = "color-3";

    //State para mostrar/ocultar la alerta de confirmación para quitar permisos a este botón
    const [modalEliminarBotonOpen, setModalEliminarBotonOpen] = useState(false);

    //Función para abrir la alerta de confirmación para quitar permisos a este botón
    const handleClickEliminarBoton = () => {
        setModalEliminarBotonOpen(true);
    };

    //Función para quitar los permisos a este botón
    const funcEliminarPermisoBoton = async () => {
        const listaPermisosParaGuardar = [
            {
                iIdPerfil: entPerfil.iIdPerfil,
                iIdModulo: entBoton.iIdModulo,
                iIdSubModulo: entBoton.iIdSubModulo,
                iIdBoton: entBoton.iIdBoton,
                iIdUsuarioMod: usuarioSesion.iIdUsuario,
                bActivo: false,
                bBaja: true,
            },
        ];

        funcLoader(true, "Removiendo permisos de botón...");

        const cguController = new CGUController();
        const response = await cguController.funcSavePermiso(listaPermisosParaGuardar);

        if (response.Code === 0) {
            setModalEliminarBotonOpen(false);
            await funcGetPermisosXPerfil();

            funcAlert(response.Message, "success");
        } else {
            funcAlert(response.Message);
        }

        funcLoader();
    };

    return (
        <Fragment>
            <TableRow>
                <TableCell classes={{ sizeSmall: classes.cell }} align="left">
                    <ExtensionIcon className={colorClass + " vertical-align-middle"} />
                    <span className={colorClass + " size-15"}>
                        {entBoton.sNombre} ({entBoton.iIdBoton})
                    </span>
                </TableCell>
                <TableCell classes={{ sizeSmall: classes.cell }} align="right">
                    {permisos.Botones["11"] !== undefined && ( //Quitar permiso para el botón
                        <Tooltip title={`${permisos.Botones["11"].Nombre} ${entBoton.sNombre}`} placement="top" arrow>
                            <IconButton onClick={handleClickEliminarBoton}>
                                <BlockIcon />
                            </IconButton>
                        </Tooltip>
                    )}
                </TableCell>
            </TableRow>
            <MeditocConfirmacion
                title="Quitar permiso a botón"
                open={modalEliminarBotonOpen}
                setOpen={setModalEliminarBotonOpen}
                okFunc={funcEliminarPermisoBoton}
            >
                ¿Desea remover el permiso para el boton {entBoton.sNombre}?
            </MeditocConfirmacion>
        </Fragment>
    );
};

PermisoBoton.propTypes = {
    entBoton: PropTypes.shape({
        iIdBoton: PropTypes.number,
        iIdModulo: PropTypes.number,
        iIdSubModulo: PropTypes.number,
        sNombre: PropTypes.string,
    }),
    entPerfil: PropTypes.shape({
        iIdPerfil: PropTypes.number,
    }),
    funcAlert: PropTypes.func,
    funcGetPermisosXPerfil: PropTypes.func,
    funcLoader: PropTypes.func,
    usuarioSesion: PropTypes.shape({
        iIdUsuario: PropTypes.number,
    }),
};

export default PermisoBoton;
