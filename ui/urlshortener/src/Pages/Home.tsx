import React, { useState } from 'react'
import { useAddUrlMutation } from '../api/urlShortenerApi'
import { MainLoader } from '../Components/Common';
import { extractCodeFromShortUrl, inputHelper } from '../Helpers';
import { apiResponse, urlCode, user } from '../Interfaces';
import { useNavigate } from 'react-router-dom';
import { isElementAccessExpression } from 'typescript';
import { useSelector } from 'react-redux';
import { useAddUserUrlMutation } from '../api/userUrlsApi';

function Home() {

    const loggedInUser : user = useSelector((state : any) => state.userStore);
    
    const initialFormData = {
        url: "",
        alias: ""
    }

    const navigate = useNavigate();

    const [formData,setFormData] = useState(initialFormData);
    const [qrCodeString,setQrCodeString] = useState("");
    const [isLoading,setIsLoading] = useState<boolean>(false);
    const [shortLink,setShortLink] = useState("");
    const [showQrCode,setShowQrCode] = useState<boolean>(false);

    const [addUrl] = useAddUrlMutation();
    const [addUserUrl] = useAddUserUrlMutation();
    
    const handleChange = (e : React.ChangeEvent<HTMLInputElement>) => {
        const newData = inputHelper(e, formData);
        setFormData(newData);
    }

    const handleSubmit = async () => {
        setIsLoading(true);

        var result:any = await addUrl({
            url: formData.url,
            alias: formData.alias
        });
        console.log(result);

        const response : apiResponse = result.error ? (result.error):(result.data);
        if (result.error){
            console.log(result.error);
            setIsLoading(false);
            return;
        }
        else{
            if (formData.alias != ""){
                setShortLink(response.shortenedUrlWithAlias!);
                //setQrCodeString(urlCode.pngQrCodeImage!);
                if (loggedInUser.id != ""){
                    await addUserUrl({
                        userId: loggedInUser.id,
                        codes: [extractCodeFromShortUrl(response.shortenedUrlWithAlias!)]
                    });
                }
            }
            else{
                setShortLink(response.shortenedUrl!);
                //setQrCodeString(urlCode.pngQrCodeImage!);
                if (loggedInUser.id != ""){
                    await addUserUrl({
                        userId: loggedInUser.id,
                        codes: [extractCodeFromShortUrl(response.shortenedUrl!)]
                    });
                }
            }
        }

        setIsLoading(false);
    }

    const handleVisit = (e : React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
        console.log(e);
        if (e.button == 1){
            window.open(shortLink, "_blank");
        }
        else{
            window.location.href = shortLink;
        }
    }

    return (
        isLoading ? (<MainLoader/>):
        (shortLink == "" ? (
            <div className='text-white'>
                <div className='vh-100 d-flex justify-content-center align-items-center w-100'>
                    <div className='row w-50 border'>
                        <form onSubmit={handleSubmit} className='p-5 bg-dark'>
                            <div className='text-info h5 text-start'>Shorten A Long Url</div>
                            <input name="url" className='form-control mt-2' onChange={handleChange} value={formData.url} placeholder='Enter url here'></input>
                            <div className='text-info text-start mt-4'>Enter an alias here (optional)</div>
                            <input name="alias" className='form-control mt-2' onChange={handleChange} value={formData.alias} placeholder='Enter alias here'></input>
                            <button className='btn btn-success form-control mt-3'>Get Shortened Link</button>
                        </form>
                    </div>
                </div>
            </div>
        ):(
            <div className='text-white'>
                <div className='vh-100 d-flex justify-content-center align-items-center w-100'>
                    <div className='row w-50 border'>
                        <div className='p-5 bg-dark'>
                            <div className='text-info h5 text-start'>Your shortened link:</div>
                            <input disabled name="url" className='form-control mt-2' value={shortLink} placeholder={shortLink}></input>
                            <div className='container d-flex justify-content-between mt-3'>
                                <div className='d-flex'>
                                    <button onMouseDown={(e) => handleVisit(e)} className='visit-btn btn mx-1 btn-primary'>
                                        <i className="bi bi-box-arrow-in-up-right me-2"></i> Visit
                                    </button>
                                    <button onClick={() => setShowQrCode(!showQrCode)} className='btn mx-1 btn-secondary'>
                                        <i className="bi bi-qr-code me-2"></i>QR
                                    </button>
                                    <button onClick={() => navigator.clipboard.writeText(shortLink)} className='btn mx-1 btn-success'>
                                    <i className="bi bi-clipboard-check-fill me-2"></i>Copy
                                    </button>
                                </div>
                                <button onClick={() => setShortLink("")} className='btn btn-warning'><i className="me-1 bi bi-plus-circle"></i> Get New Link</button>
                                {showQrCode ? (
                                    <div className='container d-flex align-items-center bg-white border' 
                                    style={{position:"fixed", top: "58%", left:"33%", width:"350px", height: "160px"}}>
                                        <div className='col-12 col-md-4'>
                                            <img 
                                            src={`data:image/png;base64,${qrCodeString}`} 
                                            alt="Red dot"
                                            width="150px"
                                            height="150px"
                                            />
                                        </div>
                                        {/* <div className='col-12 col-md-9'>
                                            <div className='justify-content-center'>
                                                <a href={`data:image/png;base64,${qrCodeString}`} 
                                                download={`shortLinkQrCode_${shortLink.substring(shortLink.lastIndexOf("/") + 1)}`} 
                                                className='m-1 btn btn-primary w-50'>PNG
                                                </a><br/>
                                                <a 
                                                href={`data:image/svg+xml;base64,${btoa(urlCode!.svgQrCodeImage)}`} 
                                                download={`shortLinkQrCode_${shortLink.substring(shortLink.lastIndexOf("/") + 1)}`}
                                                className='m-1 btn btn-secondary w-50'>SVG</a><br/>
                                                <a 
                                                href={`data:image/png;base64,${qrCodeString}`} 
                                                download={`shortLinkQrCode_${shortLink.substring(shortLink.lastIndexOf("/") + 1)}`}
                                                className='m-1 btn btn-warning w-50'>Ascii</a><br/>
                                            </div>
                                        </div> */}
                                    </div>
                                ):(<></>)}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        ))
    )
}

export default Home