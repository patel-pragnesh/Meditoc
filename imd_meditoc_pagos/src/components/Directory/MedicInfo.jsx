import { Grid, Hidden } from "@material-ui/core";
import React from "react";
import { MdAccountBox } from "react-icons/md";

const MedicInfo = (props) => {
    const { entColaborador } = props;

    return (
        <Grid item xs={12}>
            <Grid container spacing={2} style={{ marginBottom: 50 }}>
                <Hidden only={["xs"]}>
                    <Grid item sm={4} xs={12}>
                        <div className="directory-doctor-img-container">
                            {entColaborador.sFoto !== null && entColaborador.sFoto !== "" ? (
                                <img
                                    src={`data:image/png;base64,${entColaborador.sFoto}`}
                                    alt="MEDITOCDOCTOR"
                                    className="directory-doctor-img"
                                />
                            ) : (
                                <MdAccountBox style={{ fontSize: 220, color: "#ccc" }} />
                            )}
                        </div>
                    </Grid>
                </Hidden>
                <Grid item sm={8} xs={12}>
                    <Grid container spacing={2}>
                        <Grid item xs={12}>
                            <div className="directory-doctor-name-container">
                                <span className="directory-doctor-name">{entColaborador.sNombre}</span>
                            </div>
                        </Grid>
                        <Hidden only={["xl", "lg", "md", "sm"]}>
                            <Grid item xs={12}>
                                <img
                                    src={`data:image/png;base64,${entColaborador.sFoto}`}
                                    alt="MEDITOCDOCTOR"
                                    className="directory-doctor-img"
                                />
                            </Grid>
                        </Hidden>
                        <Grid item xs={6}>
                            <span className="directory-doctor-label">Especialidad</span>
                            <br />
                            <span className="directory-doctor-value">{entColaborador.sEspecialidad}</span>
                        </Grid>
                        <Grid item xs={6}>
                            <span className="directory-doctor-label">Céd. Prof</span>
                            <br />
                            <span className="directory-doctor-value">{entColaborador.sCedulaProfecional}</span>
                        </Grid>
                        <Grid item xs={6}>
                            <span className="directory-doctor-label">Teléfono</span>
                            <br />
                            <span className="directory-doctor-value">{entColaborador.sTelefono}</span>
                        </Grid>
                        <Grid item xs={6}>
                            <span className="directory-doctor-label">Whatsapp</span>
                            <br />
                            <span className="directory-doctor-value">{entColaborador.sTelefono}</span>
                        </Grid>
                        <Grid item xs={12}>
                            <span className="directory-doctor-label">Correo</span>
                            <br />
                            <span className="directory-doctor-value">{entColaborador.sCorreo}</span>
                        </Grid>
                        <Grid item xs={12}>
                            <span className="directory-doctor-label">Dirección</span>
                            <br />
                            <span className="directory-doctor-value">{entColaborador.sDireccionConsultorio}</span>
                        </Grid>
                        {/* <Grid item xs={12}>
                            <span className="directory-doctor-label">Consultorio</span>
                            <br />
                            <span className="directory-doctor-value">Hospital Faro del Mayab - Consultorio 241</span>
                        </Grid> */}
                        <Grid item xs={12}>
                            <span className="directory-doctor-label">URL</span>
                            <br />
                            <span className="directory-doctor-value">
                                <a href={entColaborador.sURL} className="directory-link" target="_blank">
                                    {entColaborador.sURL}
                                </a>
                            </span>
                        </Grid>
                    </Grid>
                </Grid>
            </Grid>
        </Grid>
    );
};

export default MedicInfo;
