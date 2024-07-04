import {createApi,fetchBaseQuery} from "@reduxjs/toolkit/query/react";

export const authApi = createApi({
    reducerPath: "authApi",
    baseQuery: fetchBaseQuery({
        baseUrl: "http://shorturl.com/api/auth/"
    }),
    endpoints: (builder) => ({
       login: builder.mutation({
            query: (body : any) => ({
                url: "login",
                method: "POST",
                headers:{
                    "Content-type": "application/json"
                },
                body: body
            }),
       }),
       register: builder.mutation({
        query: (body : any) => ({
            url: "register",
            method: "POST",
            headers:{
                "Content-type": "application/json"
            },
            body: body
        }),
   }) 
    })
})

export const {useLoginMutation,useRegisterMutation} = authApi;
export default authApi;