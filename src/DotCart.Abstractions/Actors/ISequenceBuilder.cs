namespace DotCart.Abstractions.Actors;

public interface ISequenceBuilder
{
    ICmdHandler? Build();
}