using DotCart.Abstractions.Schema;
using DotCart.Core;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.TestKit.Schema;


public static class Inject
{
    public static IServiceCollection AddTheIDCtor(this IServiceCollection services)
    {
        return services.AddTransient(_ => TheID.Ctor);
    }
}

[IDPrefix(TestConstants.TheIDPrefix)]
public record TheID : ID
{
    public TheID(string value = "") : base(TestConstants.TheIDPrefix, value)
    {
    }

    public static readonly IDCtorT<TheID> Ctor = 
        _ => New;

    public static TheID New => new("FD1A9876-158F-4EF5-A5B5-468465404551");
}