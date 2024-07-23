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
        nav("/ShortUrls");
    }

  return (
    <nav className="navbar bg-dark navbar-expand navbar-dark w-100 px-3" style={{position:"fixed", backgroundColor:"#e3f2fd", zIndex:10}}>
        <Link className="navbar-brand" to="/ShortUrls">ShortUrls</Link> 
        <div className="collapse navbar-collapse justify-content-between" id="navbarSupportedContent">
            <ul className="navbar-nav mr-auto">
                {loggedInUser.role == SD_ROLES.Admin ? 
                    (
                        <li className="nav-item dropdown">
                            <a className="nav-link dropdown-toggle" id="navbarDropdown" role="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                Admin
                            </a>
                            <div className="dropdown-menu" aria-labelledby="navbarDropdown">
                                <Link className="nav-link ms-1 text-dark" to="/ShortUrls/Links"><span className='text-center'>My Urls</span></Link>
                                <Link className="nav-link ms-1 text-dark" to="/ShortUrls/AllLinks"><span className='text-center'>All Urls</span></Link>
                            </div>
                        </li>
                    ):
                    (
                        <li className="nav-item">
                            <Link className="nav-link" to="/ShortUrls/Links"><span className='text-center'>Urls</span></Link>
                        </li>
                    )
                }
            <li className="nav-item">
                <Link className="nav-link" to="/ShortUrls/About"><span className='text-center'>About</span></Link>
            </li>   
            </ul>
            {loggedInUser.userId != "" ? 
                (
                    <ul className='navbar-nav ml-auto'>
                        <li className="nav-item">
                            <a onClick={handleLogout} className="nav-link" style={{cursor:"pointer"}}><span className='text-center'>Logout</span></a>
                        </li>
                        <li className="nav-item">
                            <span className="nav-link"><span className='badge text-center'>{`${loggedInUser.username}`}</span></span>
                        </li>
                    </ul>
                ):
                (
                    <ul className='navbar-nav ml-auto'>
                        <li className="nav-item">
                            <Link className="nav-link" to="/ShortUrls/Register"><span className='text-center'>Register</span></Link>
                        </li>
                        <li className="nav-item">
                            <Link className="nav-link" to="/ShortUrls/Login"><span className='text-center'>Login</span></Link>
                        </li>
                    </ul>
                )
            }
        </div>
    </nav>
  )
}
