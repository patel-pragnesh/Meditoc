import { IconButton, Tooltip } from "@material-ui/core";
import React, { Fragment, useEffect, useState } from "react";

import AddRoundedIcon from "@material-ui/icons/AddRounded";
import AdministrarFolios from "./AdministrarFolios";
import EditIcon from "@material-ui/icons/Edit";
import EmpresaController from "../../../../controllers/EmpresaController";
import FormEmpresa from "./FormEmpresa";
import MeditocBody from "../../../utilidades/MeditocBody";
import MeditocHeader1 from "../../../utilidades/MeditocHeader1";
import MeditocTable from "../../../utilidades/MeditocTable";
import ProductoController from "../../../../controllers/ProductoController";
import PropTypes from "prop-types";
import ReplayIcon from "@material-ui/icons/Replay";
import WorkRoundedIcon from "@material-ui/icons/WorkRounded";
import { cellProps } from "../../../../configurations/dataTableIconsConfig";
import { emptyFunc } from "../../../../configurations/preventConfig";

/*************************************************************
 * Descripcion: Vista principal de Empresas y clientes
 * Creado: Cristopher Noh
 * Fecha: 26/08/2020
 * Invocado desde: ContentMain
 *************************************************************/
const Empresa = (props) => {
    //=================================PROPS=================================
    const { usuarioSesion, permisos, entCatalogos, funcLoader, funcAlert } = props;

    //=================================VARIABLES=================================
    let columns = [
        { title: "ID", field: "iIdEmpresa", ...cellProps, hidden: true },
        { title: "Nombre", field: "sNombre", ...cellProps },
        { title: "Folio", field: "sFolioEmpresa", ...cellProps },
        { title: "Correo", field: "sCorreo", ...cellProps },
        { title: "Fecha", field: "sFechaCreacion", ...cellProps },
    ];

    const empresaEntidadVacia = {
        iIdEmpresa: 0,
        sNombre: "",
        sFolioEmpresa: "",
        sCorreo: "",
    };

    //=================================CONTROLLERS=================================
    const empresaController = new EmpresaController();
    const productoController = new ProductoController();

    //=================================STATES=================================
    const [listaEmpresas, setListaEmpresas] = useState([]);
    const [listaProductos, setListaProductos] = useState([]);

    const [empresaSeleccionada, setEmpresaSeleccionada] = useState(empresaEntidadVacia);
    const [empresaParaModalForm, setEmpresaParaModalForm] = useState(empresaEntidadVacia);

    const [modalFormEmpresaOpen, setModalFormEmpresaOpen] = useState(false);
    const [modalFoliosEmpresaOpen, setModalFoliosEmpresaOpen] = useState(false);

    //=================================HANDLERS=================================
    //Abrir modal para crear una empresa
    const handleClickNuevaEmpresa = () => {
        setEmpresaParaModalForm(empresaEntidadVacia);
        setModalFormEmpresaOpen(true);
    };

    //Abrir modal para editar una empresa
    const handleClickEditarEmpresa = () => {
        if (empresaSeleccionada.iIdEmpresa === 0) {
            funcAlert("Seleccione una empresa de la tabla para continuar");
            return;
        }
        setEmpresaParaModalForm(empresaSeleccionada);
        setModalFormEmpresaOpen(true);
    };

    //Abrir administrador de folios de empresa
    const handleClickFoliosEmpresa = () => {
        if (empresaSeleccionada.iIdEmpresa === 0) {
            funcAlert("Seleccione una empresa de la tabla para continuar");
            return;
        }
        setEmpresaParaModalForm(empresaSeleccionada);
        setModalFoliosEmpresaOpen(true);
    };

    //=================================FUNCTIONS=================================
    //Función para obtener la lista de empresas activas
    const funcGetEmpresas = async () => {
        funcLoader(true, "Consultando empresas...");

        const response = await empresaController.funcGetEmpresas();

        if (response.Code === 0) {
            setListaEmpresas(response.Result);
        } else {
            funcAlert(response.Message);
        }

        funcLoader();
    };

    //Función para consultar los productos disponibles
    const funcGetProductos = async () => {
        funcLoader(true, "Consultando productos...");

        const response = await productoController.funcGetProductos();

        if (response.Code === 0) {
            setListaProductos(response.Result);
        } else {
            funcAlert(response.Message);
        }
        funcLoader();
    };

    //Función para consultar los datos
    const funcGetData = async () => {
        await funcGetEmpresas();
        await funcGetProductos();
    };

    //=================================EFFECT HOOKS=================================
    //Consultar datos al cargar la vista
    useEffect(() => {
        funcGetData();
        // eslint-disable-next-line
    }, []);

    return (
        <Fragment>
            <MeditocHeader1 title={permisos.Nombre}>
                {permisos.Botones["1"] !== undefined && ( //Nueva empresa
                    <Tooltip title={permisos.Botones["1"].Nombre} arrow>
                        <IconButton onClick={handleClickNuevaEmpresa}>
                            <AddRoundedIcon className="color-0" />
                        </IconButton>
                    </Tooltip>
                )}
                {permisos.Botones["2"] !== undefined && ( //Editar datos de empresa
                    <Tooltip title={permisos.Botones["2"].Nombre} arrow>
                        <IconButton onClick={handleClickEditarEmpresa}>
                            <EditIcon className="color-0" />
                        </IconButton>
                    </Tooltip>
                )}
                {permisos.Botones["3"] !== undefined && ( //Administrar folios de empresa
                    <Tooltip title={permisos.Botones["3"].Nombre} arrow>
                        <IconButton onClick={handleClickFoliosEmpresa}>
                            <WorkRoundedIcon className="color-0" />
                        </IconButton>
                    </Tooltip>
                )}
                {permisos.Botones["4"] !== undefined && ( //Actualizar tabla
                    <Tooltip title={permisos.Botones["4"].Nombre} arrow>
                        <IconButton onClick={funcGetData}>
                            <ReplayIcon className="color-0" />
                        </IconButton>
                    </Tooltip>
                )}
            </MeditocHeader1>
            <MeditocBody>
                <MeditocTable
                    columns={columns}
                    data={listaEmpresas}
                    rowSelected={empresaSeleccionada}
                    setRowSelected={setEmpresaSeleccionada}
                    mainField="iIdEmpresa"
                    doubleClick={permisos.Botones["3"] !== undefined ? handleClickFoliosEmpresa : emptyFunc}
                />
            </MeditocBody>
            <FormEmpresa
                entEmpresa={empresaParaModalForm}
                open={modalFormEmpresaOpen}
                setOpen={setModalFormEmpresaOpen}
                funcGetEmpresas={funcGetEmpresas}
                usuarioSesion={usuarioSesion}
                funcLoader={funcLoader}
                funcAlert={funcAlert}
            />
            <AdministrarFolios
                entEmpresa={empresaParaModalForm}
                open={modalFoliosEmpresaOpen}
                setOpen={setModalFoliosEmpresaOpen}
                listaProductos={listaProductos}
                usuarioSesion={usuarioSesion}
                entCatalogos={entCatalogos}
                permisos={permisos}
                funcLoader={funcLoader}
                funcAlert={funcAlert}
            />
        </Fragment>
    );
};

Empresa.propTypes = {
    entCatalogos: PropTypes.object,
    funcAlert: PropTypes.func,
    funcLoader: PropTypes.func,
    permisos: PropTypes.shape({
        Botones: PropTypes.object,
        Nombre: PropTypes.string,
    }),
    usuarioSesion: PropTypes.object,
};

export default Empresa;
