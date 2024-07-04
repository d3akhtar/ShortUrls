import React, { useState } from 'react'
import { apiResponse, urlCode, user } from '../Interfaces'
import { useDeleteUrlMutation, useUpdateUrlLinkMutation } from '../api/urlShortenerApi'
import { MainLoader } from './Common'
import { useSelector } from 'react-redux'
import { useDeleteUserUrlMutation } from '../api/userUrlsApi'
import { inputHelper } from '../Helpers'
import { updateAliasBody, updateUrlCodeLinkBody } from '../Interfaces/RequestBody'

interface UrlCodeRowProps {
    shortUrlCode: string
    destinationUrl: string,
    isUserUrlCode: boolean
}

function UrlCodeRow(props: UrlCodeRowProps) {

  const loggedInUser : user = useSelector((state : any) => state.userStore);
  const [deleteUrlCode] = useDeleteUrlMutation();
  const [deleteUserUrl] = useDeleteUserUrlMutation();
  const [editUrlCodeLink] = useUpdateUrlLinkMutation();
  const [isLoading,setIsLoading] = useState<boolean>(false);
  const [isEditing,setIsEditing] = useState<boolean>(false);
  
  const initialEditData = {
    code: props.shortUrlCode,
    link: props.destinationUrl,
  }
  const [editData,setEditData] = useState(initialEditData)

  const handleDelete = async () => {
    setIsLoading(true);

    var result : any;
    if (props.isUserUrlCode){
      result = await deleteUserUrl({
        userId: loggedInUser.id,
        code: props.shortUrlCode,
      });
    }
    else{
      result = deleteUrlCode(props.shortUrlCode)
    }

    const response : apiResponse = result.error ? (result.error.data):(result.data);
    console.log("response");
    console.log(response);

    setIsLoading(false);
  }

  const handleEdit = async () => {
    var result : any;
    const body : updateUrlCodeLinkBody = {
      code: props.shortUrlCode,
      newUrl: editData.link,
    };
    result = await editUrlCodeLink(body);

    const response : apiResponse = result.error ? (result.error.data):(result.data);
    if (!result.error){
      setIsEditing(false);
    }
    else{
      console.log(response);
    }
  }

  return (
      <div>
        {isLoading ?
          (<MainLoader/>):
          (
          <div className='row w-100 border text-start'>
            <div className='p-1 col-12 col-md-2 d-flex align-items-center justify-content-center' style={{borderRight:"1px solid white"}}>
            <img 
            src={`data:image/png;base64,`} 
            alt="Red dot"
            width="100px"
            height="100px"
            className='p-1'
            />
            </div>
            <div className='p-1 col-12 col-md-2 d-flex align-items-center' style={{borderRight:"1px solid white"}}>
              <span className='ms-1 text-light'>{props.shortUrlCode}</span>
            </div>
            <div className='p-1 col-12 col-md-7 d-flex align-items-center' style={{borderRight:"1px solid white"}}>
              {isEditing ? 
                (
                  <input onChange={(e) => setEditData(inputHelper(e, editData))} className='ms-1 form-control' name='link' value={editData.link}></input>
                ):
                (
                  <a className='ms-1 text-light' href={props.destinationUrl}>{props.destinationUrl}</a>
                )
              }
            </div>
            <div className='p-1 col-12 col-md-1 d-flex justify-content-center align-items-center'>
              <div className='p-1 btn-group d-flex justify-content-center align-items-center'>
                {!props.isUserUrlCode ? 
                (<button onClick={() => setIsEditing(!isEditing)} className={`btn btn-${isEditing ? "danger":"warning"}`}><i className={`bi ${!isEditing ? "bi-pencil-square":"bi-x-lg"}`}></i></button>):(<></>)}
                {!isEditing ? 
                  (
                    <button onClick={handleDelete} className='btn btn-danger'><i className="bi bi-trash-fill"></i></button> 
                  )
                  :
                  (
                    <button onClick={handleEdit} className='btn btn-success'><i className="bi bi-check-square-fill"></i></button>                   
                  )
                }
              </div>
            </div>
          </div>
        )
        }
      </div>
  )
}

export default UrlCodeRow