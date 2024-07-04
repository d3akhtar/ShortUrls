import React from 'react'
import { useNavigate } from 'react-router-dom'

function Denied() {
  const nav = useNavigate();
  return (
    <div className='bg-dark vh-100 d-flex align-items-center justify-content-center'>
      <div className='container'>
        <div className='h1 text-danger'>You Don't Have Permission To View This Page!</div>
        <button onClick={() => nav("/UrlShortener")} className='btn w-25 mt-2 btn-light'>Home</button>
      </div>
    </div>
  )
}

export default Denied