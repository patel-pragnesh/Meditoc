import { Grid, IconButton, InputAdornment, MenuItem, TextField, Tooltip, makeStyles } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import { apiGetDirectorio, apiGetEspecialidades } from "../../configuration/apiConfig";

import { AiOutlineFileSearch } from "react-icons/ai";
import { BsSearch } from "react-icons/bs";
import GetApp from "../GetApp";
import { MdClose } from "react-icons/md";
import MedicInfo from "./MedicInfo";
import Pagination from "@material-ui/lab/Pagination";
import PropTypes from "prop-types";
import { serverMain } from "../../configuration/serverConfig";

const useStyles = makeStyles({
    ul: {
        display: "inline-flex",
        flexWrap: "initial",
    },
});

/*****************************************************
 * Descripción: Contenido del directorio médico, filtro y detalle
 * Autor: Cristopher Noh
 * Fecha: 07/09/2020
 * Modificaciones:
 *****************************************************/
const Content = (props) => {
    const { funcLoader } = props;

    const classes = useStyles();
    const pageSize = 20;

    const [paginaSeleccionada, setPaginaSeleccionada] = useState(1);
    const [paginasTotales, setPaginasTotales] = useState(0);
    const [especialidadSeleccionada, setEspecialidadSeleccionada] = useState("");
    const [buscadorIngresada, setBuscadorIngresada] = useState("");
    const [ultimaBusqueda, setUltimaBusqueda] = useState("");

    const [listaColaboradores, setListaColaboradores] = useState([]);
    const [listaEspecialidades, setListaEspecialidades] = useState([]);

    const funcGetDirectorio = async (especialidad = null, buscador = "", page = null) => {
        funcLoader(true, "Consultando en directorio médico...");

        try {
            const apiResponse = await fetch(
                `${serverMain}${apiGetDirectorio}?piIdEspecialidad=${especialidad}&psBuscador=${buscador}&piPage=${page}&piPageSize=${pageSize}`
            );

            const response = await apiResponse.json();

            if (response.Code === 0) {
                setPaginasTotales(response.Result.iTotalPaginas);
                setListaColaboradores(response.Result.lstColaboradores);
            }
        } catch (error) {}

        funcLoader();
    };

    const funcGetEspecialidades = async () => {
        funcLoader(true, "Consultando especialidades...");

        try {
            const apiResponse = await fetch(`${serverMain}${apiGetEspecialidades}`);

            const response = await apiResponse.json();

            if (response.Code === 0) {
                setListaEspecialidades(response.Result);
            }
        } catch (error) {}

        funcLoader();
    };

    const handleChangeEspecialidad = async (e) => {
        await funcGetDirectorio(e.target.value, buscadorIngresada, 1);
        setEspecialidadSeleccionada(e.target.value);
        setPaginaSeleccionada(1);
    };

    const handleChangePage = async (e, page) => {
        await funcGetDirectorio(especialidadSeleccionada, buscadorIngresada, page);
        setPaginaSeleccionada(page);
    };

    const handleChangeBuscador = (e) => {
        setBuscadorIngresada(e.target.value);
    };

    const handleClickBuscar = async (e) => {
        e.preventDefault();
        await funcGetDirectorio(null, buscadorIngresada, 1);
        setUltimaBusqueda(buscadorIngresada);
        setPaginaSeleccionada(1);
    };

    const handleClickLimpiar = async () => {
        await funcGetDirectorio(especialidadSeleccionada, "", 1);
        setBuscadorIngresada("");
        setUltimaBusqueda("");
        setPaginaSeleccionada(1);
    };

    const funcGetData = async () => {
        await funcGetDirectorio(null, "", 1);
        await funcGetEspecialidades();
    };

    useEffect(() => {
        funcGetData();
        // eslint-disable-next-line
    }, []);

    return (
        <div className="directory-content">
            <form id="form-directorio" onSubmit={handleClickBuscar} noValidate>
                <Grid container spacing={3}>
                    <Grid item sm={6} xs={12}>
                        <TextField
                            variant="outlined"
                            fullWidth
                            label="Buscar..."
                            InputProps={{
                                endAdornment: (
                                    <InputAdornment edge="start">
                                        {ultimaBusqueda !== "" ? (
                                            <Tooltip title="Limpiar búsqueda" arrow placement="top">
                                                <IconButton onClick={handleClickLimpiar}>
                                                    <MdClose />
                                                </IconButton>
                                            </Tooltip>
                                        ) : null}
                                        <Tooltip title="Iniciar búsqueda" arrow placement="top">
                                            <IconButton onClick={handleClickBuscar}>
                                                <BsSearch />
                                            </IconButton>
                                        </Tooltip>
                                    </InputAdornment>
                                ),
                            }}
                            value={buscadorIngresada}
                            onChange={handleChangeBuscador}
                        />
                    </Grid>

                    <Grid item sm={6} xs={12}>
                        <TextField
                            fullWidth
                            variant="outlined"
                            label="Especialidad:"
                            select
                            value={especialidadSeleccionada}
                            onChange={handleChangeEspecialidad}
                            SelectProps={{ MenuProps: { PaperProps: { style: { maxHeight: 300 } } } }}
                        >
                            <MenuItem value="">
                                <em>Todas las especialidades</em>
                            </MenuItem>
                            {listaEspecialidades
                                .sort((a, b) => (a.sNombre > b.sNombre ? 1 : -1))
                                .map((especialidad) =>
                                    especialidad.iIdEspecialidad === 1 ? null : (
                                        <MenuItem
                                            key={especialidad.iIdEspecialidad}
                                            value={especialidad.iIdEspecialidad.toString()}
                                        >
                                            {especialidad.sNombre}
                                        </MenuItem>
                                    )
                                )}
                        </TextField>
                    </Grid>
                </Grid>
                <button type="submit" hidden />
            </form>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    {listaColaboradores.map((entColaborador) => (
                        <MedicInfo key={entColaborador.iIdColaborador} entColaborador={entColaborador} />
                    ))}
                    {paginasTotales > 0 ? (
                        <div className="directory-pagination-container">
                            <Pagination
                                classes={{ ul: classes.ul }}
                                count={paginasTotales}
                                size="large"
                                showFirstButton
                                showLastButton
                                color="primary"
                                page={paginaSeleccionada}
                                onChange={handleChangePage}
                            />
                        </div>
                    ) : (
                        <div className="center">
                            <AiOutlineFileSearch style={{ fontSize: 150, color: "#ccc" }} />
                            <br />
                            <span className="price-content-description-normal">No se encontraron registros</span>
                        </div>
                    )}
                </Grid>
                <Grid item xs={12}>
                    <GetApp />
                </Grid>
            </Grid>
        </div>
    );
};

Content.propTypes = {
    funcLoader: PropTypes.func,
};

export default Content;
