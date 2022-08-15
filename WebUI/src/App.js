import logo from './logo.svg';
import './css/App.css';
import { useState } from "react";
import 'bootstrap/dist/css/bootstrap.css';
import Register from "./components/Register";
import Login from "./components/Login";

function App() {
  const [showRegisterModal, setShowRegisterModal] = useState(false);
  const [showLoginModal, setShowLoginModal] = useState(false);
  const [token, setToken] = useState();
  const [user, setUser] = useState();
  
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
                  token != null ? "" :
                      <>
                        <Register state={showRegisterModal} setState={setShowRegisterModal} setLoginState={setShowLoginModal} user={user} setUser={setUser}/>
                        <Login state={showLoginModal} setState={setShowLoginModal} setRegisterState={setShowRegisterModal} setToken={setToken} user={user} setUser={setUser}/>
                      </>
                }
              </div>
            </div>
          </div>
        </nav>
      </header>
    </div>
  );
}

export default App;
