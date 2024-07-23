import React, { useState } from 'react'
import { useNavigate } from 'react-router-dom';
import { useForgotPasswordMutation } from '../api/authApi';
import { apiResponse } from '../Interfaces';
import { MiniLoader } from '../Components/Common';
import { inputHelper } from '../Helpers';

function ForgotPassword() {

    const initialFormData = {
        email: "",
    }

    const [formData,setFormData] = useState(initialFormData);
    const [error,setError] = useState<string>("");
    const [isLoading,setIsLoading] = useState<boolean>(false);

    const [forgotPassword] = useForgotPasswordMutation();

    const [emailSent,setEmailSent] = useState<boolean>(false);

    const handleSendPasswordResetLink = async() => {
        setError("");
        setEmailSent(false);
        setIsLoading(true);

        const result:any = await forgotPassword(formData.email);
        console.log("result");
        console.log(result);
        if (result){
            if (!result.error){
                setEmailSent(true);
            }
            else{
                setError(result.error.data.message);
                console.log(result.error.data.message);
            }
        }
        else{
            setError("Unknown error");
            console.log("Unknown error");
        }

        setIsLoading(false);
    }

    const handleSubmit = async (e : any) => {
        e.preventDefault();
        handleSendPasswordResetLink();
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
        { emailSent ? 
            (
                <div className='p-5 bg-white border shadow-sm' style={{width: "400px"}}>
                    <div className='row w-100'>
                        <span className='h3 text-center text-success'>Email Has Been Sent!</span>
                    </div>
                    <div className='row w-100 p-4 text-dark text-center'>
                        <span className='p lead mb-3'>Check your inbox for a link to reset your password.</span>
                        <span className='fs-6 mb-4'>The email may take a few minutes...</span>
                        <span className='p'>Didn't get an email? Try <a className='text-link' style={{cursor: "pointer"}} onClick={() => handleSendPasswordResetLink()}>again</a></span>
                        <span className='p mt-1'><a className='badge text-dark' style={{cursor: "pointer"}} onClick={() => setEmailSent(false)}>Reset another account's password</a></span>
                    </div>
                </div>
            ):
            (
                <div className='p-5 bg-white border shadow-sm' style={{width: "400px"}}>
                    <div className='row w-100'>
                        <span className='h3 text-center text-dark fw-bold'>FORGOT PASSWORD</span>
                    </div>
                    <div className='row w-100 p-4'>
                        <form onSubmit={handleSubmit}>
                            <div className='m-3 d-flex justify-content-center'>
                                <input onChange={handleInputChange} value={formData.email} name="email" type="email" className='form-control' placeholder='Enter Email' style={formStyle}></input>
                            </div>
                            {error == "" ? (<></>):(
                                <div className='text-center'>
                                    <p className='text-danger'>{error}</p>
                                </div>
                            )}
                            <div className='d-flex justify-content-center'>
                                <button type="submit" className='mt-1 btn btn-success form-control' style={formStyle}>Send Link</button>
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
            )
        }
    </div>
  )
}

export default ForgotPassword

