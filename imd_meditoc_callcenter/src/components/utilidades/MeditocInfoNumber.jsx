import React, { Fragment } from "react";

import PropTypes from "prop-types";

const MeditocInfoNumber = (props) => {
    const { label, value, color } = props;

    return (
        <Fragment>
            <span className={"rob-nor lighter size-50 " + color}>{value}</span>
            <br />
            <span className="rob-nor size-13 color-3">{label}</span>
        </Fragment>
    );
};

MeditocInfoNumber.propTypes = {
    color: PropTypes.any,
    label: PropTypes.any,
    value: PropTypes.any,
};

export default MeditocInfoNumber;
