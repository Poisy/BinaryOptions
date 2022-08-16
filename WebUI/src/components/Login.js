import React from 'react';
import 'bootstrap/dist/css/bootstrap.css';
import "bootstrap/dist/js/bootstrap.min.js";
import {Modal, Button, Form} from "react-bootstrap";
import {useForm} from "react-hook-form";
import {loginUser, registerUser, getUser, doesUserExist} from "../services/authenticationService";
import {GoogleLogin, googleLogout} from "@react-oauth/google";
import FacebookLogin from 'react-facebook-login';
import jwt_decode from 'jwt-decode';

googleLogout();

const Login = (props) => {
    const handleClose = () => props.setState(false);
    const handleShow = () => props.setState(true);
    const handleShowRegister = () =>
    {
        props.setState(false);
        props.setRegisterState(true);
    }

    const { register, handleSubmit, setError, formState: { errors } } = useForm();

    async function onSubmit(data) {
        await loginUser(data)
            .then(async res => {
                if (res.status === 200) {
                    await res.json().then(async res => {
                        let token = res.token;
                        await getUser(token).then(async res => {
                            if (res.status === 200) {
                                await res.json().then(res => {
                                    props.login(res, token);
                                })
                            } else {
                                alert('Something went wrong...');
                            }
                        });
                    });
                } else {
                    setError('error', 
                        { type: 'focus', message: 'Wrong Email or Password!' }, 
                        { shouldFocus: true })
                }
            });
    }
    
    async function handleGoogleLogin(result) {
        let objResult = jwt_decode(result.credential);
        await doesUserExist(objResult.email).then(res => {
            res.json().then(async res => {
                if (res.exist) {
                    await onSubmit({
                        email: objResult.email,
                        thirdParty: true
                    })
                } else {
                    await registerUser({
                        username: objResult.email,
                        email: objResult.email,
                        nationality: 'Unknown',
                        thirdParty: true
                    }).then(async res => await onSubmit({
                        email: objResult.email,
                        thirdParty: true
                    }));
                }
            })
        })
    }

    const handleFacebookLogin = (response) => {
        doesUserExist(response.email).then(res => {
            res.json().then(async res => {
                if (res.exist) {
                    await onSubmit({
                        email: response.email,
                        thirdParty: true
                    })
                } else {
                    await registerUser({
                        username: response.email,
                        email: response.email,
                        nationality: 'Unknown',
                        thirdParty: true
                    }).then(async res => await onSubmit({
                        email: response.email,
                        thirdParty: true
                    }));
                }
            })
        })
    }

    return (
        props.user != null ? "" :
            <>
                <Button variant="outline-success" onClick={handleShow}>
                    Sign In
                </Button>

                <Modal show={props.state} onHide={handleClose}>
                    <Modal.Header closeButton>
                        <Modal.Title>Login</Modal.Title>
                    </Modal.Header>
                    <Modal.Body className="">
                        <form onSubmit={handleSubmit(onSubmit)}>
                            <div className="text-danger">{errors.error?.message}</div>
                            <div className="mb-3">
                                <label htmlFor="email" className="form-label">Email</label>
                                <input type="email" className="form-control" id="email" aria-describedby="emailHelp"
                                       {...register("Email")}
                                />
                                <div className="text-danger">{errors.Email?.message}</div>
                            </div>
                            <div className='row'>
                                <div className="mb-3 col">
                                    <label htmlFor="password" className="form-label">Password</label>
                                    <input type="password" className="form-control" id="password"
                                           {...register("Password")}
                                    />
                                </div>
                            </div>
                            <Modal.Footer>
                                <Button variant="outline-primary" onClick={handleShowRegister}>
                                    Don't have account ?
                                </Button>
                                <Button type='submit' variant="primary">
                                    Sign In
                                </Button>
                                <GoogleLogin
                                    onSuccess={handleGoogleLogin}>
                                </GoogleLogin>
                                <FacebookLogin
                                    appId={process.env.REACT_APP_FACEBOOK_APP_ID}
                                    fields="name,email,picture"
                                    callback={handleFacebookLogin}
                                    size='small'/>
                            </Modal.Footer>
                        </form>
                    </Modal.Body>
                </Modal>
            </>
    );
};  

export default Login;
