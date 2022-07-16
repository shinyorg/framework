using DryIoc;
using Shiny.Framework;

namespace Shiny.Framework.Tests;


public class DryIocTests
{
    [Fact]
    public void Test1()
    {
        var container = new Container(Rules.MicrosoftDependencyInjectionRules);
        container.Register<BaseServices>
        //builder
        //    .UseShiny()
        //    .UsePrismApp<TApp>(container, prismBuilder);

        //builder.Services.AddScoped<BaseServices>();
        //builder.Services.TryAddScoped<IDialogs, NativeDialogs>();

        //builder.Services.TryAddSingleton(AppInfo.Current);
        //builder.Services.TryAddSingleton(Connectivity.Current);
    }
}
