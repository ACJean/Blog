import React, { useEffect, useState } from 'react';
import Alert from './components/Alert';
import Post from './components/Post';
import { ALL_POST, CREATE_POST } from './consts/Api';
import PostDTO from './objs/PostDTO';
import httpUtil from './utils/HttpUtil';

function App() {

    const [posts, setPosts] = useState([]);
    const [post, setPost] = useState(new PostDTO());

    const [showAlert, setShowAlert] = useState(false);
    const [messageAlert, setMessageAlert] = useState([]);
    const [typeAlert, setTypeAlert] = useState("");

    const token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI2MmIzOWU3MGZhMmIyMGVjNThlNDg2YmIiLCJuYmYiOjE2NTYyMTA1NzQsImV4cCI6MTY1NjIyMTM3NCwiaWF0IjoxNjU2MjEwNTc0fQ.L8VAaTBSaRBrXLrXVktpcHZ-xRFQXrDM7uav-V2csqU";

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
                setPost(new PostDTO());
                alert('success', serviceResponse.message);
            }
            else if (response.status === 400 && !showAlert) {
                alert('warning', serviceResponse.validationMessages);
            }
        } catch {

        }
    }

    const alert = (type, message) => {
        let fnAlert = () => {
            setShowAlert(true);
            setMessageAlert(message);
            setTypeAlert(type);
            setTimeout(() => setShowAlert(false), 5000);
        }
        if (showAlert) setTimeout(fnAlert, 4000);
        else fnAlert();
    }

    return (
        <>
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
            <Alert show={showAlert} type={typeAlert} messages={messageAlert}/>
        </>
    );
}

export default App;
