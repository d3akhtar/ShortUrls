using AutoMapper;
using ShortUrlService.Helper;

namespace ShortUrlService.Profiles.Converters
{
    public class AsciiQrCodeConverter : IValueConverter<string, string>
    {
        public string Convert(string sourceMember, ResolutionContext context)
            => QrCodeStringGenerator.GetAsciiQrCodeRepresentation(sourceMember);
    }
}