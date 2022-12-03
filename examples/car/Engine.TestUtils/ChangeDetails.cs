using DotCart.Abstractions.Schema;

namespace Engine.TestUtils;

public static class ChangeDetails
{
    public static PayloadCtorT<
            Contract.ChangeDetails.Payload>
        PayloadCtor =
            () => Contract.ChangeDetails.Payload.New(Contract.Schema.Details.New("Engine #2", "A V8 Merlin engine."));

    public static HopeCtorT<
            Contract.ChangeDetails.Hope,
            Contract.ChangeDetails.Payload>
        HopeCtor =
            (_, _) => Contract.ChangeDetails.Hope.New(Schema.IDCtor().Id(), PayloadCtor());

    public static FactCtorT<
            Contract.ChangeDetails.Fact,
            Contract.ChangeDetails.Payload>
        FactCtor =
            (_, _) => Contract.ChangeDetails.Fact.New(Schema.IDCtor().Id(), PayloadCtor());
}