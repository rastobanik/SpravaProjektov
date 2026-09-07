using Asp.Versioning;

namespace SpravaProjektovAPI.Infrastructure
{
    public static class ApiVersions
    {
        public static readonly ApiVersion V1 = new ApiVersion(1);
        public static readonly ApiVersion V2 = new ApiVersion(2);
    }
}
