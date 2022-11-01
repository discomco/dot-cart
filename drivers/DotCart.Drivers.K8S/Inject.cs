using k8s;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Drivers.K8S;

public static class Inject
{
    public static IServiceCollection AddK8S(this IServiceCollection services, string filename = "")
    {
        return services
            .AddSingleton(cfg => KubernetesClientConfiguration.IsInCluster()
                ? KubernetesClientConfiguration.InClusterConfig()
                : KubernetesClientConfiguration.BuildConfigFromConfigFile(filename))
            .AddSingleton<IK8SFactory, K8SFactory>();
    }
}