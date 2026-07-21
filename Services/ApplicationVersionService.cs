using System.Reflection;

namespace WorkScout.Services
{
    /// <summary>
    /// FEATURE: VERSION DISPLAY — čte informační verzi sestavení a odstraňuje pouze
    /// technický commit hash za znakem +, aby UI zachovalo suffix feature/DEV/TEST.
    /// </summary>
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
