import React, { useState } from 'react'
import {  } from '../api/urlShortenerApi'
import withAdmin from '../Wrapped/withAdmin';
import { MainLoader } from '../Components/Common';
import { urlCode, user, userUrlCode } from '../Interfaces';
import { UrlCodeRow } from '../Components';
import { withAuth } from '../Wrapped';
import { useGetUserUrlsQuery } from '../api/userUrlsApi';
import { useSelector } from 'react-redux';

function Urls() {
    const loggedInUser : user = useSelector((state : any) => state.userStore);
    const [formSearchQuery,setFormSearchQuery] = useState<string>("");
    const [searchQuery,setSearchQuery] = useState<string>("");
    const [pageNumber,setPageNumber] = useState<number>(1);
    const [pageSize,setPageSize] = useState<number>(5);
    const {data,isLoading,isSuccess,isError,error} = useGetUserUrlsQuery({
      userId: loggedInUser.userId,
      searchQuery: searchQuery,
      pageNumber: pageNumber,
      pageSize: pageSize
    });
    var userUrlCodes: userUrlCode[];
    if (!isLoading && isSuccess){
      userUrlCodes = data.userShortUrls;
      console.log(userUrlCodes);
    }
    if (!isSuccess && !isLoading){
        console.log(error);
    }
  return (
    <div className='vh-100 bg-dark w-100 d-flex justify-content-center' style={{overflow: "auto"}}>
      {!isLoading && isSuccess ? 
        (
          <div className='w-75'>
            <div className='row w-100' style={{height:"12%"}}></div>
            <div className='row w-100 text-start d-flex justify-content-between'>
              <div className='h2 text-info w-25'>All Urls</div>
                <form className='w-25'>
                  <input 
                    onChange={(e) => {
                      e.preventDefault();
                      setFormSearchQuery(e.target.value);
                    }}
                    onKeyDown={(e) => {
                      if (e.key == "Enter"){
                        e.preventDefault();
                        setSearchQuery(e.currentTarget.value);
                      }
                    }} 
                    className='form-control' 
                    placeholder='Search' 
                    value={formSearchQuery}>
                  </input>
                </form>
            </div>
            <div className='row w-100 text-start'>
              <div className='p-2 col-12 col-md-2 border'>
                <span className='text-info h3'>QR</span>
              </div>
              <div className='p-2 col-12 col-md-2 border'>
                <span className='text-info h3'>Code</span>
              </div>
              <div className='p-2 col-12 col-md-7 border'>
                <span className='text-info h3'>Url</span>
              </div>
              <div className='p-2 col-12 col-md-1 border'>
                <span className='text-info h3'>Actions</span>
              </div>
            </div>
            {
              userUrlCodes!.map((userUrlCode: userUrlCode, i:number) => {
                return i < 10 ? 
                  (<UrlCodeRow pngQrCodeImage={userUrlCode.pngQrCodeImage!} shortUrlCode={userUrlCode.shortUrlCode} destinationUrl={userUrlCode.destinationUrl} isUserUrlCode={true} key={i}/>):(<></>)
              })
            }
            <div className='row mt-3 w-100 text-start'>
              <div className='col-12 col-md-2'>
                <div className='btn-group d-flex'>
                  <button disabled={pageNumber == 1} onClick={() => setPageNumber(pageNumber == 1 ? 1:pageNumber - 1)} className='btn me-1 btn-light'>{"<"}</button>
                  <button disabled={userUrlCodes!.length < pageSize} onClick={() => setPageNumber(pageNumber + 1)} className='btn btn-light'>{">"}</button>
                </div>
              </div>
              <div className='col-12 col-md-3 offset-7 mb-5 d-flex'>
                <form className='me-4'>
                  <input min="1" type="number" onChange={(e) => setPageNumber(Number(e.target.value) < 1 || e.target.value == null ? 1:Number(e.target.value))} className='form-control' placeholder='Page Number' value={pageNumber}></input>
                </form>
                <select onChange={(e) => setPageSize(Number(e.target.value))} className='w-100' value={pageSize}>
                  <option>5</option>
                  <option>10</option>
                  <option>25</option>
                  <option>50</option>
                </select>
                <span className='text-white w-100 ms-1 d-flex align-items-center'>Pages per entry</span>
              </div>
            </div>
          </div>
        ):
        (
          <MainLoader/>
        )
      }
    </div>
  )
}

export default withAuth(Urls);