import React, { useState } from 'react'
import { MiniLoader } from '../Components/Common';
import { inputHelper } from '../Helpers';
import { useLoginMutation, useRegisterMutation } from '../api/authApi';
import { apiResponse, user } from '../Interfaces';
import { SD_General } from '../constants/constants';
import jwtDecode from 'jwt-decode';
import { setUser } from '../redux/slices/userSlice';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';

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

    const handleSubmit = async (e : React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        setIsLoading(true);

        var result:any = await register(formData);
        console.log(result);
        if (result){
            var response : apiResponse = result.error ? (result.error.data):(result.data);
            if (!result.error){
                // Automatically log user in if the registration was successful
                result = await login({
                    email: formData.email,
                    password: formData.password
                })
                response = result.error ? (result.error.data):(result.data);
                if (!result.error){
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
                    setError(response.message!);
                    console.log(response.message);
                }
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

  return (
    <div className='bg-dark vh-100 d-flex align-items-center'>
        <div className='w-100 p-5 bg-dark'>
            <div className='row w-100'>
                <span className='h1 text-center text-success'>Register</span>
            </div>
            <div className='row w-100 p-4'>
                <form onSubmit={(e) => handleSubmit(e)}>
                    <div className='m-3 d-flex justify-content-center'>
                        <input onChange={handleInputChange} value={formData.email} type="email" name="email" className='form-control w-50' placeholder='Enter Email'></input>
                    </div>
                    <div className='m-3 d-flex justify-content-center'>
                        <input onChange={handleInputChange} value={formData.username} name="username" className='form-control w-50' placeholder='Enter Username'></input>
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
                        <button type="submit" className='mt-1 btn btn-success form-control w-25'>Register</button>
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

export default Register