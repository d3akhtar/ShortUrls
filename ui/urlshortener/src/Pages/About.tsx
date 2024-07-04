import React from 'react'

function About() {
  return (
    <div className='vh-100 bg-dark w-100 d-flex justify-content-center' style={{overflow: "auto"}}>
        <div className='w-75'>
            <div className='row w-100' style={{height:"12%"}}></div>
            <div className='row w-100 d-flex text-start border p-5' style={{backgroundColor: "#3e454d"}}>
                <span className='h1 text-info'>About: </span>
                <span className='mt-4 h5 text-white'>
                    If you want to shorten long URLs, go to the home page and paste any link and save the shortened link by copying it
                    and saving it somewhere. If you want to have a link with a name of your choice, enter an alias, and as long as the
                    alias hasn't been used before, you will get a shortened link with your alias. You can also download a QR code, either
                    as a PNG, SVG, or ASCII text. <br/><br/>

                    If you are logged in, whenever you save a link, it will get added to your own list, which you can access by clicking on
                    "My Urls" on the navigation bar.

                    Just to note, admins have the option to change what links certain codes lead to, and can also change the names of aliases. 
                </span>
            </div>
        </div>
    </div>
  )
}

export default About