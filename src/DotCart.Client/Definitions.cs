using DotCart.Client.Contracts;

namespace DotCart.Client;

public delegate THope GenerateHope<out THope>()
    where THope : IHope;

public class Definitions
{
}