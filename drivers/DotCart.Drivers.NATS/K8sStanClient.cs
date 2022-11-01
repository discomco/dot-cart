using DotCart.Drivers.K8S;
using NATS.Client;

namespace DotCart.Drivers.NATS;

internal class K8sStanClient : IK8sStanClient
{
    private readonly IK8SFactory _k8SFact;
    private readonly INatsClientConnectionFactory _natsFact;

    public K8sStanClient(INatsClientConnectionFactory natsFact, IK8SFactory k8sFact)
    {
        _natsFact = natsFact;
        _k8SFact = k8sFact;
    }
}

public interface IK8sStanClient
{
}