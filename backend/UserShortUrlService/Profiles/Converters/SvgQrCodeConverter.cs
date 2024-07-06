using AutoMapper;
using ShortUrlService.Helper;

namespace ShortUrlService.Profiles.Converters
{
    public class SvgQrCodeConverter : IValueConverter<string, string>
    {
        public string Convert(string sourceMember, ResolutionContext context)
            => QrCodeStringGenerator.GetSvgQrCodeImage(sourceMember);
    }
}