import {createApi,fetchBaseQuery} from "@reduxjs/toolkit/query/react";

export const authApi = createApi({
    reducerPath: "authApi",
    baseQuery: fetchBaseQuery({
        baseUrl: "https://shorturls.danyalakt.com/auth/"
        //baseUrl: "http://localhost:5219/api/auth/"
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
        }),
        forgotPassword: builder.mutation({
            query: (email : string) => ({
                url: "forgot-password",
                method: "POST",
                headers:{
                    "Content-type": "application/json"
                },
                params:{
                    email
                }
            }),
        }),
        resetPassword: builder.mutation({
            query: (body : any) => ({
                url: "reset-password",
                method: "POST",
                headers:{
                    "Content-type": "application/json"
                },
                body: body
            })
        }), 
    })
})

export const {useLoginMutation,useRegisterMutation,useForgotPasswordMutation,useResetPasswordMutation} = authApi;
export default authApi;