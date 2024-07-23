import React from 'react';
import './App.css';
import { Route, Routes } from 'react-router-dom';
import { About, AllUrls, Denied, ForgotPassword, Home, Login, NotFound, Register, ResetPassword, Urls, Verification } from './Pages';
import { Header } from './Layout';

function App() {
  return (
    <div className="App">
      <Header/>
      <div>
        <Routes>
          <Route path="/" element={<Home/>}/>
          <Route path="/ShortUrls" element={<Home/>}/>
          <Route path="*" element={<NotFound/>}/>
          <Route path="/ShortUrls/Login" element={<Login/>}/>
          <Route path="/ShortUrls/Register" element={<Register/>}/>
          <Route path="/ShortUrls/AllLinks" element={<AllUrls/>}/>
          <Route path="/ShortUrls/Links" element={<Urls/>}/>
          <Route path="/ShortUrls/AccessDenied" element={<Denied/>}/>
          <Route path="/ShortUrls/About" element={<About/>}/>
          <Route path="/ShortUrls/ForgotPassword" element={<ForgotPassword/>}/>
          <Route path="/ShortUrls/Verification/:email" element={<Verification/>}/>
          <Route path="/ShortUrls/ResetPassword/:token" element={<ResetPassword/>}/>
        </Routes>
      </div>
    </div>
  );
}

export default App;
