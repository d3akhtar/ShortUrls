import React from 'react'

function About() {
  return (
    <div className='vh-100 bg-white w-100 d-flex justify-content-center' style={{overflow: "auto"}}>
        <div className='w-75'>
            <div className='row w-100' style={{height:"12%"}}></div>
            <div className='row w-100 d-flex text-start border p-5 bg-light shadow-sm'>
                <span className='fs-4 fw-bold text-dark lead'>About: </span>
                <span className='mt-4 p fs-6'>
                    If you want to shorten long URLs, go to the home page and paste any link and save the shortened link by copying it
                    and saving it somewhere. If you want to have a link with a name of your choice, enter an alias, and as long as the
                    alias hasn't been used before and is <b>at most</b> 20 characters, you will get a shortened link with your alias. You can also download a QR code, either
                    as a PNG, SVG, or ASCII text. <br/><br/>

                    If you are logged in, whenever you save a link, it will get added to your own list, which you can access by clicking on
                    "My Urls" on the navigation bar. <br/><br/>

                    Admins (currently only me) have the option to <b>change</b>  what links certain codes lead to, and can also 
                    change the names of aliases. <br/><br/>

                    Another note, since the domain name for this website is long, for shorter links (for example, a youtube video link), 
                    you will probably end up with a longer url. So unless you want to attach an alias to the link, it is best to shorten
                    very long urls (such as urls with multiple or long query parameters), unless I decide to use a host with a shorter
                    name. <br/><br/>

                    For any bugs or features you want added, send an email to <a href="mailto:shorturls@danyalakt.com">shorturls@danyalakt.com</a> <br/>
                    If you wish to contact me directly, send an email to <a href="mailto:danyal.akhtar@torontomu.ca">danyal.akhtar@torontomu.ca</a>
                </span>
            </div>
        </div>
    </div>
  )
}

export default About