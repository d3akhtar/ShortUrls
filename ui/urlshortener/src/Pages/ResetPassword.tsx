import React, { useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { useResetPasswordMutation } from '../api/authApi';
import { MiniLoader } from '../Components/Common';
import { inputHelper } from '../Helpers';

function ResetPassword() {

    const { token } = useParams();

    const initialFormData = {
        password: "",
        confirmPassword: ""
    };
    
    const [formData,setFormData] = useState(initialFormData);
    const [error,setError] = useState<string>("");
    const [isLoading,setIsLoading] = useState<boolean>(false);
    const [resetPassword] = useResetPasswordMutation();

    const nav = useNavigate();

    const handleSubmit = async (e : any) => {
        e.preventDefault();
        setError("");
        setIsLoading(true);

        if (formData.password != formData.confirmPassword){
            setIsLoading(false);
            setError("Passwords do not match.");
            return;
        }

        const result:any = await resetPassword({
            token: token,
            password: formData.password
        });

        console.log("result");
        console.log(result);
        if (result){
            if (!result.error){
                nav("/ShortUrls/Login");
            }
            else{
                setError(result.error.data.message);
                console.log(result.error.data.message);
                setIsLoading(false);
            }
        }
        else{
            setError("Unknown error");
            console.log("Unknown error");
            setIsLoading(false);
        }
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
                <div className='row w-100'>
                    <span className='h3 text-center text-dark fw-bold'>RESET PASSWORD</span>
                </div>
                <div className='row w-100 p-4'>
                    <form onSubmit={handleSubmit}>
                        <div className='m-3 d-flex justify-content-center'>
                            <input onChange={handleInputChange} value={formData.password} name="password" type="password" className='form-control' style={formStyle} placeholder='Enter New Password'></input>
                        </div>
                        <div className='m-3 d-flex justify-content-center'>
                            <input onChange={handleInputChange} value={formData.confirmPassword} name="confirmPassword" type="password" className='form-control' style={formStyle} placeholder='Confirm Password'></input>
                        </div>
                        {error == "" ? (<></>):(
                            <div className='text-center'>
                                <p className='text-danger'>{error}</p>
                            </div>
                        )}
                        <div className='d-flex justify-content-center mb-3'>
                            <button type="submit" className='mt-1 btn btn-success form-control' style={formStyle}>Reset Password</button>
                        </div>
                        <span className='lead fs-6 text-dark text-center'>If the password is reset successfully, you will be redirected to the login page, where you can enter your new login details</span>
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

export default ResetPassword