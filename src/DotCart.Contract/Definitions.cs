using DotCart.Contract.Dtos;

namespace DotCart.Contract;

public delegate THope GenerateHope<out THope>()
    where THope : IHope;

public class Definitions
{
}