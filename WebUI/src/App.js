import './css/App.css';
import React, {useState} from "react";
import 'bootstrap/dist/css/bootstrap.css';
import Register from "./components/Register";
import Login from "./components/Login";
import {Button} from "react-bootstrap";
import { useCookies } from "react-cookie";
import Charts from "./components/Charts";

function App() {
  const [cookies, setCookie, removeCookie] = useCookies();
  
  const [showRegisterModal, setShowRegisterModal] = useState(false);
  const [showLoginModal, setShowLoginModal] = useState(false);
  const [token, setToken] = useState(cookies['token']);
  const [user, setUser] = useState(cookies['user']);
  
  function login(userArg, tokenArg) {
    setUser(userArg);
    setToken(tokenArg);
    
    setCookie('user', userArg, { maxAge: 86400 })
    setCookie('token', tokenArg, { maxAge: 86400 })
  }
  
  function logout() {
    setUser(null);
    setToken(null);
    
    removeCookie('user');
    removeCookie('token');
  }
  
  return (
    <div className="App">
      <header className="">
        <nav className="navbar navbar-expand-lg navbar-light bg-light">
          <div className="container-fluid">
            <a className="navbar-brand" href="#">Home</a>
            <button className="navbar-toggler" type="button" data-bs-toggle="collapse"
                    data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent"
                    aria-expanded="false" aria-label="Toggle navigation">
              <span className="navbar-toggler-icon"></span>
            </button>
            <div className="collapse navbar-collapse" id="navbarSupportedContent">
              <ul className="navbar-nav me-auto mb-2 mb-lg-0">
                <li className="nav-item">
                  <a className="nav-link" href="#">Hello {user?.username}</a>
                </li>
              </ul>
              <div className="d-flex">
                {
                  token == null ?
                      <>
                        <Register state={showRegisterModal} setState={setShowRegisterModal} setLoginState={setShowLoginModal} user={user}/>
                        <Login state={showLoginModal} setState={setShowLoginModal} setRegisterState={setShowRegisterModal} user={user} login={login}/>
                      </> :
                      <Button variant='outline-success' onClick={logout}>Logout</Button>
                }
              </div>
            </div>
          </div>
        </nav>
      </header>
      <div className='container align-items-center'>
        {
          token == null ? "Please sing in to see the charts..." :
              <Charts token={token}></Charts>
        }
      </div>
    </div>
  );
}

export default App;
