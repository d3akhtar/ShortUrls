import React, { useEffect, useState } from 'react'
import { MiniLoader } from '../Components/Common';
import { handleGoogleAuth, inputHelper } from '../Helpers';
import { useLoginMutation, useRegisterMutation } from '../api/authApi';
import { apiResponse, user } from '../Interfaces';
import { SD_General } from '../constants/constants';
import jwtDecode from 'jwt-decode';
import { setUser } from '../redux/slices/userSlice';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import Cookies from "js-cookie";

function Register() {

    const initialFormData = {
        username: "",
        password: "",
        email: "",
        role: "user"
    }
    const [formData,setFormData] = useState(initialFormData);
    const [error,setError] = useState<string>("");
    const [isLoading,setIsLoading] = useState<boolean>(false);

    const [register] = useRegisterMutation();
    const [login] = useLoginMutation();
    const dispatch = useDispatch();

    const nav = useNavigate();

    useEffect(() => {
        const accessTokenRegex = /access_token=([^&]+)/;
        const isMatch = window.location.href.match(accessTokenRegex);
    
        if (isMatch) {
          const accessToken = isMatch[1];
          console.log("accessToken");
          console.log(accessToken);
        
          const getAccessToken = async () => {
            var result = await fetch(`https://shorturls.danyalakt.com/auth/external-login?thirdPartyName=google&accessToken=${accessToken}`);
            var response : apiResponse = await result.json();
            console.log(response);

            localStorage.setItem(SD_General.tokenKey,response.token!);
            const decodedToken : user = jwtDecode(response.token!);
            dispatch(setUser({
                userId: decodedToken.userId,
                email: decodedToken.email,
                username: decodedToken.username,
                role: decodedToken.role
            }));
            nav("/ShortUrls");
          }

          getAccessToken();
          //Cookies.set("access_token", accessToken); I don't need the access token for long, just need it to register user email and name
        }
      }, []);

    const handleSubmit = async (e : React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        setIsLoading(true);

        var result:any = await register(formData);
        console.log(result);
        if (result){
            var response : apiResponse = result.error ? (result.error.data):(result.data);
            if (!result.error){
                nav(`/ShortUrls/Verification/${formData.email}`);
            }
            else{
                console.log(response);
                setError(response.message!);
                console.log(response.message);
            }
        }
        else{
            setError("Unknown error");
            console.log("Unknown error");
        }

        setIsLoading(false);
    }
    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const newFormData = inputHelper(e, formData);
        setFormData(newFormData);
    }

    const formStyle :any = {
        width: "25%",
        minWidth: "300px"
    }

  return (
    <div className='bg-light vh-100 d-flex justify-content-center align-items-center'>
        <div className='p-5 bg-white border shadow-sm' style={{width: "400px"}}>
            <div className='row w-100 p-1'>
                <span className='h1 text-center text-dark fw-bold'>REGISTER</span>
            </div>
            <div className='row w-100 p-1'>
                <form onSubmit={(e) => handleSubmit(e)}>
                    <label htmlFor="email" className='row text-start'>Email</label>
                    <div className='d-flex justify-content-center'>
                        <input onChange={handleInputChange} id="email" value={formData.email} type="email" name="email" className='form-control' placeholder='Enter Email' style={formStyle}></input>
                    </div>
                    <label htmlFor="username" className='row text-start mt-3'>Username</label>
                    <div className='d-flex justify-content-center'>
                        <input onChange={handleInputChange} value={formData.username} id="username" name="username" className='form-control' placeholder='Enter Username' style={formStyle}></input>
                    </div>
                    <label htmlFor="password" className='row text-start mt-3'>Password</label>
                    <div className='d-flex justify-content-center'>
                        <input onChange={handleInputChange} value={formData.password} id="password" name="password" type="password" className='form-control' placeholder='Enter Password' style={formStyle}></input>
                    </div>
                    {error == "" ? (<></>):(
                        <div className='text-center'>
                            <p className='text-danger'>{error}</p>
                        </div>
                    )}
                    <div className='d-flex justify-content-center'>
                        <button type="submit" className='mt-3 btn btn-success form-control' style={formStyle}>Register</button>
                    </div>
                    {isLoading ? 
                        (<div className='mt-5 d-flex justify-content-center'>
                            <MiniLoader/>
                        </div>):
                        (<></>)
                    }
                </form>
                <div className='d-flex justify-content-center mt-3'>
                    <p className='text-center text-dark lead'>Or Register With:</p>
                </div>
                <div className='d-flex justify-content-center'>
                    <button onClick={handleGoogleAuth} className='mt-1 btn btn-dark' style={formStyle}><i className="bi bi-google"></i></button>
                </div>
            </div>
        </div>
    </div>
  )
}

export default Register