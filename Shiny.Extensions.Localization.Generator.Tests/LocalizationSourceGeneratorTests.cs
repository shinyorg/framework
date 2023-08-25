using Microsoft.Extensions.DependencyInjection;
using Xunit;
using TODO;
using FluentAssertions;


namespace Shiny.Extensions.Localization.Generator.Tests;


public class LocalizationSourceGeneratorTests
{

    [Fact]
    public void EndToEndTest()
    {
        var services = new ServiceCollection();
        services.AddLocalization();
        services.AddStrongTypedLocalizations();
        var sp = services.BuildServiceProvider();

        sp.GetRequiredService<MyClassLocalized>().Should().NotBeNull("MyClass localization missing in registration");
        sp.GetRequiredService<FolderTest1Localized>().Should().NotBeNull("FolderTest1 localization missing in registration");
        sp.GetRequiredService<FolderTest2Localized>().Should().NotBeNull("FolderTest2 localization missing in registration");

        // TODO: ensure keys are set
        // TODO: check keys with spaces
    }
}