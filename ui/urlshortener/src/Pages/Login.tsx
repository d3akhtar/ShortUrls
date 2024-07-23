import React, { useEffect, useState } from 'react'
import { MiniLoader } from '../Components/Common';
import { handleGoogleAuth, inputHelper } from '../Helpers';
import { useLoginMutation } from '../api/authApi';
import { apiResponse, user } from '../Interfaces';
import { useDispatch } from 'react-redux';
import { setUser } from '../redux/slices/userSlice';
import { SD_General } from '../constants/constants';
import jwtDecode from 'jwt-decode';
import { useNavigate } from 'react-router-dom';

function Login() {

    const [login] = useLoginMutation();
    const dispatch = useDispatch();

    const initialFormData = {
        password: "",
        email: ""
    }
    const [formData,setFormData] = useState(initialFormData);
    const [error,setError] = useState<string>("");
    const [isLoading,setIsLoading] = useState<boolean>(false);

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

    const handleSubmit = async (e : any) => {
        e.preventDefault();
        setIsLoading(true);

        const result:any = await login(formData);
        console.log("result");
        console.log(result);
        if (result){
            const response : apiResponse = result.error ? (result.error):(result.data);
            if (!result.error){
                console.log(response.token);
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
            else{
                setError(result.error.data);
                console.log(result.error.data);
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
    <div className='vh-100 d-flex bg-light justify-content-center align-items-center'>
        <div className='p-5 bg-white border shadow-sm' style={{width: "400px"}}>
            <div className='row w-100'>
                <span className='h1 text-center text-dark fw-bold'>LOGIN</span>
            </div>
            <div className='row w-100 p-1'>
                <form onSubmit={handleSubmit}>
                    <label htmlFor="email" className='row text-start'>Email</label>
                    <div className='d-flex justify-content-center'>
                        <input onChange={handleInputChange} value={formData.email} name="email" className='form-control' placeholder='Enter Email' style={formStyle}></input>
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
                    <div className='d-flex justify-content-cente mt-3'>
                        <span className='p text-center lead text-dark fs-6'>Forgot Password? Click <a style={{cursor: "pointer"}} onClick={() => nav("/ShortUrls/ForgotPassword")} className='text-link'>here</a> to reset it</span>
                    </div>
                    <div className='d-flex justify-content-center'>
                        <button type="submit" className='mt-3 btn btn-success form-control' style={formStyle}>Login</button>
                    </div>
                    {isLoading ? 
                        (<div className='mt-5 d-flex justify-content-center'>
                            <MiniLoader/>
                        </div>):
                        (<></>)
                    }
                </form>
                <div className='d-flex justify-content-center mt-4'>
                    <p className='text-center text-dark lead'>Or Login With:</p>
                </div>
                <div className='d-flex justify-content-center'>
                    <button onClick={handleGoogleAuth} className='btn btn-dark' style={formStyle}><i className="bi bi-google"></i></button>
                </div>
            </div>
        </div>
    </div>
  )
}

export default Login