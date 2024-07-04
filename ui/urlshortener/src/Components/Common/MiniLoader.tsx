import React from 'react'

function MiniLoader({type="warning", size=100}) {
  return (
    <div>
        <div className={`spinner-border text-${type}`} style={{scale: `${size}%`}}></div>
    </div>
  )
}

export default MiniLoader