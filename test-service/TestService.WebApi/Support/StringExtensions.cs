using System.Diagnostics.CodeAnalysis;

namespace TestService.WebApi.Support
{
    public static class StringExtensions
    {
        [ExcludeFromCodeCoverage]
        public static int AsPortNumberOrDefaultTo(this string port, int @default)
        {
            return int.TryParse(port, out var result) ? result : @default;
        }

        [ExcludeFromCodeCoverage]
        public static string StripTrailingSlash(this string value)
        {
            return value?.TrimEnd(new[] { '/' });
        }
    }
}
