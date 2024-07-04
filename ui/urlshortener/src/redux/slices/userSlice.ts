import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { user } from "../../Interfaces";
import checkForToken from "../../Helpers/checkForToken";
import jwtDecode from "jwt-decode";

const emptyUser : user = {
    id: "",
    email: "",
    username: "",
    role: ""
};

const token = checkForToken();


const currentUser = token ? (() => {
    const decodedToken : user = jwtDecode(token);
    return {
        id: decodedToken.id,
        email: decodedToken.email,
        username: decodedToken.username,
        role: decodedToken.role
    };
}):(emptyUser)

export const userSlice = createSlice({
    name: "user",
    initialState: currentUser,
    reducers:{
        setUser: (state, action : PayloadAction<user>) => {
            console.log("setting user...");
            state.id = action.payload.id;
            state.email = action.payload.email;
            state.username = action.payload.username;
            state.role = action.payload.role;
        },
        clearUser: (state) => {
            console.log("logging off now...");
            state.id = "";
            state.email = "";
            state.username = "";
            state.role = "";
        }
    }
})

export const {setUser,clearUser} = userSlice.actions;
export const userReducer = userSlice.reducer;