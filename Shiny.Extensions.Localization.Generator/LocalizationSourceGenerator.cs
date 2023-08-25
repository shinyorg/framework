using System.Text;
using Microsoft.CodeAnalysis;
using System.Resources.NetStandard;
using System.Collections.Immutable;

namespace Shiny.Extensions.Localization.Generator;


[Generator]
public class LocalizationSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
		var globalOptions = context.AnalyzerConfigOptionsProvider.Select(GlobalOptions.Select);

        var files = context
            .AdditionalTextsProvider
            .Where(static x => x.Path.EndsWith(".resx"))
			.Select((text, token) => (
				Path.GetFileNameWithoutExtension(text.Path),
				text.GetText(token)!.ToString())
			);

        var compilationAndFiles = context.CompilationProvider.Combine(files.Collect());
        context.RegisterSourceOutput(compilationAndFiles, (productionContext, sourceContext) => Generate(productionContext, sourceContext));
    }


    void Generate(SourceProductionContext context, (Compilation compilation, ImmutableArray<(string, string)> files) compilationAndFiles)
	{
		// TODO: make sure I only get the default resx files, not the localization ones
		var nameSpace = "TODO";
		var generatedClasses = new List<string>();

		foreach (var file in compilationAndFiles.files)
		{
			var associatedResourceClassName = file.Item1; // TODO: don't get the resource locale
			var className = associatedResourceClassName + "Localized";
			var generated = GenerateStronglyTypedClass(file.Item2, nameSpace, className, associatedResourceClassName);
			generatedClasses.Add(className);

			context.AddSource(className + ".g.cs", generated);
		}

		var serviceCollectionContext = GenerateServiceCollectionRegistration(nameSpace, generatedClasses);
        context.AddSource("ServiceCollectionRegistration.g.cs", serviceCollectionContext);
    }


    static string GenerateStronglyTypedClass(
		string resxFilePath,
		string nameSpace,
		string className,
		string associatedResourceClassName // name of the actual resource (BIG ASSUMPTION)
	)
	{
		var sb = new StringBuilder()
			.AppendLine("using Microsoft.Extensions.Localization;")
			.AppendLine()
			.AppendLine($"namespace {nameSpace};")
			.AppendLine()
			.AppendLine($"public class {className}")
			.AppendLine("{")
			.AppendLine("\treadonly IStringLocalizer localizer;")
			.AppendLine()
			.AppendLine($"\tpublic {className}(IStringLocalizer<{associatedResourceClassName}> localizer)")
			.AppendLine("\t{")
			.AppendLine("\t\tthis.localizer = localizer;")
			.AppendLine("\t}")
			.AppendLine();

		using (var fs = File.OpenRead(resxFilePath))
		{
			var reader = new ResXResourceReader(fs).GetEnumerator();
			while (reader.MoveNext())
			{
				var key = SafePropertyKey((string)reader.Key);
                sb.AppendLine($"\tpublic string {SafePropertyKey} => this.localizer[\"{reader.Key}\"];");
            }
		}

        
        sb.AppendLine("}");

		return sb.ToString();
	}


	static string SafePropertyKey(string keyName)
	{
		// TODO: spaces to _, other?
		return keyName;
	}


    static string GenerateServiceCollectionRegistration(string nameSpace, IEnumerable<string> generatedTypes)
    {
        var sb = new StringBuilder()
            .AppendLine("using Microsoft.Extensions.Localization;")
            .AppendLine()
            .AppendLine($"namespace {nameSpace};")
            .AppendLine()
            .Append("public static class Generated")
            .AppendLine("{")
            .AppendLine("\tpublic static void AddStrongTypedLocalizations(this Microsoft.Extensions.DependencyInjection.IServiceCollection services)")
            .AppendLine("\t{");

        foreach (var genType in generatedTypes)
        {
            sb.AppendLine($"\t\tservices.AddSingleton<{genType}>();");
        }
        sb
            .Append("\t}")
            .Append("}");

        return sb.ToString();
    }
}