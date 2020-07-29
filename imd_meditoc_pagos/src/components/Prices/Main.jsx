import React, { Fragment } from "react";
import Menu from "./Menu";
import Cover from "./Cover";
import Content from "./Content";
import Footer from "../Footer";

/*****************************************************
 * Descripción: Estructura principal de la pagina de precios
 * Autor: Cristopher Noh
 * Fecha: 22/07/2020
 * Modificaciones:
 *****************************************************/
const Main = () => {
    window.scrollTo(0, 0);
    return (
        <Fragment>
            <Menu />
            <Cover />
            <Content />
            <Footer />
        </Fragment>
    );
};

export default Main;