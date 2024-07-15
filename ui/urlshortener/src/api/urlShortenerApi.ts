import {createApi,fetchBaseQuery} from "@reduxjs/toolkit/query/react";
import { SD_General } from "../constants/constants";
import { updateUrlCodeLinkBody } from "../Interfaces/RequestBody";
import { getAllArgs } from "../Interfaces";

export const urlShortenerApi = createApi({
    reducerPath: "urlShortenerApi",
    baseQuery: fetchBaseQuery({
        baseUrl: "http://shorturls.danyalakt.com/shorturl/",
        prepareHeaders:(headers: Headers, api) => {
            const token = localStorage.getItem(SD_General.tokenKey);
            token && headers.append("Authorization","Bearer " + token); // Pass token so [Authorize] and [Authenticate] can check if user has permission
            headers.append('Access-Control-Allow-Origin', 'http://localhost:3000');
        }
    }),
    tagTypes: ["Urls"],
    endpoints: (builder) => ({
        getAllUrls: builder.query({
            query: (body : any) => ({
                url: ``,
                params:{
                    searchQuery: body.searchQuery,
                    pageNumber: body.pageNumber,
                    pageSize: body.pageSize
                }
            }),
            providesTags: ["Urls"]
       }),
        getUrl: builder.query({
            query: (code: string) => ({
                url: `${code}`
            }),
            providesTags: ["Urls"]
       }),
       addUrl: builder.mutation({
            query: (body : any) => ({
                url: "",
                method: "POST",
                params:{
                    url: body.url,
                    alias: body.alias
                }
            }),
            invalidatesTags: ["Urls"],
       }),
       deleteUrl: builder.mutation({
            query: (code: string) => ({
                url: `${code}`,
                method: "DELETE"
            }),
            invalidatesTags: ["Urls"]
       }),
       updateUrlLink: builder.mutation({
        query: (body: updateUrlCodeLinkBody) => ({
            url: `${body.code}`,
            method: "PUT",
            params:{
                newDestinationUrl: body.newUrl
            }
        }),
        invalidatesTags: ["Urls"]
   }),  
    })
})

export const {useGetAllUrlsQuery,useGetUrlQuery,useAddUrlMutation,useDeleteUrlMutation,useUpdateUrlLinkMutation} = urlShortenerApi;
export default urlShortenerApi;