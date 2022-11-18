using DotCart.Contract.Dtos;

namespace DotCart.TestKit.Schema;

public record TheHope(string AggId, byte[] Data) : Dto(AggId, Data), IHope<ThePayload>;
