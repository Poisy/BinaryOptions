import React from 'react';
import 'bootstrap/dist/css/bootstrap.css';
import "bootstrap/dist/js/bootstrap.min.js";
import {Modal, Button, Form} from "react-bootstrap";
import {useForm} from "react-hook-form";
import {loginUser, getUser} from "../services/authenticationService";

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
                        props.setToken(res.token);
                        await getUser(res.token).then(async res => {
                            if (res.status === 200) {
                                await res.json().then(res => {
                                    props.setUser(res);
                                })
                            } else {
                                alert('Something went wrong...');
                            }
                        });
                    });
                } else {
                    setError('Email', 
                        { type: 'focus', message: 'Wrong Email or Password!' }, 
                        { shouldFocus: true })
                }
            });
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
                            </Modal.Footer>
                        </form>
                    </Modal.Body>
                </Modal>
            </>
    );
};  

export default Login;
