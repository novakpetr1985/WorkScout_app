using System.Reflection;

namespace JobSearchApp.Services
{
    public static class ApplicationVersionService
    {
        public static string Current { get; } = ResolveVersion();

        public static string DisplayName => $"WorkScout {Current}";

        private static string ResolveVersion()
        {
            var version = typeof(ApplicationVersionService).Assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                .InformationalVersion;

            return string.IsNullOrWhiteSpace(version)
                ? "unknown"
                : version.Split('+')[0];
        }
    }
}
