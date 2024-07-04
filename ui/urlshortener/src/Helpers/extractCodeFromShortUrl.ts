const extractCodeFromShortUrl = (shortUrl:string) => {
    const splitShortUrl = shortUrl.split("/");
    return splitShortUrl[splitShortUrl.length - 1];
}

export default extractCodeFromShortUrl;