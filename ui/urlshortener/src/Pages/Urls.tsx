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
    <div className='vh-100 bg-light w-100 d-flex justify-content-center' style={{overflow: "auto"}}>
      {!isLoading && isSuccess ? 
        (
          <div className='px-5 py-2 border bg-white shadow-sm' style={{width:"80%"}}>
            <div className='row w-100' style={{height:"12%"}}></div>
            <div className='row w-100 text-start d-flex justify-content-between'>
              <div className='h2 text-dark w-25'>My Urls</div>
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
            <div className='row bg-light w-100 text-start border'>
              <div className='p-2 col-12 col-md-3'>
                <span className='text-dark h3'>QR</span>
              </div>
              <div className='p-2 col-12 col-md-3'>
                <span className='text-dark h3'>Code</span>
              </div>
              <div className='p-2 col-12 col-md-5'>
                <span className='text-dark h3'>Url</span>
              </div>
              <div className='p-2 col-12 col-md-1'>
                <span className='text-dark h3'></span>
              </div>
            </div>
            {
              userUrlCodes!.map((userUrlCode: userUrlCode, i:number) => {
                return i < 10 ? 
                  (<UrlCodeRow shortUrl={userUrlCode} shortUrlCode={userUrlCode.shortUrlCode} destinationUrl={userUrlCode.destinationUrl} isUserUrlCode={true} key={i}/>):(<></>)
              })
            }
            <div className='row mt-3 w-100 text-start'>
              <div className='col-12 col-md-2'>
                <div className='btn-group d-flex'>
                  <button disabled={pageNumber == 1} onClick={() => setPageNumber(pageNumber == 1 ? 1:pageNumber - 1)} className='btn me-1 btn-dark'>{"<"}</button>
                  <button disabled={userUrlCodes!.length < pageSize} onClick={() => setPageNumber(pageNumber + 1)} className='btn btn-dark'>{">"}</button>
                </div>
              </div>
              <div className='col-12 col-md-3 mb-5 d-flex'>
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