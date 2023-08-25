using Microsoft.CodeAnalysis.Diagnostics;

namespace Shiny.Extensions.Localization.Generator;


public record GlobalOptions
{
    public string? RootNamespace { get; }
    public string? ProjectFullPath { get; }
    public string? ProjectName { get; }


    public GlobalOptions(AnalyzerConfigOptions options)
    {
        if (options.TryGetValue("build_property.MSBuildProjectFullPath", out var projectFullPath))
            this.ProjectFullPath = projectFullPath;

        if (options.TryGetValue("build_property.RootNamespace", out var rootNamespace))     
            this.RootNamespace = rootNamespace;

        if (options.TryGetValue("build_property.MSBuildProjectName", out var projectName))
            this.ProjectName = projectName;
    }


    public static GlobalOptions Select(AnalyzerConfigOptionsProvider provider, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        return new GlobalOptions(provider.GlobalOptions);
    }
}