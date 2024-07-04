import React from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { Link, useNavigate } from 'react-router-dom'
import checkForToken from '../Helpers/checkForToken'
import { clearUser } from '../redux/slices/userSlice';
import { SD_General, SD_ROLES } from '../constants/constants';
import { user } from '../Interfaces';

export default function Header() {

    const dispatch = useDispatch();
    const loggedInUser : user = useSelector((state : any) => state.userStore);
    const nav = useNavigate();

    const handleLogout = () =>{
        dispatch(clearUser());
        localStorage.removeItem(SD_General.tokenKey);
        nav("/UrlShortener");
    }

  return (
    <nav className="navbar navbar-expand-lg navbar-light w-100 px-4" style={{position:"fixed", backgroundColor:"#e3f2fd", zIndex:10}}>
        <Link className="navbar-brand" to="/UrlShortener">UrlShortener</Link> 
        <div className="collapse navbar-collapse justify-content-between" id="navbarSupportedContent">
            <ul className="navbar-nav mr-auto">
                {loggedInUser.role == SD_ROLES.Admin ? 
                    (
                        <li className="nav-item dropdown">
                            <a className="nav-link dropdown-toggle" id="navbarDropdown" role="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                Admin
                            </a>
                            <div className="dropdown-menu" aria-labelledby="navbarDropdown">
                                <Link className="nav-link ms-1" to="/UrlShortener/Links"><span className='text-center'>My Urls</span></Link>
                                <Link className="nav-link ms-1" to="/UrlShortener/AllLinks"><span className='text-center'>All Urls</span></Link>
                                <Link className="nav-link ms-1" to="/UrlShortener/AllAliases"><span className='text-center'>All Aliases</span></Link>
                            </div>
                        </li>
                    ):
                    (
                        <li className="nav-item">
                            <Link className="nav-link" to="/UrlShortener/Links"><span className='text-center'>My Urls</span></Link>
                        </li>
                    )
                }
            <li className="nav-item">
                <Link className="nav-link" to="/UrlShortener/About"><span className='text-center'>About</span></Link>
            </li>   
            </ul>
            {loggedInUser.id != "" ? 
                (
                    <ul className='navbar-nav ml-auto'>
                        <li className="nav-item">
                            <a onClick={handleLogout} className="nav-link" style={{cursor:"pointer"}}><span className='text-center'>Logout</span></a>
                        </li>
                        <li className="nav-item">
                            <span className="nav-link"><span className='text-center'>{`${loggedInUser.username}`}</span></span>
                        </li>
                    </ul>
                ):
                (
                    <ul className='navbar-nav ml-auto'>
                        <li className="nav-item">
                            <Link className="nav-link" to="/UrlShortener/Register"><span className='text-center'>Register</span></Link>
                        </li>
                        <li className="nav-item">
                            <Link className="nav-link" to="/UrlShortener/Login"><span className='text-center'>Login</span></Link>
                        </li>
                    </ul>
                )
            }
        </div>
    </nav>
  )
}
