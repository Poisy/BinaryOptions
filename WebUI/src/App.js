import './css/App.css';
import React, {useEffect, useState} from "react";
import 'bootstrap/dist/css/bootstrap.css';
import Register from "./components/Register";
import Login from "./components/Login";
import {Button} from "react-bootstrap";
import { useCookies } from "react-cookie";
import Charts from "./components/Charts";
import Profile from "./components/Profile";
import {getUser} from "./services/authenticationService";

function App() {
  const [cookies, setCookie, removeCookie] = useCookies();
  
  const [showRegisterModal, setShowRegisterModal] = useState(false);
  const [showLoginModal, setShowLoginModal] = useState(false);
  const [token, setToken] = useState(cookies['token']);
  const [user, setUser] = useState({});

  const [defaultCurrency, setDefaultCurrency] = useState(cookies['defaultCurrency'] ?? 'AUDJPY');
  const [defaultChartType, setDefaultChartType] = useState(cookies['defaultChartType'] ?? 'candlestick');
  const [defaultPayout, setDefaultPayout] = useState(cookies['defaultPayout'] ?? 10);

  useEffect(() => {
    if (token != null && JSON.stringify(user) === '{}') {
      updateUser();
    }
  }, []);

  
  
  function login(userArg, tokenArg) {
    setUser(userArg);
    setToken(tokenArg);
    
    setCookie('token', tokenArg, { maxAge: 86400 })
  }
  
  function logout() {
    setUser(null);
    setToken(null);
    
    removeCookie('token');
  }
  
  function updateUser() {
    getUser(token).then(async res => {
      if (res.status === 200) {
        await res.json().then(res => {
          login(res, token);
        })
      } else {
        alert('Something went wrong...');
      }});
  }
  
  function updateSettings(currency, chartType, payout) {
    setDefaultCurrency(currency);
    setDefaultChartType(chartType);
    setDefaultPayout(payout);

    setCookie('defaultCurrency', currency, { maxAge: Number.MAX_SAFE_INTEGER });
    setCookie('defaultChartType', chartType, { maxAge: Number.MAX_SAFE_INTEGER });
    setCookie('defaultPayout', payout, { maxAge: Number.MAX_SAFE_INTEGER });
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
                  <a className="nav-link" href="#"></a>
                </li>
              </ul>
              <div className="d-flex">
                {
                  token == null ?
                      <>
                        <Register state={showRegisterModal} setState={setShowRegisterModal} setLoginState={setShowLoginModal} user={user}/>
                        <Login state={showLoginModal} setState={setShowLoginModal} setRegisterState={setShowRegisterModal} user={user} login={login}/>
                      </> :
                      <>
                        <Profile 
                            user={user} 
                            updateSettings={updateSettings}
                            defaultCurrency={defaultCurrency}
                            defaultChartType={defaultChartType}
                            defaultPayout={defaultPayout}>
                        </Profile>
                        <Button variant='outline-success' onClick={logout}>Logout</Button>
                      </>
                }
              </div>
            </div>
          </div>
        </nav>
      </header>
      <div className='container align-items-center'>
        {
          token == null ? "Please sing in to see the charts..." :
              <Charts 
                  token={token} 
                  updateUser={updateUser} 
                  user={user}
                  defaultCurrency={defaultCurrency}
                  defaultChartType={defaultChartType}
                  defaultPayout={defaultPayout}>
              </Charts>
        }
      </div>
    </div>
  );
}

export default App;
