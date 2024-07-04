import React from 'react';
import './App.css';
import { Route, Routes } from 'react-router-dom';
import { About, AllUrls, Denied, Home, Login, NotFound, Register, Urls } from './Pages';
import { Header } from './Layout';

function App() {
  return (
    <div className="App">
      <Header/>
      <div>
        <Routes>
          <Route path="/" element={<Home/>}/>
          <Route path="/UrlShortener" element={<Home/>}/>
          <Route path="*" element={<NotFound/>}/>
          <Route path="/UrlShortener/Login" element={<Login/>}/>
          <Route path="/UrlShortener/Register" element={<Register/>}/>
          <Route path="/UrlShortener/AllLinks" element={<AllUrls/>}/>
          <Route path="/UrlShortener/Links" element={<Urls/>}/>
          <Route path="/UrlShortener/AccessDenied" element={<Denied/>}/>
          <Route path="/UrlShortener/About" element={<About/>}/>
        </Routes>
      </div>
    </div>
  );
}

export default App;
