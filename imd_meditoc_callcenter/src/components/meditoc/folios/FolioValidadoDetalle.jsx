import { Grid, Typography } from "@material-ui/core";
import React from "react";
import { useState } from "react";
import MeditocModal from "../../utilidades/MeditocModal";
import MeditocModalBotones from "../../utilidades/MeditocModalBotones";
import MeditocTable from "../../utilidades/MeditocTable";

const FolioValidadoDetalle = (props) => {
    const { entVentaCalle, open, setOpen } = props;

    const columns = [
        { title: "Folio", field: "sFolio", align: "center" },
        { title: "Contraseña", field: "sPassword", align: "center" },
    ];

    const [folioSeleccionado, setFolioSeleccionado] = useState({ sFolio: "" });

    return (
        <MeditocModal title={entVentaCalle.producto} size="small" open={open} setOpen={setOpen} level={2}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Typography variant="h5" color="primary">
                        Folios encontrados: {entVentaCalle.totalFolios}
                    </Typography>
                </Grid>
                <Grid item xs={12}>
                    <MeditocTable
                        columns={columns}
                        data={entVentaCalle.lstFolios}
                        rowSelected={folioSeleccionado}
                        setRowSelected={setFolioSeleccionado}
                        mainField="sFolio"
                    />
                </Grid>
            </Grid>
            <MeditocModalBotones okMessage="Aceptar" hideCancel setOpen={setOpen} />
        </MeditocModal>
    );
};

export default FolioValidadoDetalle;