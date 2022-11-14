using DotCart.Contract.Schemas;
using DotCart.Core;

namespace DotCart.TestKit;

[DbName("1")]
public record TheDoc(string Id, string Name, int Age, double Height) : IState
{
    public static NewState<TheDoc> Rand => RandomTheDoc;


    private static TheDoc RandomTheDoc()
    {
        var names = new[] { "John Lennon", "Paul McCartney", "Ringo Starr", "George Harrison" };
        var randNdx = Random.Shared.Next(names.Length);
        var name = names[randNdx];
        return new TheDoc(MyID.New.Id(), name, Random.Shared.Next(21, 80), Random.Shared.NextDouble());
    }
}