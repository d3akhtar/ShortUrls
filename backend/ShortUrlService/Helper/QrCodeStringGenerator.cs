using QRCoder;

namespace ShortUrlService.Helper
{
    public static class QrCodeStringGenerator
    {
        private static string shortenedUrlBase;

        public static void SetShortenedUrlBase(string url)
        {
            shortenedUrlBase = url;
        }
        public static string GetPngQrCodeImage(string code)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(shortenedUrlBase + code, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeImage = qrCode.GetGraphic(20);
            return Convert.ToBase64String(qrCodeImage);
        }

        public static string GetSvgQrCodeImage(string code)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(shortenedUrlBase + code, QRCodeGenerator.ECCLevel.Q);
            SvgQRCode qrCode = new SvgQRCode(qrCodeData);
            string qrCodeAsSvg = qrCode.GetGraphic(20);
            return qrCodeAsSvg;
        }

        public static string GetAsciiQrCodeRepresentation(string code)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(shortenedUrlBase + code, QRCodeGenerator.ECCLevel.Q);
            AsciiQRCode qrCode = new AsciiQRCode(qrCodeData);
            string qrCodeAsAsciiArt = qrCode.GetGraphic(1);
            return qrCodeAsAsciiArt;
        }
    }
}