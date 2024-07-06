export default interface urlCode{
    code: string,
    destinationUrl: string
    isAlias: boolean
    pngQrCodeImage?: string;
    svgQrCodeImage?: string;
    asciiQrCodeImage?: string;
}