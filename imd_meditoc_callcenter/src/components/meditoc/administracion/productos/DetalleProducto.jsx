import React from "react";
import MeditocModal from "../../../utilidades/MeditocModal";
import { Grid, Button } from "@material-ui/core";
import InfoField from "../../../utilidades/InfoField";

const DetalleProducto = (props) => {
    const { entProducto, open, setOpen } = props;

    //Funcion para cerrar este modal
    const handleClose = () => {
        setOpen(false);
    };

    return (
        <MeditocModal title="Detalle de producto" size="normal" open={open} setOpen={setOpen}>
            <Grid container spacing={3}>
                <Grid item sm={6} xs={12}>
                    <InfoField label="ID de producto:" value={entProducto.iIdProducto} />
                </Grid>
                <Grid item sm={6} xs={12}>
                    <InfoField label="Tipo de producto:" value={entProducto.sTipoProducto} />
                </Grid>
                <Grid item sm={6} xs={12}>
                    <InfoField label="Nombre:" value={entProducto.sNombre} />
                </Grid>
                <Grid item sm={6} xs={12}>
                    <InfoField label="Nombre corto:" value={entProducto.sNombreCorto} />
                </Grid>
                <Grid item sm={6} xs={12}>
                    <InfoField label="Descripción:" value={entProducto.sDescripcion} />
                </Grid>
                <Grid item sm={6} xs={12}>
                    <InfoField
                        label="Costo:"
                        value={"$" + entProducto.fCosto.toLocaleString("en-US", { minimumFractionDigits: 2 })}
                    />
                </Grid>
                <Grid item sm={6} xs={12}>
                    <InfoField label="Meses de vigencia:" value={entProducto.iMesVigencia} />
                </Grid>
                <Grid item sm={6} xs={12}>
                    <InfoField label="Ícono:" value={entProducto.sIcon} />
                </Grid>
                <Grid item sm={6} xs={12}>
                    <InfoField label="Comercial:" value={entProducto.bComercial === true ? "Sí" : "No"} />
                </Grid>
                <Grid item sm={6} xs={12}>
                    <InfoField label="Foliador:" value={entProducto.sPrefijoFolio} />
                </Grid>
                {/* <Grid item xs={12}>
                    <Button variant="contained" color="secondary" fullWidth onClick={handleClose}>
                        ACPETAR
                    </Button>
                </Grid> */}
            </Grid>
        </MeditocModal>
    );
};

export default DetalleProducto;