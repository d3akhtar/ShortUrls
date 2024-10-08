import urlCode from "./urlCode";

export default interface userUrlCode {
    userId: string,
    shortUrlCode: string
    destinationUrl: string
    pngQrCodeImage?: string;
    svgQrCodeImage?: string;
    asciiQrCodeImage?: string;
}