using DotCart.Abstractions.Clients;
using DotCart.Drivers.NATS;

namespace Engine.Client.Initialize;

public class Requester : RequesterT<NATSRequesterDriverT>
{
    public Requester(NATSRequesterDriverT driver) : base(driver)
    {
    }
}