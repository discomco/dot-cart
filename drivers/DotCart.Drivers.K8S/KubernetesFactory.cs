using k8s;

namespace DotCart.Drivers.K8S;

public interface IK8SFactory
{
    bool InCluster { get; }
    Kubernetes Build();
}

public class K8SFactory : IK8SFactory
{
    private readonly KubernetesClientConfiguration config;

    public K8SFactory(KubernetesClientConfiguration config)
    {
        this.config = config;
    }

    public Kubernetes Build()
    {
        return new Kubernetes(config);
    }

    public bool InCluster => KubernetesClientConfiguration.IsInCluster();
}