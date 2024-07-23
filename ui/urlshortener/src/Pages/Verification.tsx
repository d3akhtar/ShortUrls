import React, { useEffect, useState } from 'react'
import { inputHelper } from '../Helpers';
import { useNavigate, useParams } from 'react-router-dom';
import { MiniLoader } from '../Components/Common';
import { apiResponse } from '../Interfaces';

function Verification() {

    const { email } = useParams();
    const [error,setError] = useState<string>("");
    const [receivedAlreadyVerifiedMessage,setReceivedAlreadyVerifiedMessage] = useState<boolean>();
    const [emailSent,setEmailSent] = useState<boolean>(false);
    const [isLoading,setIsLoading] = useState<boolean>(false);

    const nav = useNavigate();

    const sendVerificationEmail = async() => {
        setEmailSent(false);
        setError("");
        setReceivedAlreadyVerifiedMessage(false);

        setIsLoading(true);

        var result = await fetch(`http://localhost:5219/api/auth/send-verification-email?email=${email}`);
        console.log(result);
        var response : apiResponse = await result.json();
        
        console.log("response");
        console.log(response);
        if (result){
            if (result.status === 200){
                setEmailSent(true);
            }
            else{
                setError(response.message!);
                console.log(response.message!);
                if (response.message == "Account is already verified, try logging in."){
                    setReceivedAlreadyVerifiedMessage(true)
                }
            }
        }
        else{
            setError("Unknown error");
            console.log("Unknown error");
        }
        setIsLoading(false);
    }

    return (
        <div className='bg-light vh-100 d-flex justify-content-center align-items-center'>
            <div className='py-5 px-3 bg-white border shadow-sm' style={{width: "400px"}}>
                <div className='row w-100'>
                    <span className='h4 text-center text-dark fw-bold'>ACCOUNT VERIFICATION</span>
                </div>
                <div className='row w-100'>
                    <span className='lead text-center text-dark fs-6 mt-2'>An email was sent to {email} containing a link to verify your account. You <span className='fw-bold'>must</span> verify your account before you can log in.</span>
                </div>
                <div className='row w-100'>
                    <span className='badge text-dark mt-3'>Didn't get an email?. Click <a onClick={async () => await sendVerificationEmail()} className='text-link'>here</a> to resend.</span>
                </div>
                {error == "" ? (<></>):(
                    <div className='row w-100 mt-2'>
                        <p className='text-danger'>{error}</p>
                    </div>
                )}
                {emailSent ? 
                    (
                        <div className='row w-100 mt-2'>
                            <p className='text-success'>An email with a verification link was sent.</p>
                        </div>
                    )
                    :
                    (
                        <></>
                    )
                }
                <div className='row w-100 d-flex justify-content-center'>
                    <button className='btn btn-dark w-75 mt-3' onClick={() => nav("/ShortUrls/Register")}>Back To Register Page</button>
                    <button className={`btn btn-${receivedAlreadyVerifiedMessage ? 'primary':'dark'} w-75 mt-1`} onClick={() => nav("/ShortUrls/Login")}>Back To Login Page</button>
                </div>
                {isLoading ? 
                    (<div className='mt-5 d-flex justify-content-center'>
                        <MiniLoader/>
                    </div>):
                    (<></>)
                }
            </div>
        </div>
    )
}

export default Verification