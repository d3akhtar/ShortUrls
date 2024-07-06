import urlCode from "./urlCode";
import user from "./user";

export default interface apiResponse {
    message?: string;
    token?: string
    user?: user;
    shortenedUrl? : string;
    shortenedUrlWithAlias? : string;
    shortUrl?: urlCode;
    shortUrlWithAlias?: urlCode;
}