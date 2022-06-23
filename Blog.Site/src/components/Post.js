import React from "react";
import ReactMarkdown from 'react-markdown'
import remarkGfm from 'remark-gfm'

const Post = (props) => {
    return (
        <div className="card">
            <div className="card__content">
                <div className="card__content-header">
                    <img className="card__content-header-img" src="https://modaellos.com//wp-content/uploads/2016/04/los-mejores-cortes-de-cabello-para-hombre-2014-pelo-ondulado-o-rizado-largo-natural.jpg"/>
                    <div className="details">
                        <h5 className="details__title details__title--neon m-0 p-0">{props.post.creationUser.name}</h5>
                        <h6 className="details__circle details__circle--green m-0 p-0">Conectado</h6>
                    </div>
                </div>
                <div className="card__content-info">
                    <h6 className="m-0 p-0">{props.post.creationDate}</h6>
                    <h6 className="m-0 p-0">205 Comentarios</h6>
                </div>
                <h3>{props.post.title}</h3>
                <br/>
                <section>
                    <ReactMarkdown children={props.post.description} remarkPlugins={[remarkGfm]} />
                </section>
            </div>
        </div>
    );
}

export default Post;