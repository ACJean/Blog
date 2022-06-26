import React from "react";

const Alert = (props) => {

    let type = props.type;
    let typeClass = "";
    let typeTitle = "Anuncio";

    if (type === "error") {
        typeClass = "alert--error";
        typeTitle = "Error";
    }
    else if (type === "success") { 
        typeClass = "alert--success";
        typeTitle = "Éxito";
    }
    else if (type === "warning") {
        typeClass = "alert--warning";
        typeTitle = "Aviso";
    }
    else if (type === "info") { 
        typeClass = "alert--info";
        typeTitle = "Informativo";
    } else if (type === "purple") {
        typeClass = "alert--purple";
        typeTitle = "Logro";
    }

    return (
        <div className={`alert ${typeClass} ${props.show ? 'alert--show' : ''}`}>
            <div className="alert__title">{typeTitle}</div>
            <div className="alert__description">
                { props.messages instanceof Array ? (
                    <ul className="alert__list">
                        { props.messages.length > 0 ? props.messages.map((message, index) => <li key={index} className="alert__item">{message}</li>) : '' }
                    </ul>
                ) : props.messages }
            </div>
        </div>
    );
}

export default Alert