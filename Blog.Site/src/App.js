import React, { useEffect, useState } from 'react';
import Post from './components/Post';
import { ALL_POST, CREATE_POST } from './consts/Api';
import PostDTO from './objs/PostDTO';
import httpUtil from './utils/HttpUtil';

function App() {

    const [posts, setPosts] = useState([]);
    const [post, setPost] = useState(new PostDTO());

    const token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI2MmIzOWU3MGZhMmIyMGVjNThlNDg2YmIiLCJuYmYiOjE2NTU5NTQ3OTksImV4cCI6MTY1NTk2NTU5OSwiaWF0IjoxNjU1OTU0Nzk5fQ.ek4agSsFTjP6JC82xk6FCpaY1JeLRFsIz7DeGkdQSbg";

    useEffect(async () => {
        try {
            let response = await httpUtil().get(ALL_POST, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });
            let serviceResponse = response.content;
            if (response.ok && serviceResponse && serviceResponse.isSuccess) setPosts(serviceResponse.data);
            else setPosts([]);
        } catch {
            setPosts();
        }
    }, []);
    
    const handleChange = e => {
        setPost({ ...post, [e.target.name]: e.target.value });
    };

    const handleSubmit = async e => {
        e.preventDefault();
        try {
            let response = await httpUtil().post(CREATE_POST, {
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${token}`,
                  },
                body: JSON.stringify(post)
            });
            let serviceResponse = response.content;
            if (response.status === 201) {
                setPosts([serviceResponse.data, ...posts]);
                setPost(new PostDTO())
            }
            else if (response.status === 400) console.log(serviceResponse.validationMessages);
        } catch {

        }
    }

    return (
        <>
            <br/>
            <div className='container'>
                <div className='card'>
                    <form className='post-control' onSubmit={handleSubmit}>
                        <input className='input post-control__title' placeholder='Titulo' type="text" id="title" name="title" value={post.title} onChange={handleChange}/>
                        <textarea 
                        className='input post-control__body' 
                        placeholder='Descripción' 
                        id='description' 
                        name='description' 
                        value={post.description} 
                        onChange={handleChange}/>
                        <input className='btn btn--purple post-control__btn' type="submit" value="Publicar"/>
                    </form>
                </div>

                { posts && posts.length > 0 ? posts.map((post, index) => <Post key={post.uuid} post={post}/>) : "" }
            </div>
        </>
    );
}

export default App;
