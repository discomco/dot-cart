using DotCart.Context.Abstractions;
using DotCart.Context.Spokes;

namespace DotCart.Drivers.Mediator.Tests;

public class Spoke : SpokeT<Spoke>
{
    public Spoke(IExchange exchange) : base(exchange)
    {
    }
}