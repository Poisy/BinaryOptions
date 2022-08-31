import 'bootstrap/dist/css/bootstrap.css';
import "bootstrap/dist/js/bootstrap.min.js";
import {Modal, Button} from "react-bootstrap";
import {useForm} from "react-hook-form";
import {registerUser} from "../services/authenticationService";
import nationalities from '../data/nationalities'

function Register(props){
    const handleClose = () => props.setState(false);
    const handleShowLogin = () =>
    {
        props.setState(false);
        props.setLoginState(true);
    }

    const { register, setError, handleSubmit, watch, formState: { errors } } = useForm();

    async function onSubmit(data) {
        await registerUser(data).then(async res => {
            if (res.status !== 200) {
                await res.json().then(res => {
                    Object.values(res.errors).forEach(e => {
                        e.errors.forEach(e => {
                            setError('Username', 
                                { type: 'focus', message: e.errorMessage },
                                { shouldFocus: true })
                        })
                    })
                })
            }
        });
    }
    
    return (
        <Modal show={props.state} onHide={handleClose}>
            <Modal.Header closeButton>
                <Modal.Title>Register</Modal.Title>
            </Modal.Header>
            <Modal.Body className="">
                <form onSubmit={handleSubmit(onSubmit)}>
                    <div className="mb-3">
                        <label htmlFor="username" className="form-label">Username</label>
                        <input type="text" className="form-control" id="username"
                               {...register("Username", {
                                   required: 'Username is required!',
                                   minLength: { value: 3, message: 'Username has to be minimum 3 characters!' },
                                   maxLength: { value: 32, message: 'Username has to be maximum 30 characters!' } })}
                        />
                        <div className="text-danger">{errors.Username?.message}</div>
                    </div>
                    <div className="mb-3">
                        <label htmlFor="email" className="form-label">Email</label>
                        <input type="email" className="form-control" id="email" aria-describedby="emailHelp"
                               {...register("Email", {
                                   required: 'Email is required!',
                                   pattern: { value: /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/, message: 'Invalid Email!'}})}
                        />
                        <div className="text-danger">{errors.Email?.message}</div>
                        <div id="emailHelp" className="form-text">We'll never share your email with anyone else. </div>
                    </div>
                    <div className="mb-3">
                        <label htmlFor="nationality" className="form-label">Nationality</label>
                        <select className='form-select'
                                {...register("Nationality",
                                    {
                                        required: true,
                                        validate: { isNotDefault: v => v !== 'Select a Nationality'
                                                || 'Select nationality!'} })}>
                            <option defaultValue>Select a Nationality</option>
                            {
                                nationalities.map(option => (
                                    <option key={option} value={option}>{option}</option>
                                ))
                            }
                        </select>
                        <div className="text-danger">{errors.Nationality?.message}</div>
                    </div>
                    <div className='row'>
                        <div className="mb-3 col">
                            <label htmlFor="password" className="form-label">Password</label>
                            <input type="password" className="form-control" id="password"
                                   {...register("Password", {
                                       required: 'Password is required!',
                                       pattern: {
                                           value: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{6,32}$/,
                                           message: 'Password needs to be between 6 and 32 chars, has at least one upper case, one digit and one alphabetic character!'} })}
                            />
                            <div className="text-danger">{errors.Password?.message}</div>
                        </div>
                        <div className="mb-3 col">
                            <label htmlFor="confirmPassword" className="form-label">Confirm Password</label>
                            <input type="password" className="form-control" id="confirmPassword"
                                   {...register("ConfirmPassword",
                                       {
                                           required: 'ConfirmPassword is required!',
                                           validate: {
                                               passwordMatch: v => watch('Password') === v
                                                   || 'Passwords need to match!'
                                           }})}
                            />
                            <div className="text-danger">{errors.ConfirmPassword?.message}</div>
                        </div>
                    </div>
                    <Modal.Footer>
                        <Button variant="outline-primary" onClick={handleShowLogin}>
                            Already have account ?
                        </Button>
                        <Button type='submit' variant="primary">
                            Sign Up
                        </Button>
                    </Modal.Footer>
                </form>
            </Modal.Body>
        </Modal> 
    );
}

export default Register;