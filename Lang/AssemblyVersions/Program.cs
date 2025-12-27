using System.Reflection;

namespace AssemblyVersions;

class Program
{
    static void Main(string[] args)
    {
        var assembly = typeof(Program).Assembly;
        var assemblyVersion = assembly.GetName().Version;
        var infoVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
        var semVerNoMeta = infoVersion?.Split('+')[0];
        var fileVersion = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;
        var info = $"""
            Assembly Version: {assemblyVersion}
            Assembly Informational Version: {infoVersion}
            Assembly Sematic Version without metadata: {semVerNoMeta}
            Assembly File Version: {fileVersion}
            """;
        Console.WriteLine(info);
    }
}
