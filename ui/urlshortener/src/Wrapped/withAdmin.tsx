import jwtDecode from "jwt-decode";
import { SD_General, SD_ROLES } from "../constants/constants";
import { user } from "../Interfaces";

const withAdmin = (WrappedComponent : any) => {
    return(props: any) => {
        const token = localStorage.getItem(SD_General.tokenKey);
        if (!token){
            window.location.replace("/UrlShortener/Login")
            return null; 
        }
        else{
            const decodedToken : user = jwtDecode(token);
            if (decodedToken.role == SD_ROLES.Admin){
                return <WrappedComponent {...props}/>
            }
            else{
                window.location.replace("/UrlShortener/AccessDenied")
                return null; 
            }
        }
    }
}

export default withAdmin;