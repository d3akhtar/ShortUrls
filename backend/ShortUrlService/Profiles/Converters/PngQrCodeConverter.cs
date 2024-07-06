using AutoMapper;
using ShortUrlService.Helper;

namespace ShortUrlService.Profiles.Converters
{
    public class PngQrCodeConverter : IValueConverter<string, string>
    {
        public string Convert(string sourceMember, ResolutionContext context)
            => QrCodeStringGenerator.GetPngQrCodeImage(sourceMember);
    }
}