import React, { useState } from 'react'
import { MiniLoader } from '../Components/Common';
import { inputHelper } from '../Helpers';
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
                    id: decodedToken.id,
                    email: decodedToken.email,
                    username: decodedToken.username,
                    role: decodedToken.role
                }));
                nav("/UrlShortener");
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

  return (
    <div className='bg-dark vh-100 d-flex align-items-center'>
        <div className='w-100 p-5 bg-dark'>
            <div className='row w-100'>
                <span className='h1 text-center text-success'>Login</span>
            </div>
            <div className='row w-100 p-4'>
                <form onSubmit={handleSubmit}>
                    <div className='m-3 d-flex justify-content-center'>
                        <input onChange={handleInputChange} value={formData.email} name="email" className='form-control w-50' placeholder='Enter Email'></input>
                    </div>
                    <div className='m-3 d-flex justify-content-center'>
                        <input onChange={handleInputChange} value={formData.password} name="password" type="password" className='form-control w-50' placeholder='Enter Password'></input>
                    </div>
                    {error == "" ? (<></>):(
                        <div className='text-center'>
                            <p className='text-danger'>{error}</p>
                        </div>
                    )}
                    <div className='d-flex justify-content-center'>
                        <button type="submit" className='mt-1 btn btn-success form-control' style={{width:"5%"}}>Login</button>
                    </div>
                    {isLoading ? 
                        (<div className='mt-5 d-flex justify-content-center'>
                            <MiniLoader/>
                        </div>):
                        (<></>)
                    }
                </form>
            </div>
        </div>
    </div>
  )
}

export default Login